const express = require('express');
const app = express.Router();
const cats = require('../data/cats.json');

app.get('/', (req, res) => {
    res.render('home', {
        layout: false,
        cats
    });
});

app.post('/search', (req, res) => {
    let catFilter = cats.filter(c => c.name.includes(req.body.search));

    res.render('home', {
        layout: false,
        cats: catFilter,
    });
});

module.exports = app;