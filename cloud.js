handlers.list_lobby_events = function (args, context) {

    var lobby_events = server.GetTitleData([args.Lobby]) || [];

    return { events: lobby_events };
};

handlers.send_lobby_event = function (args, context) {

    var lobby_events = server.GetTitleData([args.Lobby]) || [];
    lobby_events.push(args.event);
    server.SetTitleData(args.Lobby, lobby_events);
    return { events: lobby_events };
}
