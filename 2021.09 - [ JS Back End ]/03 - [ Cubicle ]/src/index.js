//**************************** Import Modules **********************
const express = require('express');

const app = express();
const config = require('./config/config');

//**************************** Setups **********************
require('./config/handlebars')(app);
require('./config/express')(app);
require('./config/router')(app);
require('./config/database')(config.DB_CONNECTION_STRING);

app.listen(config.PORT, console.log.bind(console, `Application is running on http://localhost:${config.PORT}`));

// index -> routes -> controller -> service -> database -> END
// database -> service -> controller -> handlebars -> browser -> END