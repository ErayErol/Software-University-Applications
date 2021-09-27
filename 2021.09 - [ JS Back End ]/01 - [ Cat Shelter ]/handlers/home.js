const url = require('url');
const fs = require('fs');
const path = require('path');
const qs = require('querystring');
const cats = require('../data/cats');

module.exports = (req, res) => {
    const pathname = url.parse(req.url).pathname;

    if (pathname === '/' && req.method === 'GET') {
        let filePath = path.join(__dirname, '../views/home/index.html');
        const readStream = fs.createReadStream(filePath);

        readStream.on("data", (data) => {
            let modifiedCats = cats.map(cat => `
                <li>
                    <img src="/content/images/${cat.image}" alt="Cat name : ${cat.name} Cat breed : ${cat.breed}">
                    <h3>${cat.name}</h3>
                    <p><span>Breed: </span>${cat.breed}</p>
                    <p><span>Description: </span>${cat.description}</p>
                    <ul class="buttons">
                        <li class="btn edit"><a href="/cats-edit/${cat.id}">Change Info</a></li>
                        <li class="btn delete"><a href="/cats-find-new-home/${cat.id}">New Home</a></li>
                    </ul>
                </li>
            `);

            let modifiedData = data.toString().replace('{{cats}}', modifiedCats);
            res.write(modifiedData);
        }).on("end", () => {
            res.end();
        }).on("error", (err) => {
            res.write(err);
        });
    } else if (pathname === '/search' && req.method === 'POST') {
        let formData = '';

        req.on('data', (data) => {
            formData += data;
        });

        req.on('end', () => {
            let filePath = path.join(__dirname, '../views/home/index.html');
            const readStream = fs.createReadStream(filePath);

            readStream.on("data", (data) => {
                let body = qs.parse(formData);
                let catFilter = cats.filter(c => c.name.includes(body.search));
                let modifiedCats = catFilter.map(cat => `
                    <li>
                        <img src="/content/images/${cat.image}" alt="Cat name : ${cat.name} Cat breed : ${cat.breed}">
                        <h3>${cat.name}</h3>
                        <p><span>Breed: </span>${cat.breed}</p>
                        <p><span>Description: </span>${cat.description}</p>
                        <ul class="buttons">
                            <li class="btn edit"><a href="/cats-edit/${cat.id}">Change Info</a></li>
                            <li class="btn delete"><a href="/cats-find-new-home/${cat.id}">New Home</a></li>
                        </ul>
                    </li>
                `);

                let modifiedData = data.toString().replace('{{cats}}', modifiedCats);
                res.write(modifiedData);
            }).on("end", () => {
                res.end();
            }).on("error", (err) => {
                res.write(err);
            });
        });
    } else {
        return true;
    }
};