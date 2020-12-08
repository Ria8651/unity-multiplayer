using System;
using System.Numerics;

class ServerHandle {
    public static void WelcomeReceived(int _fromClient, Packet _packet) {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck) {
            Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerData(int _fromClient, Packet _packet) {
        if (Server.clients[_fromClient].player != null) {
            Vector3 position = _packet.ReadVector3();
            Vector3 velocity = _packet.ReadVector3();

            Server.clients[_fromClient].player.UpdatePlayerData(position, velocity);
        }
    }

    public static void ClientReady(int _fromClient, Packet _packet) {
        GameLogic.ReadyUpClient(_fromClient);
    }

    public static void InfectPlayer(int _fromClient, Packet _packet) {
        int playerToInfect = _packet.ReadInt();

        GameLogic.InfectPlayer(_fromClient, playerToInfect);
    }

    public static void Debug(int _fromClient, Packet _packet) {
        if (Server.map.mapId == 0) {
            Server.map.LoadMap(1);
        } else if (Server.map.mapId == 1) {
            Server.map.LoadMap(2);
        } else if (Server.map.mapId == 2) {
            Server.map.LoadMap(0);
        }
    }
}