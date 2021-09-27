const express = require('express');
const app = express.Router(); // Modular Routers
const path = require('path');

//**************************** Import "Database" **********************
const breeds = require('../data/breeds.json');
const cats = require('../data/cats.json');


//**************************** Route Methods **********************
app.get('/add-breed', (req, res) => {
    res.render('addBreed', { layout: false });
});

app.post('/add-breed', (req, res) => {
    breeds.push(req.body.breed);
    const filePath = path.join(__dirname, '../data/breeds.json');
    const result = JSON.stringify(breeds);
    res.sendFile(filePath, result, (err) => {
        if (err) {
            next(err);
        } else {
            console.log('The breed was added successfully.');
        }
    });
    res.redirect(302, '/');
});

app.get('/add-cat', (req, res) => {
    res.render('addCat', {
        layout: false,
        breeds,
    });
});

app.post('/add-cat', (req, res) => {
    let image = '';
    if (req.files) {
        const file = req.files.file;
        image = file.name;
        const filePath = path.join(__dirname, "../public/images/" + file.name,);

        file.mv(filePath, (err) => {
            if (err) {
                res.send(err);
            } else {
                console.log('Files was uploaded successfully.');
            }
        });
    } else {
        image = null;
    }

    let i = 1;
    while (true) {
        const id = cats.length + i;
        const isIdFree = cats.findIndex(cat => cat.id === id);

        if (isIdFree > -1) {
            i++;
        } else {
            cats.push({ id: id, ...req.body, image });
            break;
        }
    }

    const result = JSON.stringify(cats, '', 2);
    const resultPath = path.join(__dirname, '../data/cats.json');
    res.sendFile(resultPath, result, (err) => {
        if (err) {
            next(err);
        } else {
            console.log('The cat was added successfully.');
        }
    });
    res.redirect(302, '/');
});

app.get('/edit/:id', (req, res) => {
    const catId = +req.params.id;
    const cat = cats.find(c => c.id === catId);

    res.render('editCat', {
        layout: false,
        cat,
        breeds,
    });
});

app.post('/edit/:id', (req, res) => {
    const catId = +req.params.id;
    const catIndex = cats.findIndex((cat => cat.id == catId));
    cats[catIndex].name = req.body.name;
    cats[catIndex].description = req.body.description;
    cats[catIndex].breed = req.body.breed;

    if (req.files) {
        const file = req.files.upload;
        const imagePath = path.join(__dirname, "../public/images/" + file.name,);

        file.mv(imagePath, (err) => {
            if (err) {
                res.send(err);
            } else {
                console.log('Files was uploaded successfully.');
            }
        });

        cats[catIndex].image = file.name;
    }

    const result = JSON.stringify(cats, '', 2);
    const filePath = path.join(__dirname, '../data/cats.json');
    res.sendFile(filePath, result, (err) => {
        if (err) {
            next(err);
        } else {
            console.log('The cat was edited successfully.');
        }
    });
    res.redirect(302, '/');
});

app.get('/find-new-home/:id', (req, res) => {
    const catId = +req.params.id;
    const cat = cats.find(c => c.id === catId);

    res.render('catShelter', {
        layout: false,
        cat,
    });
});

app.post('/find-new-home/:id', (req, res) => {
    const catId = +req.params.id;
    const catIndex = cats.findIndex((cat => cat.id == catId));

    if (catIndex > -1) {
        cats.splice(catIndex, 1);
        const result = JSON.stringify(cats, '', 2);
        const filePath = path.join(__dirname, '../data/cats.json');
        res.sendFile(filePath, result, (err) => {
            if (err) {
                next(err);
            } else {
                console.log('The cat was deleted successfully.');
            }
        });
    }

    res.redirect(302, '/');
});

module.exports = app;