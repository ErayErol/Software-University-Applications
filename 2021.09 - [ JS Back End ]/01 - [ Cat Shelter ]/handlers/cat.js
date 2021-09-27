const url = require('url');
const fs = require('fs');
const path = require('path');
const qs = require('querystring');
const formidable = require('formidable');
const cats = require('../data/cats');
const breeds = require('../data/breeds');

module.exports = (req, res) => {
    const pathname = url.parse(req.url).pathname;

    if (pathname === '/cats/add-cat' && req.method === 'GET') {
        const filePath = path.join(__dirname, '../views/addCat.html');
        const readStream = fs.createReadStream(filePath);

        readStream.on("data", (data) => {
            let catBreedPlaceholder = breeds.map(breed => `<option value="${breed}">${breed}</option>`);
            let modifiedData = data.toString().replace('{{catBreeds}}', catBreedPlaceholder);
            res.write(modifiedData);
        }).on("end", () => {
            res.end();
        }).on("error", (err) => {
            res.write(err);
        });
    } else if (pathname === '/cats/add-cat' && req.method === 'POST') {
        let form = formidable.IncomingForm();
        form.uploadDir = path.join(__dirname, '../content/images/');

        form.parse(req, (err, fields, files) => {
            if (err) {
                throw err;
            }

            let oldPath = files.upload.path;
            let newPath = path.join(__dirname, '../content/images/' + files.upload.name);
            fs.rename(oldPath, newPath, (err) => {
                if (err) {
                    throw err;
                } else {
                    console.log('Files was uploaded successfully.');
                }
            });

            let i = 1;
            while (true) {
                const id = cats.length + i;
                let isIdFree = cats.findIndex(cat => cat.id === id);

                if (isIdFree > -1) {
                    i++;
                } else {
                    cats.push({ id: id, ...fields, image: files.upload.name });
                    break;
                }
            }

            let result = JSON.stringify(cats, '', 2);
            let filePath = path.join(__dirname, '../data/cats.json');
            fs.writeFile(filePath, result, 'utf8', () => console.log('The cat was added successfully.'));
            res.writeHead(302, { location: '/' }).end();
        });
    } else if (pathname === '/cats/add-breed' && req.method === 'GET') {
        const filePath = path.join(__dirname, '../views/addBreed.html');
        const readStream = fs.createReadStream(filePath);
        readStream.pipe(res);
    } else if (pathname === '/cats/add-breed' && req.method === 'POST') {
        let formData = '';

        req.on('data', (data) => {
            formData += data;
            // Too much POST data, kill the connection!
            // 1e6 === 1 * Math.pow(10, 6) === 1 * 1000000 ~~~ 1MB
            if (formData.length > 1e6) {
                req.connection.destroy();
            }
        });

        req.on('end', () => {
            const filePath = path.join(__dirname, '../data/breeds.json');
            const readStream = fs.createReadStream(filePath);

            readStream.on("data", (data) => {
                let breeds = JSON.parse(data);
                let body = qs.parse(formData);
                breeds.push(body.breed);
                let result = JSON.stringify(breeds);
                fs.writeFile(filePath, result, 'utf8', () => console.log('The breed was added successfully.'));
                res.writeHead(302, { location: '/' });
            }).on("end", () => {
                res.end();
            }).on("error", (err) => {
                res.write(err);
            });
        });
    } else if (pathname.includes('/cats-edit') && req.method === 'GET') {
        const filePath = path.join(__dirname, '../views/editCat.html');
        const readStream = fs.createReadStream(filePath);

        readStream.on("data", (data) => {
            let catId = +pathname.substring(pathname.lastIndexOf('/') + 1);
            let currentCat = cats.find(c => c.id === catId);
            let modifiedData = data.toString().replace('{{id}}', catId);
            modifiedData = modifiedData.replace('{{name}}', currentCat.name);
            modifiedData = modifiedData.replace('{{description}}', currentCat.description);
            let breedsAsOption = breeds.map(breed => `<option value="${breed}">${breed}</option>`);
            modifiedData = modifiedData.replace('{{catBreeds}}', breedsAsOption.join('/'));
            modifiedData = modifiedData.replace('{{breed}}', currentCat.breed);
            res.write(modifiedData);
        }).on("end", () => {
            res.end();
        }).on("error", (err) => {
            res.write(err);
        });
    } else if (pathname.includes('/cats-edit') && req.method === 'POST') {
        let form = formidable.IncomingForm();

        form.parse(req, (err, fields, files) => {
            if (err) {
                throw err;
            }

            let catId = +pathname.substring(pathname.lastIndexOf('/') + 1);
            let catIndex = cats.findIndex((cat => cat.id == catId));
            cats[catIndex].name = fields.name;
            cats[catIndex].description = fields.description;
            cats[catIndex].breed = fields.breed;

            if (files.upload.name) {
                let oldPath = files.upload.path.toString();
                let newPath = path.join(__dirname, '../content/images/' + files.upload.name);

                fs.copyFile(oldPath, newPath, (err) => {
                    if (err) {
                        throw err;
                    } else {
                        console.log('Files was uploaded successfully.');
                        cats[catIndex].image = files.upload.name;
                    }
                });
            }

            let result = JSON.stringify(cats, '', 2);
            let filePath = path.join(__dirname, '../data/cats.json');
            fs.writeFile(filePath, result, 'utf8', () => console.log('The cat was edited successfully.'));
            res.writeHead(302, { location: '/' }).end();
        });
    } else if (pathname.includes('/cats-find-new-home') && req.method === 'GET') {
        const filePath = path.join(__dirname, '../views/catShelter.html');
        const readStream = fs.createReadStream(filePath);

        readStream.on("data", (data) => {
            let catId = +pathname.substring(pathname.lastIndexOf('/') + 1);
            let currentCat = cats.find(c => c.id === catId);
            let modifiedData = data.toString().replace('{{id}}', catId);
            modifiedData = modifiedData.replace('{{name}}', currentCat.name);
            modifiedData = modifiedData.replace('{{description}}', currentCat.description);
            modifiedData = modifiedData.replace('{{image}}', currentCat.image);
            let breed = `<option value="${currentCat.breed}">${currentCat.breed}</option>`;
            modifiedData = modifiedData.replace('{{breed}}', breed);
            res.write(modifiedData);
        }).on("end", () => {
            res.end();
        }).on("error", (err) => {
            res.write(err);
        });
    } else if (pathname.includes('/cats-find-new-home') && req.method === 'POST') {
        let catId = +pathname.substring(pathname.lastIndexOf('/') + 1);
        let catIndex = cats.findIndex((cat => cat.id == catId));

        if (catIndex > -1) {
            cats.splice(catIndex, 1);
            let result = JSON.stringify(cats, '', 2);
            let filePath = path.join(__dirname, '../data/cats.json');
            fs.writeFile(filePath, result, 'utf8', () => console.log('The cat was deleted successfully.'));
        }

        res.writeHead(302, { location: '/' }).end();
    } else {
        return true;
    }
};