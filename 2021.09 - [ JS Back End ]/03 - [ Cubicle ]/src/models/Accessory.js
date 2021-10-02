//**************************** Import Modules **********************
const mongoose = require('mongoose');

// Everything in Mongoose starts with a Schema. 
// Each schema maps to a MongoDB collection and defines the shape of the documents within that collection.
const accessorySchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
    },
    imageUrl: {
        type: String,
        required: [true, 'Image Url is required!'],
        validate: [/^https?:\/\//i, 'Image url is invalid']
    },
    description: {
        type: String,
        required: true,
        maxlength: 500,
    }
});

// To use our schema definition, we need to convert our cubeSchema into a Model we can work with.
const Accessory = mongoose.model('Accessory', accessorySchema);

module.exports = Accessory;
