/// SignalR connection
const connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

/// Handle Connection start
connection.start().catch(function (err) {
  return console.error(err.toString());
});

/// Handle incoming message
connection.on("Message", function (message) {
  $("#message").text(message);
});

/// Handle game state updates (draw map, update player list)
connection.on("GameState", function (gameState) {
  handleGameState(JSON.parse(gameState))
});