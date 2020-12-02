using System;
using System.Numerics;

public enum PlayerStates {
    none,
    waiting,
    ready,
    bunny,
    human
}

class Player {
    public int id;
    public string username;

    public Vector3 position;
    public Vector3 velocity;
    
    public PlayerStates state;

    public Player(int _id, string _username, Vector3 spawnPosition) {
        id = _id;
        username = _username;
        position = spawnPosition;
    }

    public void UpdatePlayerData(Vector3 _position, Vector3 _velocity) {
        position = _position;
        velocity = _velocity;

        ServerSend.UpdatePlayerData(this);
    }

    public void TeleportPlayer(Vector3 position) {
        UpdatePlayerData(position, Vector3.Zero);

        ServerSend.TeleportPlayer(this, position);
    }

    public void UpdatePlayerState(PlayerStates newState) {
        state = newState;

        ServerSend.SetPlayerState(id, state);
    }
}