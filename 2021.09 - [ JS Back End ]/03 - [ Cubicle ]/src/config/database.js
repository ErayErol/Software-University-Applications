//**************************** Import Modules **********************
const mongoose = require('mongoose');

//*********************** Setup **********************
function initDatabase(connectionString) {
    return mongoose.connect(connectionString);
} 

module.exports = initDatabase;