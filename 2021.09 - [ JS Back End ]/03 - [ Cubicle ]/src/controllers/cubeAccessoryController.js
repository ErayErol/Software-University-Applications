// 1. Controller
// 1.1 is responsible to receive user input and decide what to do
// 1.2 determines what response to send back to a user when a user makes a browser request.

//**************************** Import Modules **********************
// You must pass {mergeParams: true} to the child router if you want to access the params from the parent router.
const router = require('express').Router({ mergeParams: true });

//**************************** Import Services **********************
const cubeService = require('../services/cubeService');
const accessoryService = require('../services/accessoryService');

//**************************** Functions **********************
router.get('/add', async (req, res) => {
    let cube = await cubeService.getOne(req.params.cubeId);
    let accessories = await accessoryService.getAllWithout(cube.accessories.map(x => x._id));

    res.render('cube/accessory/add', { cube, accessories });
}); 

router.post('/add', async (req, res) => {
    const cubeId = req.params.cubeId;
    
    await cubeService.attachAccessory(cubeId, req.body.accessory);

    res.redirect(`/cube/${cubeId}`);
});

module.exports = router;