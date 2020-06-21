module.exports = {
  dialect: 'postgres',
  host: 'localhost',
  username: 'postgres',
  password: 'docker',
  database: 'Barber',
  define: {
    timestamps: true,
    underscored: true,
    uderscoredAll: true,
  },
}