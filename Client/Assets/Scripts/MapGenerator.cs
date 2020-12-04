using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType {
    solid = 7, // 111
    corridor = 0, // 000
    room1 = 4, // 100
    room2 = 2, // 010
    room3 = 1, // 001
    door = 6 // 110
}

public class MapGenerator : MonoBehaviour {

    public Texture2D map;
    public GameObject solid;
    public GameObject wall;
    public GameObject open;
    public GameObject door;

    Vector2Int[] orth = new Vector2Int[] {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1)
    };

    float[] rotations = new float[] {
        270,
        180,
        90,
        0
    };

    public void GenerateMap() {
        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }
        
        TileType[,] tiles = new TileType[map.width, map.height];

        Color[] colours = map.GetPixels();
        for (int y = 0; y < map.height; y++) {
            for (int x = 0; x < map.width; x++) {
                Color c = colours[y * map.width + x];
                int num = (int)c.b | ((int)c.g * 2) | ((int)c.r * 4);

                tiles[x, y] = (TileType)num;
            }
        }

        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                if (tiles[x, y] == TileType.solid) {
                    GameObject newPiece = Instantiate(solid, transform);
                    newPiece.transform.position = new Vector3(x * 4, 0, y * 4);
                } else {
                    for (int i = 0; i < 4; i++) {
                        TileType facing;
                        if (x + orth[i].x < 0 || x + orth[i].x >= tiles.GetLength(0)) {
                            facing = TileType.solid;
                        } else if (y + orth[i].y < 0 || y + orth[i].y >= tiles.GetLength(1)) {
                            facing = TileType.solid;
                        } else {
                            facing = tiles[x + orth[i].x, y + orth[i].y];
                        }

                        GameObject piece;
                        if (tiles[x, y] == TileType.door) {
                            if (facing == TileType.corridor) {
                                piece = door;
                            } else {
                                piece = facing == TileType.solid ? wall : open;
                            }
                        } else if (tiles[x, y] == TileType.corridor && facing == TileType.door) {
                            piece = door;
                        } else {
                            if (facing == TileType.door) {
                                piece = open;
                            } else {
                                piece = facing != tiles[x, y] ? wall : open;
                            }
                        }

                        GameObject newPiece = Instantiate(piece, transform);
                        newPiece.transform.position = new Vector3(x * 4, 0, y * 4);
                        newPiece.transform.eulerAngles = new Vector3(0, rotations[i], 0);
                    }
                }
            }
        }
    }
}