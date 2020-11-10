using System;
using System.Numerics;

public class Map {
    public int mapId;
    public bool[] doorStates;

    public Map(int _mapId) {
        mapId = _mapId;
        doorStates = new bool[1];
    }

    public void LoadMap(int _mapId) {
        mapId = _mapId;
        ServerSend.LoadMap(mapId);

        int playerCount = Server.clients.Count;
        double angle = 2 * Math.PI / playerCount;
        double radius = 4;
        int i = 0;
        foreach (int index in Server.clients.Keys) {
            if (Server.clients[index].player != null) {
                Vector3 position = new Vector3(
                    (float)(radius * Math.Cos(angle * i)),
                    1f,
                    (float)(radius * Math.Sin(angle * i))
                );

                Server.clients[index].player.TeleportPlayer(position);

                i++;
            }
        }
    }
}
