using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour {
    public static void Welcome(Packet _packet) {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet) {
        int _id = _packet.ReadInt();

        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void KickPlayer(Packet _packet) {
        int id = _packet.ReadInt();
        string message = _packet.ReadString();

        if (id == Client.instance.myId) {
            Debug.Log("You were kicked for: " + message);
            Client.instance.Disconnect();

            foreach (int player in GameManager.players.Keys) {
                GameManager.instance.DestroyPlayer(player);
            }
        } else {
            GameManager.instance.DestroyPlayer(id);
        }
    }

    public static void PlayerData(Packet _packet) {
        int _id = _packet.ReadInt();

        Vector3 position = _packet.ReadVector3();
        Vector3 velocity = _packet.ReadVector3();

        GameManager.players[_id].UpdatePlayerData(position, velocity);
    }

    public static void LoadMap(Packet _packet) {
        int mapId = _packet.ReadInt();

        GameManager.instance.LoadMap(mapId);
    }

    public static void SetPlayerState(Packet _packet) {
        int id = _packet.ReadInt();
        PlayerStates state = (PlayerStates)_packet.ReadInt();
        
        GameManager.players[id].UpdatePlayerState(state);
    }

    public static void Teleport(Packet _packet) {
        Vector3 position = _packet.ReadVector3();

        GameManager.players[Client.instance.myId].Teleport(position);
    }
}