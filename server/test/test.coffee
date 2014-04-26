net = require 'net'
stream = require 'stream'
assert = require 'assert'
ConnectionPool = require '../connectionPool'
server = require '../server'

describe 'ConnectionPool', ->
  connectionPool = new ConnectionPool
  connection = new net.Socket({readeable: true, writeable: true})
  describe 'addConnection', ->
    it 'should add a connection to the pool', ->
      connectionPool.addConnection(connection)
      assert.equal connectionPool.connectionCount(), 1

  describe 'removeConnection', ->
    it 'should remove a connection from the pool', ->
      connectionPool.removeConnection(connection)
      assert.equal connectionPool.connectionCount(), 0

describe 'Server', ->
  it 'should allow clients to connect', (done) ->
    server.listen 8214, ->
      clientA = net.connect port: 8214, ->
        clientB = net.connect port: 8214, ->
          clientB.on 'data', (data) ->
            assert.equal data, 'Hello world'
            done()
          clientA.write 'Hello world'

