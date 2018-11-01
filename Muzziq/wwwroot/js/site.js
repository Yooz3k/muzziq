var connectionUrl = document.getElementById("connectionUrl");
var connectButton = document.getElementById("connectButton");
var stateLabel = document.getElementById("stateLabel");
var sendMessage = document.getElementById("sendMessage");
var sendButton = document.getElementById("sendButton");
var commsLog = document.getElementById("commsLog");
var closeButton = document.getElementById("closeButton");
var socket;
var SEPARATOR = " ";

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
};

sendButton.onclick = function () {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        alert("socket not connected");
    }
    //Add messageType and Separator to sendData
    // var data = WSMessageType.TEXT + SEPARATOR + sendMessage.value;
    var data = sendMessage.value;
    socket.send(data);
    commsLog.innerHTML += '<tr>' +
        '<td class="commslog-client">Client</td>' +
        '<td class="commslog-server">Server</td>' +
        '<td class="commslog-data">' + htmlEscape(data) + '</td></tr>';
};

connectButton.onclick = function () {
    stateLabel.innerHTML = "Connecting";
    socket = new WebSocket(connectionUrl.value);
    socket.onopen = function (event) {
        updateState();
        commsLog.innerHTML += '<tr>' +
            '<td colspan="3" class="commslog-data">Connection opened</td>' +
            '</tr>';
    };
    socket.onclose = function (event) {
        updateState();
        commsLog.innerHTML += '<tr>' +
            '<td colspan="3" class="commslog-data">Connection closed. Code: ' + htmlEscape(event.code) + '. Reason: ' + htmlEscape(event.reason) + '</td>' +
            '</tr>';
    };
    socket.onerror = updateState;

    socket.onmessage = function (event) {
        //commsLog.innerHTML += '<tr>' +
        //    '<td class="commslog-server">Server</td>' +
        //    '<td class="commslog-client">Client</td>' +
        //    '<td class="commslog-data">' + htmlEscape(event.data) + '</td></tr>';
        console.log("Przyszla wiadomosc: " + event.data)
        dispatch(event.data);
    };
};

function htmlEscape(str) {
    return str.toString()
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

var WSMessageType = {
    TEXT: "TEXT",
    SCORE: "SCORE",
    AUDIO: "AUDIO",
    OTHER: "OTHER",
}

function dispatch(message) {
    var type = message.split(SEPARATOR)[0];
    console.log(type);
    var text = message.substr(message.indexOf(" ")).trim();
    console.log(text);

    switch (type) {
        case WSMessageType.AUDIO:
            playMusic(text);
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

// functions for different message types:

function playMusic(text) {
    var ctx = new AudioContext();
}