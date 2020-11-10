using UnityEngine;
using TMPro;

public class UIManager: MonoBehaviour {
    public static UIManager instance;

    public GameObject startMenu;
    public TMP_InputField usernameField;
    public TMP_InputField ipField;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer() {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        ipField.interactable = false;
        Client.instance.ip = ipField.text;
        Client.instance.ConnectToServer();
    }
}