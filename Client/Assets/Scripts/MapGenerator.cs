using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    solid = 7, // 111
    corridor = 0, // 000
    room1 = 4, // 100
    room2 = 2, // 010
    room3 = 1, // 001
    door = 6 // 110
}

public class MapGenerator : MonoBehaviour {
    public GameObject solid;
    public GameObject wall;
    public GameObject open;
    public GameObject door;

    public static MapGenerator instance;

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

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public GameObject GenerateMap(TileType[,] tiles) {
        GameObject mapGO = new GameObject("Map");

        //Color[] colours = map.GetPixels();
        //for (int y = 0; y < map.height; y++) {
        //    for (int x = 0; x < map.width; x++) {
        //        Color c = colours[y * map.width + x];
        //        int num = ((int)c.r * 4) | ((int)c.g * 2) | (int)c.b;

        //        tiles[x, y] = (TileType)num;
        //    }
        //}

        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (tiles[x, y] == TileType.solid) {
                    GameObject newPiece = Instantiate(solid, mapGO.transform);
                    newPiece.transform.position = new Vector3(x * 4 - width * 2, 0, y * 4 - height * 2);
                } else {
                    for (int i = 0; i < 4; i++) {
                        TileType facing;
                        if (x + orth[i].x < 0 || x + orth[i].x >= width) {
                            facing = TileType.solid;
                        } else if (y + orth[i].y < 0 || y + orth[i].y >= height) {
                            facing = TileType.solid;
                        } else {
                            facing = tiles[x + orth[i].x, y + orth[i].y];
                        }

                        GameObject piece;
                        if (tiles[x, y] == TileType.door) {
                            if (facing == TileType.corridor) {
                                piece = open;
                            } else if (facing == TileType.solid) {
                                piece = wall;
                            } else {
                                piece = door;
                            }
                        } else if (tiles[x, y] == TileType.corridor) {
                            if (facing == TileType.door) {
                                piece = open;
                            } else if (facing == TileType.solid) {
                                piece = wall;
                            } else {
                                piece = door;
                            }
                        } else {
                            if (facing == TileType.door) {
                                piece = door;
                            } else if (facing != tiles[x, y]) {
                                piece = wall;
                            } else {
                                piece = open;
                            }
                        }

                        if (tiles[x, y] == TileType.door) {
                            if (facing == TileType.corridor) {
                                piece = open;
                            } else if (facing == TileType.solid) {
                                piece = wall;
                            } else {
                                piece = door;
                            }
                        } else if (tiles[x, y] == TileType.corridor) {
                            if (facing == TileType.door) {
                                piece = open;
                            } else {
                                piece = tiles[x, y] != facing ? wall : open;
                            }
                        } else {
                            if (facing == TileType.door) {
                                piece = door;
                            } else {
                                piece = tiles[x, y] != facing ? wall : open;
                            }
                        }

                        //if (tiles[x, y] == TileType.door) {
                        //    if (facing == TileType.door) {
                        //        piece = door;
                        //    } else {
                        //        piece = facing == TileType.solid ? wall : open;
                        //    }
                        //} else {
                        //    if (facing != tiles[x, y] && facing != TileType.door) {
                        //        piece = wall;
                        //    } else {
                        //        piece = open;
                        //    }
                        //}

                        //if (tiles[x, y] == TileType.door) {
                        //    if (facing == TileType.corridor) {
                        //        piece = door;
                        //    } else {
                        //        piece = facing == TileType.solid ? wall : open;
                        //    }
                        //} else if (tiles[x, y] == TileType.corridor && facing == TileType.door) {
                        //    piece = door;
                        //} else {
                        //    if (facing == TileType.door) {
                        //        piece = open;
                        //    } else {
                        //        piece = facing != tiles[x, y] ? wall : open;
                        //    }
                        //}

                        GameObject newPiece = Instantiate(piece, mapGO.transform);
                        newPiece.transform.position = new Vector3(x * 4 - width * 2, 0, y * 4 - height * 2);
                        newPiece.transform.eulerAngles = new Vector3(0, rotations[i], 0);
                    }
                }
            }
        }

        return mapGO;
    }
}