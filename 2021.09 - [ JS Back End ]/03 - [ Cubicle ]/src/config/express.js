//**************************** Import Modules **********************
const express = require('express');
const path = require('path');

//*********************** Setup **********************
const setupExpress = (app) => {
    //**************************** Middleware **********************
    app.use(express.urlencoded({ extended: true })); // support parsing of application/x-www-form-urlencoded post data
    app.use(express.static(path.resolve(__dirname, '../public'))); // serving static files
};

module.exports = setupExpress;