const databaseName = 'cubes';

const config = {
    development: {
        PORT: 3000,
        DB_CONNECTION_STRING: `mongodb://localhost/${databaseName}`
    },
    production: {
        PORT: 80,
        DB_CONNECTION_STRING: 'mongodb+srv://pesho:GtbCkhgBiYPQDdvf@test.ioq0t.mongodb.net/myFirstDatabase?retryWrites=true&w=majority'
    }
};

module.exports = config[process.env.NODE_ENV.trim()];
