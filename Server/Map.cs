using System;
using System.Collections.Generic;
using System.IO;

public enum TileType {
    solid = 7, // 111
    corridor = 0, // 000
    room1 = 4, // 100
    room2 = 2, // 010
    room3 = 1, // 001
    door = 6 // 110
}

public class Map {
    public int mapId = -1;
    public TileType[,] tiles = new TileType[4, 4];
    public string mapFolder = @"/Users/Brians/Desktop/test/unity-multiplayer/Server/Maps";

    Dictionary<int, string> mapImagePathDictionary = new Dictionary<int, string>();

    public Map(int mapId) {
        GetMapImages();
        LoadMap(mapId);
    }

    public void LoadMap(int mapId) {
        if (this.mapId == mapId) {
            return;
        }

        if (!mapImagePathDictionary.ContainsKey(mapId)) {
            Console.WriteLine("Load Map: No map with given id!");
            return;
        }

        this.mapId = mapId;
        tiles = MapLoader.LoadMap(mapImagePathDictionary[mapId]);
        ServerSend.LoadMap(tiles);
    }

    void GetMapImages() {
        string appPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"Maps"));

        string[] mapImagePaths = Directory.GetFiles(appPath, "*.png");
        foreach (var mapImagePath in mapImagePaths) {
            string name = Path.GetFileNameWithoutExtension(mapImagePath);
            if (int.TryParse(name, out int mapIdOfImage)) {
                if (mapImagePathDictionary.ContainsKey(mapIdOfImage)) {
                    Console.WriteLine("GetMapImages: MapImagePathsDictionary already contains map image path!");
                }

                mapImagePathDictionary.Add(mapIdOfImage, mapImagePath);
                Console.WriteLine($"Found map {name}");
            }
        }
    }
}
