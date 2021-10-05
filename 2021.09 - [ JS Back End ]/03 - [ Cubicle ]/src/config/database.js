//**************************** Import Modules **********************
const mongoose = require('mongoose');

//*********************** Setup **********************
const initDatabase = async (connectionString) => {
    await mongoose.connect(connectionString);
    console.log('DB Connected!');
};

module.exports = initDatabase;