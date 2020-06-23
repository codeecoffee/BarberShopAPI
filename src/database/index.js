//BD connection 
import Sequelize from 'sequelize'
import mongoose from 'mongoose'
import DBconfig from '../config/database'

import User from '../app/models/User'
import File from '../app/models/File'
import Appointment from '../app/models/Appointment'

const models = [User, File, Appointment]
class Database{
  constructor(){
    this.init()
    this.mongo()
  }
  init(){
    this.connection = new Sequelize(DBconfig)

    models
    .map(model=> model.init(this.connection))
    .map(model=> model.associate && model.associate(this.connection.models))
  }
  mongo(){
    this.mongoConnetion = mongoose.connect(
      'mongodb://localhost:27017/barber',
      {
        useNewUrlParser: true,
        useFindAndModify: true
      }
    )
  }
}

export default new Database();