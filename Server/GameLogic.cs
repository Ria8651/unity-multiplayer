using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public enum PlayerStates {
    none,
    lobby,
    bunny,
    human
}

class GameLogic {
    public static Dictionary<int, ClienData> clientData = new Dictionary<int, ClienData>();

    public static void Update() {
        ThreadManager.UpdateMain();
    }

    public static void StartGame() {
        if (Server.map.mapId != 0) {
            Console.WriteLine("Start Game: Game already in progress!");
            return;
        }

        Client[] onlinePlayers = Server.OnlinePlayers();
        Vector3[] spawns = GenerateSpawns(onlinePlayers.Length);

        PlayerStates[] roles = new PlayerStates[onlinePlayers.Length];
        for (int i = 0; i < roles.Length; i++) { roles[i] = PlayerStates.human; }
        roles[0] = PlayerStates.bunny;

        Random random = new Random();
        roles = roles.OrderBy(x => random.Next()).ToArray();

        for (int i = 0; i < onlinePlayers.Length; i++) {
            ServerSend.TeleportPlayer(onlinePlayers[i].player, spawns[i]);
            ServerSend.SetPlayerState(onlinePlayers[i].id, roles[i]);
        }

        Server.map.LoadMap(1);
    }

    public static void ReadyUpClient(int client) {
        if (Server.map.mapId == 0) {
            clientData[client].ready = true;

            foreach (ClienData data in clientData.Values) {
                if (!data.ready) {
                    return;
                }
            }
            
            if (Server.OnlinePlayers().Length >= 2) {
                StartGame();
            }
        }
    }

    static Vector3[] GenerateSpawns(int size) {
        Vector3[] spawns = new Vector3[size];

        double angle = 2 * Math.PI / size;
        double radius = 4;

        for (int i = 0; i < size; i++) {
            spawns[i] = new Vector3(
                (float)(radius * Math.Sin(angle * i)),
                1f,
                (float)(radius * Math.Cos(angle * i))
            );
        }

        return spawns;
    }
}

public class ClienData {
    public bool ready;
    public PlayerStates state;
}