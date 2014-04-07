net = require 'net'

ConnectionPool = require './connectionPool'

connectionPool = new ConnectionPool

server = net.createServer (c) ->
  console.log "Connected"
  connectionPool.addConnection(c)
  c.on "data", (data) ->
    if connectionPool.connectionCount() == 1
      c.write("No one's listening to you bro!")
      c.read()
  c.on "end", ->
    connectionPool.removeConnection(c)
    console.log "Disconnected"

server.listen 8214, ->
  console.log "Server started"
