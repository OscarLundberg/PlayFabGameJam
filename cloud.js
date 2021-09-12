const read = function (key) {
    return JSON.parse(server.GetTitleData()[key] || "[]");
}

const write = function (key, val) {
    let request = { key, value: JSON.stringify(val) }
    server.SetTitleData(request);
}


handlers.list_lobby_events = function (args, context) {

    var lobby_events = read(args.Lobby);
    return { events: lobby_events };
};

handlers.send_lobby_event = function (args, context) {

    var lobby_events = read(args.Lobby);
    lobby_events.push(args.Payload);
    write(args.Lobby, lobby_events);
    return { events: lobby_events };
}

handlers.create_lobby = function (args, context) {
    let lobbies = read("lobbies");
    lobbies.push(args.Payload);
    write(lobbies);
    return { success: true };
}

handlers.get_lobbies = function (args, context) {
    return { lobbies: read("lobbies") };
}

handlers.join_lobby = function (args, context) {
    let lobbies = read("lobbies");
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
    return { success: false };
}
