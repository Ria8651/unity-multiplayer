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
        using (Packet _packet = new Packet((int)ServerPackets.welcome)) {
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

            SendTCPData(_toClient, _packet);
        }
    }

    public static void KickPlayer(int id, string mesage) {
        using (Packet _packet = new Packet((int)ServerPackets.kickPlayer)) {
            _packet.Write(id);
            _packet.Write(mesage);

            SendTCPDataToAll(_packet);
        }
    }

    public static void UpdatePlayerData(Player player, bool excludeSelf = true) {
        using (Packet _packet = new Packet((int)ServerPackets.playerData)) {
            _packet.Write(player.id);

            _packet.Write(player.position);
            _packet.Write(player.velocity);

            if (excludeSelf) {
                SendUDPDataToAll(player.id, _packet);
            } else {
                SendUDPDataToAll(_packet);
            }
        }
    }

    public static void LoadMap(TileType[,] tiles) {
        using (Packet _packet = new Packet((int)ServerPackets.loadMap)) {
            _packet.Write(tiles);

            SendTCPDataToAll(_packet);
        }
    }

    public static void InitiliseMap(int id) {
        using (Packet _packet = new Packet((int)ServerPackets.loadMap)) {
            _packet.Write(Server.map.tiles);

            SendTCPData(id, _packet);
        }
    }

    public static void SetPlayerState(int id, PlayerStates state) {
        using (Packet _packet = new Packet((int)ServerPackets.setPlayerState)) {
            _packet.Write(id);
            _packet.Write((int)state);

            SendTCPDataToAll(_packet);
        }
    }

    public static void TeleportPlayer(Player player, Vector3 position) {
        using (Packet _packet = new Packet((int)ServerPackets.teleportPlayer)) {
            _packet.Write(position);

            SendTCPData(player.id, _packet);
        }
    }
}