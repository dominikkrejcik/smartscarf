var express = require('express');
var app = express();

app.use(express.static(__dirname + '/public'));

var port = Number(process.env.PORT | 5001);
app.listen(port, function() {
	console.log("Listening on port " + port);
});
