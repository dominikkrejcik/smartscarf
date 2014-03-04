class ConnectionPool
  constructor: ->
    @_connections = []

  addConnection: (conn) ->
    @_pipeConnections(conn, c) for c in @_connections
    @_connections.push(conn)

  removeConnection: (conn) ->
    #@_connections.splice(@_connections.indexOf(conn), 1)
    #conn.removeAllListeners()

  _pipeConnections: (connA, connB) ->
    connA.pipe(connB)
    connB.pipe(connA)

module.exports = ConnectionPool
