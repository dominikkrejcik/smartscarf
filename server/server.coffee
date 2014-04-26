net = require 'net'

ConnectionPool = require './connectionPool'

connectionPool = new ConnectionPool

server = net.createServer (c) ->
  connectionPool.addConnection(c)
  c.on "data", (data) ->
    if connectionPool.connectionCount() == 1
      c.write("No one's listening to you bro!")
      c.read()
  c.on "end", ->
    connectionPool.removeConnection(c)

module.exports = server
