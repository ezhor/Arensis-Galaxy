const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

const port = 80;

var playersInfo = {};

app.use(express.static(__dirname + '/game/'));

io.on('connection', (socket) => {
	console.log("User connected: " + socket.id);
  
	socket.on('PlayerInfo', (info) => {
		console.log("Player info:" + info);
		info = JSON.parse(info);
		info.ID = socket.id;
		playersInfo[socket.id] = info;
		io.emit('PlayerInfo', JSON.stringify(info));
	});
	
	socket.on('PlayerStatus', (state) => {
		state = JSON.parse(state);
		state.ID = socket.id;
		io.emit('PlayerStatus', JSON.stringify(state));
	});
	
	socket.on('PlayerInfoRequest', (playerId) => {
		console.log("Requested: " + playerId);
		var info = playersInfo[playerId];
		if(info != undefined){
			socket.emit('PlayerInfo', JSON.stringify(info));
		}
	});
	
	socket.on('disconnect', () => {
		console.log("User disconnected: " + socket.id);
		delete playersInfo[socket.id];
		io.emit('PlayerDisconnected', socket.id);
	});
	
});

server.listen(port, () => {
	console.log("Listening on port " + port);
});