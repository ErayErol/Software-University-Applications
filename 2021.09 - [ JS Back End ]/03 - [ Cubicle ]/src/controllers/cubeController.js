// 1. Controller
// 1.1 is responsible to receive user input and decide what to do
// 1.2 determines what response to send back to a user when a user makes a browser request.

//**************************** Import Modules **********************
const router = require('express').Router();

//**************************** Import Services **********************
const cubeService = require('../services/cubeService');
const cubeAccessoryController = require('./cubeAccessoryController');

//**************************** Functions **********************
const getCreateCubePage = (req, res) => {
    res.render('cube/create');
};

const createCube = async (req, res) => {
    let { name, description, imageUrl, difficulty } = req.body;

    try {
        await cubeService.create(name, description, imageUrl, difficulty);

        res.redirect('/');
    } catch (error) {
        res.status(400).send(error.message).end();
    }
};

const cubeDetails = async (req, res) => {
    let cube = await cubeService.getOne(req.params.cubeId);

    res.render('cube/details', { ...cube });
};

// Router handle requests
router.get('/create', getCreateCubePage); // handle requests '/create' and execute function getCreateCubePage
router.post('/create', createCube);
router.get('/:cubeId', cubeDetails);
router.use('/:cubeId/accessory', cubeAccessoryController);

module.exports = router;
