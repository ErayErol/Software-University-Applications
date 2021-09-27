const port = 3000;
const express = require('express');
const app = express();
const path = require('path');
const handlebars = require('express-handlebars');
const upload = require('express-fileupload');
const bodyParser = require('body-parser'); // extracts the entire body portion of an incoming request stream and exposes it on req.body

//**************************** Import controllers ****************
const catsController = require('./controllers/catsController.js');
const homeController = require('./controllers/homeController.js');

//**************************** Handlebars setup ****************
app.engine('.hbs', handlebars({
    extname: '.hbs',
    partialsDir: [
        //  path to your partials
        path.join(__dirname, 'views/partials'),
    ]
}));

app.set('view engine', '.hbs'); // Set file extension

//**************************** Middleware **********************
app.use(express.static('./public')); // serving static files
app.use(bodyParser.urlencoded({ extended: false })); // support parsing of application/x-www-form-urlencoded post data
app.use(bodyParser.json()); // support parsing of application/json type post data
app.use(upload()); // Upload files (like jpg, png...)
app.use('/cats', catsController); // define route handlers
app.use('/', homeController); // define route handlers

app.listen(port, () => console.log(`Express running on port: ${port}...`));
