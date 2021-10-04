const routes = require('../routes');

//*********************** Setup **********************
const setupRouter = (app) => {
    //**************************** Middleware **********************
    app.use(routes);
};

module.exports = setupRouter;