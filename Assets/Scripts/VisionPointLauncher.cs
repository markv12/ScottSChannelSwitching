using UnityEngine;
using System.Collections;

public class VisionPointLauncher : MonoBehaviour {

    public Transform theCamera;
    public GameObject projectilePrefab;

    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = theCamera.position + theCamera.forward;
            newProjectile.transform.rotation = theCamera.rotation;
        }
    }
}
