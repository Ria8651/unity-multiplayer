using UnityEngine;

public class Map : MonoBehaviour {
    
    bool[] doorStates;

    public void CloseDoor(int door) {
        if (door >= doorStates.Length || door < 0) {
            Debug.LogError("Close Door: Index outside bouds of array!");
            return;
        }
    }
}