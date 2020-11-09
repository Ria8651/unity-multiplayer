using System;
using System.Numerics;

class Player {
    public int id;
    public string username;

    public Vector3 position;
    public Vector3 velocity;
    public Quaternion rotation;

    public Player(int _id, string _username, Vector3 spawnPosition) {
        id = _id;
        username = _username;
        position = spawnPosition;
        rotation = Quaternion.Identity;
    }

    public void UpdatePlayerData(Vector3 _position, Vector3 _velocity) {
        position = _position;
        velocity = _velocity;

        ServerSend.UpdatePlayerData(this);
    }
}