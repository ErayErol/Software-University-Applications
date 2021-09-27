const port = 3000;
const http = require('http');
const handlers = require('./handlers');

let app = http.createServer((req, res) => {
    
    for (let handler of handlers) {
        if (!handler(req, res)) {
            break;
        }
    }
});

app.listen(port);

console.log(`Node.js server running on port ${port}`);