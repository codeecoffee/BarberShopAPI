import { Router } from 'express'

import multer from 'multer'
import multerConfig from './config/multer'

import UserController from './app/controllers/UserController'
import SessionController from './app/controllers/SessionController'
import FileController from './app/controllers/FileController'
import ProvidersController from './app/controllers/ProvidersController'
import AppointmentController from './app/controllers/AppointmentController'
import ScheduleController from './app/controllers/ScheduleController'
import NotificationController from './app/controllers/NotificationController'

import authMiddleware from './app/middleware/auth'

const routes = new Router()
const upload = multer(multerConfig)

routes.post('/users', UserController.store)
routes.post('/sessions', SessionController.store)

routes.use(authMiddleware)

routes.put('/users', UserController.update)

routes.get('/providers',ProvidersController.index)

routes.get('/appointments', AppointmentController.index)
routes.post('/appointments', AppointmentController.store)

routes.get('/schedule', ScheduleController.index)

routes.get('/notifications', NotificationController.index)
routes.put('/notifications/:id', NotificationController.update)

routes.post('/files', upload.single('file'), FileController.store)

export default routes