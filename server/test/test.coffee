net = require 'net'
stream = require 'stream'
assert = require 'assert'
ConnectionPool = require '../connectionPool'

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
