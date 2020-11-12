using System;

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
    }
}
