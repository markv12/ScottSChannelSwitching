using UnityEngine;
using System.Collections;

public class VisionPointLauncher : MonoBehaviour {

    public VisionPointManager vpManager;
    public Transform theCamera;
    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if(Physics.Raycast(theCamera.position, theCamera.forward, out hit, 1000)) {
                vpManager.AddVisionPoint(hit.point);
            }
        }
    }
}
