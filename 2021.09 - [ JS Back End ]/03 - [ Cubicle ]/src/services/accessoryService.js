// 1. Services
// 1.1 Communication between a controller and data layer

//**************************** Import Models **********************
const Accessory = require('../models/Accessory');

//**************************** Business Logic **********************
//**************************** Functions ***************************
async function create(name, description, imageUrl) {
    return Accessory.create({name, description, imageUrl});
}

async function getAll() {
    return Accessory.find({}).lean();
}

async function getAllWithout(accessoryIds) {
    // return Accessory.find({_id: {$nin: accessoryIds }}).lean();
    return Accessory.find().where('_id').nin(accessoryIds).lean();
}

// All functions
const accessoryService = {
    getAll,
    create,
    getAllWithout,
};

module.exports = accessoryService;