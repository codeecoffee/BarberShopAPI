import { format, parseISO } from 'date-fns'
import enUS from 'date-fns/locale/en-US'
import Mail from '../../lib/Mail'


class CancellationMail {
  get key(){
    return 'CancellationMail'
  }

  async handle({ data }){
    const { appointment } = data
    await Mail.sendMail({
      to: `${appointment.provider.name}<${appointment.provider.email}>`,
      subject: 'Appointment canceled',
      template: 'cancellation',
      context: {
        provider: appointment.provider.name,
        user: appointment.user.name,
        date: format(parseISO(appointment.date), "MMMM ',' dd ' at ' H:MM 'h' ", {
          locale: enUS
        })
      }
    })
  }
}

export default new CancellationMail()