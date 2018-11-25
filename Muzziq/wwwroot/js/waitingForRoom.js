var WSMessageType = {
    TEXT: "TEXT",
    SCORE: "SCORE",
    AUDIO_START: "AUDIO_START",
    AUDIO_END: "AUDIO_END",
    PLAYER_LEAVE: "PLAYER_LEAVE",
    PLAYER_JOIN: "PLAYER_JOIN",
    ROOM_SUBSCRIBE: "ROOM_SUBSCRIBE",
    OTHER: "OTHER",
}

document.body.onload = function () {
    connectToRoom(scheme + "://" + document.location.hostname + port);
    document.getElementById("leaveLobbyButton").onclick = function () {
        send(WSMessageType.PLAYER_LEAVE, playerID.value + " " + roomID.value);
    }
};

function connectToRoom(url) {
    socket = new WebSocket(url);
    socket.onopen = function (event) {
        console.log("Udalo sie polaczyc");
        alert(document.getElementById("playerID").value);
        send(WSMessageType.ROOM_SUBSCRIBE, playerID.value + " " + roomID.value);
    };
    socket.onclose = function (event) {
        console.log("Polaczenie zamkniete");
    };
    socket.onerror = function (event) {
        console.log("Wystapil blad");
    };
    socket.onmessage = function (event) {
        console.log("Przyszla wiadomosc: " + event.data)
        updatePlayersList(event.data);
    };
}

function updatePlayersList(message) {
    var type = message.split(SEPARATOR)[0];
    console.log(type);
    var text = message.substr(message.indexOf(SEPARATOR)).trim();
    console.log(text);

    if (!socket) {
        document.location.reload()
    } else {
        switch (type) {
            case "PLAYER_JOIN":
                addPlayer(text)
                break;
            case "PLAYER_LEAVE":
                deletePlayer(text)
                break;
            default:
                alert("dupsko")
                
                break;
        }

    }
}

function addPlayer(name) {
    alert('add_player' + name)
}
function deletePlayer(name) {
    alert('delete_player' + name)
}
