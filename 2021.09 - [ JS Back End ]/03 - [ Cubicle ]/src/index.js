//**************************** Import Modules **********************
const express = require('express');
const path = require('path');

//**************************** Setups **********************
const app = express();
require('./config/handlebars')(app);
const routes = require('./routes');
const config = require('./config/config.json')[process.env.NODE_ENV];
const initDatabase = require('./config/database');

//**************************** Middleware **********************
app.use(express.urlencoded({extended: true})); // support parsing of application/x-www-form-urlencoded post data
app.use(express.static(path.resolve(__dirname, './public'))); // serving static files
app.use(routes);

initDatabase(config.DB_CONNECTION_STRING)
    .then(() => {
        app.listen(config.PORT, console.log.bind(console, `Application is running on http://localhost:${config.PORT}`));
    })
    .catch(err => {
        console.log('Application init failed: ', err);
    });

// index -> routes -> controller -> service -> database -> END
// database -> service -> controller -> handlebars -> browser -> END