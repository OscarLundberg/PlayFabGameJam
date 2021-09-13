function read(key) {
    // return JSON.parse(server.GetTitleData()[key] || "[]");

    var dataRequest = { "Keys": [key] };
    var response = server.GetTitleData(dataRequest);
    if (!response.Data.hasOwnProperty(key)) {
        log.error("no data found");
        return [];
    }
    else {
        return JSON.parse(response.Data[key]);
    }
}

function write(key, val) {
    let request = { "Key": key, "Value": JSON.stringify(val) }
    server.SetTitleData(request);
}

function assignRoles(lobby) {
    let x = shuffle(lobby.users);
    let dishonorableCount = 0;
    if (x.length >= 9) {
        dishonorableCount = 3;
    }
    else if (x.length >= 5) {
        dishonorableCount = 2;
    }
    else {
        dishonorableCount = 1;
    }
    return x.map((nm, ind) => ind < dishonorableCount ? { name: nm, role: "dishonorable" } : { name: nm, role: "honorable" })
}

function shuffle(a) {
    for (let i = a.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [a[i], a[j]] = [a[j], a[i]];
    }
    return a;
}


const mode = (arr) => [...new Set(arr)]
    .map((value) => [value, arr.filter((v) => v === value).length])
    .sort((a, b) => a[1] - b[1])
    .reverse()
    .filter((value, i, a) => a.indexOf(value) === i)
    .filter((v, i, a) => v[1] === a[0][1])
    .map((v) => v[0])

// state {
//     players: [{name:nm, role:role}],
//     stages: [{
//          convicted:[""],
//          robbed: [],
//     }],
//     stage: -1
// }



handlers.start_game = function (args, context) {
    const lobby = read("lobbies")[args.Lobby];
    const roles = assignRoles(lobby);
    const state = {
        players: roles,
        stages: [],
        stage: 0
    }
    write(args.Lobby + "_state", state);
}


handlers.lobby_poll = function (args, context) {
    write(args.user + "-activity", { timestamp: Date.now() });
    return read(args.Lobby);
}

handlers.progress_game = function (args, context) {
    let state = read(args.Lobby + "_state");
    let currentStage = {};
    let playerActions = {};
    for (let player of state.players) {
        let actions = read(`${args.Lobby}-${player.name}-${args.stage}`);
        if (actions.hasOwnProperty(target)) {
            if (playerActions.hasOwnProperty(actions.reason)) {
                playerActions[actions.reason].push(actions.target);
            } else {
                playerActions[actions.reason] = [actions.target]
            }
        }
    }

    if (playerActions.hasOwnProperty("rob")) {
        currentStage.robbed = playerActions.rob;
    }

    if (playerActions.hasOwnProperty("vote")) {
        const mode = mode(playerActions["vote"]);
        if (mode.length <= 1) {
            currentStage.convicted = mode[0];
        }
    }

    state.stages[args.stage] = currentStage;
    write(args.Lobby + "_state", state);
    return { success: true };
}

handlers.target = function (args, context) {
    let content = { target: args.target, reason: args.reason };
    write(`${args.Lobby}-${args.user}-${args.stage}`, content)
}

handlers.list_lobby_events = function (args, context) {
    write(args.user + "-activity", { timestamp: Date.now() });
    var lobby_events = read(args.Lobby);
    return { events: lobby_events };
};

handlers.send_lobby_event = function (args, context) {
    var lobby_events = read(args.LobbyID);
    lobby_events.push(args);
    write(args.LobbyID, lobby_events);
    return { events: lobby_events };
}

handlers.create_lobby = function (args, context) {
    let lobbies = read("lobbies");
    lobbies.push(args.Payload);
    write("lobbies", lobbies);
    return { success: true };
}

function prune(lobbies) {
    let activeLobbies = [];
    for (let lobby of lobbies) {
        let activeUsers = [];
        for (let user of lobby.users) {
            const activity = read(user + "-activity");
            if (Date.now() - activity.timestamp > 15000) {

            } else {
                activeUsers.push(user);
            }
        }
        if (activeUsers.length > 0) {
            let updatedLobby = lobby;
            updatedLobby.users = activeUsers;
            activeLobbies.push(updatedLobby);
        } else {

        }
    }
    return activeLobbies;
}

handlers.get_lobbies = function (args, context) {
    const data = read("lobbies")

    const res = { "lobbies": prune(data) }
    return res;
}

handlers.join_lobby = function (args, context) {
    let lobbies = read("lobbies");
    for (let i = 0; i < lobbies.length; i++) {
        if (lobbies[i].id === args.Lobby) {
            var lobbyData = lobbies[i];
            if (lobbyData.isJoinable) {
                lobbyData.users.push(args.user)
                lobbies[i] = lobbyData;
                write("lobbies", lobbies);
                return lobbyData;
            }
        }
    }
    return { success: false };
}

handlers.leave_lobby = function (args, context) {
    let lobbies = read("lobbies");
    for (let i = 0; i < lobbies.length; i++) {
        if (lobbies[i].id === args.Lobby) {
            var lobbyData = lobbies[i];
            lobbyData.users.remove(args.user);
            lobbies[i] = lobbyData;
            write("lobbies", lobbies);
            return lobbyData;
        }
    }
    return { success: false };
}
