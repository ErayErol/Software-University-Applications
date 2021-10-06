// 1. Services
// 1.1 Communication between a controller and data layer

//**************************** Import Models **********************
const Cube = require('../models/Cube');
const Accessory = require('../models/Accessory');

//**************************** Business Logic **********************
//**************************** Functions ***************************
const getAll = () =>
    Cube.find({}).lean();

const getOne = (id) =>
    Cube.findById(id).populate('accessories').lean();

const create = (name, description, imageUrl, difficulty) => {
    let cube = new Cube({
        name,
        description,
        imageUrl,
        difficulty,
    });

    return cube.save();
};

const search = async (text, from, to) => {
    let result = await getAll();

    if (text) {
        result = result.filter(x => x.name.toLowerCase().includes(text.toLowerCase()))
    }

    if (from) {
        result = result.filter(x => x.difficulty >= from);
    }

    if (to) {
        result = result.filter(x => x.difficulty <= to);
    }

    return result;
};

const attachAccessory = async (cubeId, accessoryId) => {
    let cube = await Cube.findById(cubeId);
    let accessory = await Accessory.findById(accessoryId);

    cube.accessories.push(accessory);

    return cube.save();
};

// All functions
const cubeService = {
    getAll,
    create,
    getOne,
    search,
    attachAccessory,
};

module.exports = cubeService;