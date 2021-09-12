handlers.list_lobby_events = function (args, context) {

    var lobby_events = server.GetTitleData([args.Lobby]) || [];

    return { events: lobby_events };
};

handlers.send_lobby_event = function (args, context) {

    var lobby_events = server.GetTitleData([args.Lobby]) || [];
    lobby_events.push(args.Payload);
    server.SetTitleData(args.Lobby, lobby_events);
    return { events: lobby_events };
}

handlers.create_lobby = function (args, context) {
    let lobbies = server.GetTitleData(["lobbies"]) || [];
    lobbies.push(args.Payload);
    server.SetTitleData("lobbies", lobbies);
    this.join_lobby()
    return true;
}

handlers.get_lobbies = function (args, context) {
    return { lobbies: server.GetTitleData(["lobbies"]) };
}

handlers.join_lobby = function (args, context) {
    let lobbies = server.GetTitleData(["lobbies"]) || [];
    for (let i = 0; i < lobbies.length; i++) {
        if (lobbies[i].id === args.Lobby) {
            var lobbyData = lobbies[i];
            if (lobbyData.isJoinable) {
                lobbyData.users.push(args.user)
                lobbies[i] = lobbyData;
                return lobbyData;
            }
        }
    }
    return false;
}
