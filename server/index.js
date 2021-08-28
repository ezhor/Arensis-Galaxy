const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

app.use(express.static(__dirname + '/game/'));

io.on('connection', (socket) => {
  socket.on('state', (state) => {
	state.id = socket.id;
    io.emit('state', state);
  });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});