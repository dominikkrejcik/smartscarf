class ConnectionPool
  constructor: ->
    @_connections = []

  addConnection: (conn) ->
    @_pipeConnections(conn, c) for c in @_connections
    conn.setNoDelay()
    conn.bufferSize = 6144
    @_connections.push(conn)

  removeConnection: (conn) ->
    @_connections.splice(@_connections.indexOf(conn), 1)
    conn.unpipe()
    conn.removeAllListeners()

  connectionCount: ->
    @_connections.length

  _pipeConnections: (connA, connB) ->
    connA.read()
    connB.read()
    connA.pipe(connB)
    connB.pipe(connA)

module.exports = ConnectionPool
