// 1. Controller
// 1.1 is responsible to receive user input and decide what to do
// 1.2 determines what response to send back to a user when a user makes a browser request.

//**************************** Import Modules **********************
const router = require('express').Router();

//**************************** Import Services **********************
const cubeService = require('../services/cubeService');

//**************************** Functions **********************
const home = async (req, res) => {
    let cubes = await cubeService.getAll();

    res.render('index', { cubes });
};

const about = (req, res) => {
    res.render('about');
};

const search = async (req, res) => {
    let { search, from, to } = req.query;

    let cubes = await cubeService.search(search, from, to);

    res.render('index', { 
        title: 'SEARCH',
        search,
        from,
        to,
        cubes,
    });
};

// Router handle requests
router.get('/', home); // handle requests '/' and execute function home
router.get('/about', about);
router.get('/search', search);

module.exports = router;
