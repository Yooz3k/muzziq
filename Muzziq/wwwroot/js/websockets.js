//Only for websocket communication

var connectionUrl = document.getElementById("connectionUrl");
var connectButton = document.getElementById("connectButton");
var stateLabel = document.getElementById("stateLabel");
var sendMessage = document.getElementById("sendMessage");
var sendButton = document.getElementById("sendButton");
var commsLog = document.getElementById("commsLog");
var closeButton = document.getElementById("closeButton");
var socket;
var SEPARATOR = " ";
var readyToReciveAudio = false;

var scheme = document.location.protocol === "https:" ? "wss" : "ws";
var port = document.location.port ? (":" + document.location.port) : "";

connectionUrl.value = scheme + "://" + document.location.hostname + port;

function updateState() {
    function disable() {
        sendMessage.disabled = true;
        sendButton.disabled = true;
        closeButton.disabled = true;
    }
    function enable() {
        sendMessage.disabled = false;
        sendButton.disabled = false;
        closeButton.disabled = false;
    }

    connectionUrl.disabled = true;
    connectButton.disabled = true;

    if (!socket) {
        disable();
    } else {
        switch (socket.readyState) {
            case WebSocket.CLOSED:
                stateLabel.innerHTML = "Closed";
                disable();
                connectionUrl.disabled = false;
                connectButton.disabled = false;
                break;
            case WebSocket.CLOSING:
                stateLabel.innerHTML = "Closing...";
                disable();
                break;
            case WebSocket.CONNECTING:
                stateLabel.innerHTML = "Connecting...";
                disable();
                break;
            case WebSocket.OPEN:
                stateLabel.innerHTML = "Open";
                enable();
                break;
            default:
                stateLabel.innerHTML = "Unknown WebSocket State: " + htmlEscape(socket.readyState);
                disable();
                break;
        }
    }
}

closeButton.onclick = function () {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        alert("socket not connected");
    }
    socket.close(1000, "Closing from client");
    console.log("Zamknieto polaczenie")
};

sendButton.onclick = function () {
    send(WSMessageType.TEXT, sendMessage.value)
};

connectButton.onclick = function () {
    stateLabel.innerHTML = "Connecting";
    connect(connectionUrl.value);
};

function htmlEscape(str) {
    return str.toString()
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}


// Normal functions (for future, not for example)

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

function connect(url) {
    socket = new WebSocket(connectionUrl.value);
    socket.onopen = function (event) {
        console.log("Udalo sie polaczyc");
        updateState(); //OnlyForExample - ToRemove
    };
    socket.onclose = function (event) {
        console.log("Polaczenie zamkniete");
        updateState(); //OnlyForExample - ToRemove
    };
    socket.onerror = function (event) {
        console.log("Wystapil blad");
        updateState(); //OnlyForExample - ToRemove
    };
    socket.onmessage = function (event) {
        console.log("Przyszla wiadomosc: " + event.data)
        updateState(); //OnlyForExample - ToRemove
        dispatch(event.data);
    };
}

function disconnect() {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        alert("socket not connected");
    }
    socket.close(1000, "Closing from client");
    console.log("Zamknieto polaczenie")
}

// messageType: WSMessageType, text: string
function send(messageType, text) {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        alert("socket not connected");
    }
    var data = messageType + SEPARATOR + text;
    socket.send(data);
    console.log("Wyslano wiadomosc: " + data)
}

function dispatch(message) {

    if (message instanceof Blob && readyToReciveAudio) {
        audio = event.data;
        playMusic(audio);
    } else {
        var type = message.split(SEPARATOR)[0];
        console.log(type);
        var text = message.substr(message.indexOf(SEPARATOR)).trim();
        console.log(text);

        switch (type) {
            case WSMessageType.AUDIO_START:
                readyToReciveAudio = true;
                break;
            case WSMessageType.AUDIO_END:
                readyToReciveAudio = false;
                break;
            case WSMessageType.SCORE:
                console.log(2);
                break;
            case WSMessageType.TEXT:
                console.log(3);
                break;
            case WSMessageType.OTHER:
                console.log(4);
                break;
            default:
                console.log(5);
                break;
        }
    }



    
}

// functions for different message types - should be in site.js:

function playMusic(audio) {
    var ctx = new AudioContext();
    var source = ctx.createBufferSource();

    var fileReader = new FileReader();
    fileReader.onload = function (event) {
        var arrayBuffer = event.target.result;
        ctx.decodeAudioData(arrayBuffer, function (buffer) {
            source.buffer = buffer;
            source.connect(ctx.destination);
            source.start();
        })
    };
    fileReader.readAsArrayBuffer(audio);

    console.log("Odtwarzanie muzyki " + audio)
}