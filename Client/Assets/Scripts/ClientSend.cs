using UnityEngine;

public class ClientSend : MonoBehaviour {
    static void SendTCPData(Packet _packet) {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }
    
    static void SendUDPData(Packet _packet) {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived() {
        using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived)) {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerData(Vector3 position, Vector3 velocity) {
        using (Packet _packet = new Packet((int)ClientPackets.playerData)) {
            _packet.Write(position);
            _packet.Write(velocity);

            SendUDPData(_packet);
        }
    }

    public static void LoadMap(int map) {
        using (Packet _packet = new Packet((int)ClientPackets.loadMap)) {
            _packet.Write(map);

            SendTCPData(_packet);
        }
    }
}