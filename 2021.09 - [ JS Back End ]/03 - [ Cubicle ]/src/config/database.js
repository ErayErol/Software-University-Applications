//**************************** Import Modules **********************
const mongoose = require('mongoose');

//*********************** Setup **********************
const initDatabase = (connectionString) => {
    mongoose.connect(connectionString, {useNewUrlParser: true, useUnifiedTopology: true});

    const db = mongoose.connection;

    db.on('error', console.error.bind(console, 'connection error:'));
    db.once('open', console.log.bind(console, 'Db Connected!'));
};

module.exports = initDatabase;