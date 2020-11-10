using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    public int id;
    public string username;
    public Text usernameFeild;

    public void Teleport(Vector3 position) {
        transform.position = position;

        if (GetComponent<Rigidbody>() != null) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
