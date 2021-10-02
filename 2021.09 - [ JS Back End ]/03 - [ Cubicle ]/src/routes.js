//**************************** Import Modules **********************
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
