//BD connection 
import Sequelize from 'sequelize'
import DBconfig from '../config/database'
import User from '../app/models/User'
import File from '../app/models/File'

const models = [User, File]
class Database{
  constructor(){
    this.init()
  }
  init(){
    this.connection = new Sequelize(DBconfig)

    models
    .map(model=> model.init(this.connection))
    .map(model=> model.associate && model.associate(this.connection.models))
  }
}

export default new Database();