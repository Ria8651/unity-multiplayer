using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public int id;
    public string username;

    public void Teleport(Vector3 position) {
        transform.position = position;

        if (GetComponent<Rigidbody>() != null) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
