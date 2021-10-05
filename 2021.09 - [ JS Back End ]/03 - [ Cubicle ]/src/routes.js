//**************************** Import Modules **********************
// This function is used when you want to create a new router object in your program to handle requests
const router = require('express').Router();

//**************************** Import Controllers **********************
const homeController = require('./controllers/homeController');
const cubeController = require('./controllers/cubeController');
const accessoryController = require('./controllers/accessoryController');

//**************************** Middleware **********************
router.use(homeController);
router.use('/cube', cubeController);
router.use('/accessory', accessoryController);
router.use('*', (req, res) => {
    res.render('404');
});

module.exports = router;
