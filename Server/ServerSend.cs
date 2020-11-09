using System;
using System.Numerics;

class ServerSend {
    static void SendTCPData(int _toClient, Packet _packet) {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    static void SendUDPData(int _toClient, Packet _packet) {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    static void SendTCPDataToAll(Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++) {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    static void SendTCPDataToAll(int _exceptClient, Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++) {
            if (i != _exceptClient) {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    static void SendUDPDataToAll(Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++) {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    static void SendUDPDataToAll(int _exceptClient, Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++) {
            if (i != _exceptClient) {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    public static void Welcome(int _toClient, string _msg) {
        using(Packet _packet = new Packet((int) ServerPackets.welcome)) {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player player) {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer)) {
            _packet.Write(player.id);
            _packet.Write(player.username);
            _packet.Write(player.position);
            _packet.Write(player.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void UpdatePlayerData(Player player) {
        using (Packet _packet = new Packet((int)ServerPackets.playerData)) {
            _packet.Write(player.id);
            
            _packet.Write(player.position);
            _packet.Write(player.velocity);

            SendUDPDataToAll(player.id, _packet);
        }
    }

    public static void LoadMap(int mapId) {
        using (Packet _packet = new Packet((int)ServerPackets.loadMap)) {
            _packet.Write(mapId);

            SendTCPDataToAll(_packet);
        }
    }
}