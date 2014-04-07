net = require 'net'

ConnectionPool = require './connectionPool'

connectionPool = new ConnectionPool

server = net.createServer (c) ->
  console.log "Connected"
  connectionPool.addConnection(c)
  c.on "end", ->
    connectionPool.removeConnection(c)
    console.log "Disconnected"

server.listen 8214, ->
  console.log "Server started"
