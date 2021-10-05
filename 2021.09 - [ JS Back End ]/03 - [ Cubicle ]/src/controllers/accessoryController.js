// 1. Controller
// 1.1 is responsible to receive user input and decide what to do
// 1.2 determines what response to send back to a user when a user makes a browser request.

//**************************** Import Modules **********************
const router = require('express').Router();

//**************************** Import Services **********************
const accessoryService = require('../services/accessoryService');

//**************************** Functions **********************
router.get('/create', (req, res) => {
    res.render('accessory/create');
});

router.post('/create', async (req, res) => {
    let { name, description, imageUrl } = req.body;

    await accessoryService.create(name, description, imageUrl);

    res.redirect('/');
});

module.exports = router;