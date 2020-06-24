import * as Yup from 'yup'
import { startOfHour, parseISO, isBefore, format, subHours } from 'date-fns'
import { enUS } from 'date-fns/locale/en-US'

import Appointment from '../models/Appointment'
import Notification from '../schemas/Notification'
import User from '../models/User'
import File from '../models/File'

import Queue from '../../lib/Queue'
import CancellationMail from '../jobs/CancellationMail'

class AppointmentController {
  async index(req,res){
    const { page = 1} = req.query;
    const appointments = await Appointment.findAll({
      where: {
        user_id: req.userId,
        canceled_at: null
      },
      order:['date'],
      attributes: ['id', 'date', 'past', 'cancelable'],
      limit: 20,
      offset: (page-1) * 20,
      include: [{
        model: User,
        as: 'provider',
        attributes: ['id', 'name'],
        include: [{
          model: File,
          as: 'avatar',
          attributes: ['id','path','url']
        }]
      }]
    })
    return res.json(appointments)
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      provider_id: Yup.number().required(),
      date: Yup.date().required(),
    })
    if(!(await schema.isValid(req.body))){
      return res.status(400).json({ error: 'Validation failed' })
    }
    const {provider_id, date } = req.body
    const isProvider = await User.findOne({
      where: {
        id: provider_id,
        provider: true,
      }
    })

    if(!isProvider)
      return res
        .status(401)
        .json({ error: 'You can only book with a provider'})


    // Check past dates
    const hourStart = startOfHour(parseISO(date))
    
    if(isBefore(hourStart, new Date())){
      return res.status(400).json({ error: 'Past dates are not allowed'})
    }

    //Check availability
    const checkAvailability = await Appointment.findOne({
      where: {
        provider_id,
        canceled_at: null,
        date: hourStart,
      }
    })

    if(checkAvailability){
      return res.status(400).json({ error: 'Date is not available'})
    }
    const appointment = await Appointment.create({
     user_id: req.userId,
     provider_id,
     date,
    }) 
    
    //Provider notification 
    const user = await User.findByPk(req.userId)
    const formattedDate = format(
      hourStart,
      "MMMM ',' dd ' at ' H:MM 'h' ",
      {locale: enUS}
    )
    await Notification.create({
      
      content: `New appointment from ${user.name} on ${formattedDate}`,
      user: provider_id,
    })

   return res.json(appointment)
  }
  async delete(req, res){
    const appointment= await Appointment.findByPk(req.params.id, {
      include: [
        {
          model: User,
          as: 'provider',
          attributes: ['name', 'email']
        }
      ]
    })

    if(appointment.user_id !== req.userId){
      res.status(401).json({ error: 'You can only cancel your own appointments.'})
    }
    const dateWithSub = subHours(appointment.date, 2 )

    if(isBefore(dateWithSub, new Date())){
      return res.status(401).json({ error: 'Appointments can only be canceled until 2 hours in advance'})
    }
    appointment.canceled_at = new Date()
    await appointment.save()
    await Queue.add(CancellationMail.key,{
      appointment
    })
    return res.json(appointment)
  }
}

export default new AppointmentController()