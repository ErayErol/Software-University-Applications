//**************************** Import Modules **********************
const mongoose = require('mongoose');

// Everything in Mongoose starts with a Schema. 
// Each schema maps to a MongoDB collection and defines the shape of the documents within that collection.
const cubeSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
    },
    description: {
        type: String,
        required: true,
        maxlength: 100,
    },
    imageUrl: {
        type: String,
        required: true,
        validate: [/^https?:\/\//i, 'invalid image url']
        // validate: {
        //     validator: function(value) {
        //         return /^https?:\/\//i.test(value)
        //     },
        //     message: (props) => `Image Url ${props.value} is invalid!`
        // }
    },
    difficulty: {
        type: Number,
        required: true,
        min: 1,
        max: 5,
    },
    accessories: [
        {
            type: mongoose.Types.ObjectId,
            ref: 'Accessory',
        }
    ]
});

// Defining our custom instance methods. Not used, only for demo.
cubeSchema.statics.findByName = function(name) {
    return this.find({name});
};

// To use our schema definition, we need to convert our cubeSchema into a Model we can work with.
const Cube = mongoose.model('Cube', cubeSchema);

module.exports = Cube;
