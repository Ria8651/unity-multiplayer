using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

class GameLogic {
    static Random random;

    public static void Update() {
        ThreadManager.UpdateMain();
    }

    public static void StartGame() {
        if (Server.map.mapId != 0) {
            Console.WriteLine("Start Game: Game already in progress!");
            return;
        }

        Console.WriteLine("Starting Game...");

        Client[] onlinePlayers = Server.OnlinePlayers();
        Vector3[] spawns = GenerateSpawns(onlinePlayers.Length);

        PlayerStates[] roles = new PlayerStates[onlinePlayers.Length];
        for (int i = 0; i < roles.Length; i++) { roles[i] = PlayerStates.human; }
        roles[0] = PlayerStates.bunny;

        if (random == null) {
            random = new Random();
        }

        roles = roles.OrderBy(x => random.Next()).ToArray();

        for (int i = 0; i < onlinePlayers.Length; i++) {
            onlinePlayers[i].player.TeleportPlayer(spawns[i]);
            onlinePlayers[i].player.UpdatePlayerState(roles[i]);

            onlinePlayers[i].player.ready = false;
        }

        Server.map.LoadMap(1);
    }

    public static void EndGame() {
        if (Server.map.mapId == 0) {
            Console.WriteLine("End Game: Game not running!");
            return;
        }

        Console.WriteLine("Game Over");

        Client[] onlinePlayers = Server.OnlinePlayers();
        Vector3[] spawns = GenerateSpawns(onlinePlayers.Length);

        for (int i = 0; i < onlinePlayers.Length; i++) {
            onlinePlayers[i].player.TeleportPlayer(spawns[i]);
            onlinePlayers[i].player.UpdatePlayerState(PlayerStates.lobby);
        }

        Server.map.LoadMap(0);
    }

    public static void ReadyUpClient(int clientID) {
        if (Server.map.mapId == 0) {
            Server.clients[clientID].player.ready = true;
        }

        CheckGameState();
    }

    public static void CheckGameState() {
        if (Server.map.mapId == 0) {
            foreach (Client client in Server.OnlinePlayers()) {
                if (!client.player.ready) {
                    return;
                }
            }

            if (Server.OnlinePlayers().Length >= 2) {
                StartGame();
            }
        } else {
            foreach (Client client in Server.OnlinePlayers()) {
                if (client.player.state != PlayerStates.bunny) {
                    return;
                }
            }

            EndGame();
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

    public static void InfectPlayer(int bunnyID, int humanID) {
        if (Server.map.mapId == 0) {
            return;
        }

        Player bunny = Server.clients[bunnyID].player;
        Player human = Server.clients[humanID].player;

        if ((bunny.position - human.position).Length() > 0.7f) {
            return;
        }

        human.UpdatePlayerState(PlayerStates.bunny);

        CheckGameState();
    }
}