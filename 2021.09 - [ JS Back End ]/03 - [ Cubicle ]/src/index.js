//**************************** Import Modules **********************
const app = require('express')(); // create an express app

const config = require('./config/config');

//**************************** Setups **********************
require('./config/handlebars')(app);
require('./config/express')(app);
require('./config/router')(app);
require('./config/database')(config.DB_CONNECTION_STRING)
    .catch(err => console.log(err));

app.listen(config.PORT, console.log.bind(console, `Application is running on http://localhost:${config.PORT}`));

// Client(browser) -> http request -> index -> routes -> controller -> service -> database -> END
// http response -> database -> service -> controller -> handlebars -> Client(browser)