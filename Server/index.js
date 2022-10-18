const WebSocket = require('ws')

let ordenLlegada = []
let jugadores = []
let puntos = []
let listos = []

let bancos = []

let nroJugador = 0
let nroBancos = 0
let correctAnswer = ""
let questionSetted = false
let multiplier = 0

const wss = new WebSocket.Server({ port:8080 }, () => {
    console.log('server started');
})

wss.on('listening', () => {
    console.log('server is listening in port 8080');
})

wss.addListener('connection', (ws) => {
    ws.on('message', (data) => {
        console.log('data received %o', data.toString());
        if (data.toString().split(" ")[0] === "bancos") {
            nroBancos = parseInt(data.toString().split(" ")[1])
        }
        else if (data.toString().split(" ")[0] === "bank" && bancos.length < nroBancos) {
            bancos.push(parseInt(data.toString().split(" ")[1]))
            console.log("bancos anadidos");
        }
        else if (data.toString().split(" ")[0] === "connect") {
            jugadores.push(data.toString().split(" ")[1])
            puntos.push(0)
            multiplier += 1
            ws.send("el jugador " + jugadores[nroJugador] + " se ha conectado! con el id " + nroJugador.toString())
            wss.clients.forEach(client => {
                for (let index = 0; index < jugadores.length; index++) {
                    client.send("player " + jugadores[index] + " = " + puntos[index] + " puntos")
                }
            });
            console.log(jugadores[nroJugador]);
            nroJugador += 1
        }
        else if (data.toString().startsWith("request") && !questionSetted) {
            let banco = Math.floor(Math.random() * bancos.length)
            let question = Math.floor(Math.random() * bancos[banco])
            wss.clients.forEach(client => {
                client.send("Question " + question + " " + banco)
            });
            questionSetted = true;
        }
        else if (data.toString().startsWith("answer")) {
            correctAnswer = data.toString().split("answer ")[1]
            console.log(correctAnswer);
        }
        else if (data.toString().startsWith("ready")) {
            listos.push(1)
            console.log(listos + ", " + jugadores);
            if (listos.length == jugadores.length) {
                console.log("start")
                wss.clients.forEach(client => {
                    client.send("start")
                });
            }
        }
        else if (data.toString().startsWith("selected")) {
            let index = parseInt(data.toString().split(" ")[1])
            puntos[index] += multiplier / 2
            multiplier -= 1
            if (multiplier == 0) {
                wss.clients.forEach(client => {
                    for (let index = 0; index < puntos.length; index++) {
                        client.send("puntaje " + jugadores[index] + " = " + puntos[index] + " puntos indice " + index)                   
                    }
                });
                questionSetted = false;
                multiplier = jugadores.length
            }
        }

        else if (data.toString().startsWith("wrong")) {
            multiplier -= 1
            if (multiplier == 0) {
                wss.clients.forEach(client => {
                    for (let index = 0; index < puntos.length; index++) {
                        client.send("puntaje " + puntos[index] + " " + index)                   
                    }
                });
                questionSetted = false;
                multiplier = jugadores.length
            }
        }
        
    })
    ws.on("open", (data) => {
        console.log("open received %o", data.toString())
    })
})

var score = 0