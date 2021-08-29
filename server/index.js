const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

var playersInfo = {};

app.use(express.static(__dirname + '/game/'));

io.on('connection', (socket) => {
  
	socket.on('PlayerInfo', (info) => {
		console.log("Player info - Name: " + info.name + ", Color: " + info.color);
		info.id = socket.id;
		playersInfo[socket.id] = info;
		io.emit('PlayerStatus', info);
	});
	
	socket.on('PlayerStatus', (state) => {
		state.id = socket.id;
		io.emit('PlayerStatus', state);
	});
	
	socket.on('PlayerInfoRequest', (playerId) => {
		var info = playersInfo[playerId];
		if(info != undefined){
			socket.to(socket.id).emit('PlayerInfo', info.id, info);
		}
	});
	
});

server.listen(3000, () => {
	console.log('listening on *:3000');
});