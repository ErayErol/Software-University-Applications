//**************************** Import Modules **********************
const handlebars = require('express-handlebars');
const path = require('path');

//*********************** Setup **********************
const initHandlebars = (app) => {
    const hbs = handlebars({
        extname: 'hbs',
    });
    
    app.set('views', path.resolve(__dirname, '../views'));
    app.engine('hbs', hbs);
    app.set('view engine', 'hbs');
};

module.exports = initHandlebars;
