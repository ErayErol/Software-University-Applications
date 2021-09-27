const homeHandler = require('./home');
const staticFiles = require('./static-handler');
const catHandler = require('./cat');

module.exports = [homeHandler, staticFiles, catHandler];