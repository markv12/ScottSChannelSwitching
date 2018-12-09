using UnityEngine;

public class FallOffEdgeProtection : MonoBehaviour {

    [SerializeField]
    private Transform theTransform;
    private Vector3 startPos;

    private void Awake() {
        startPos = theTransform.position;
    }

    void Update () {
		if(theTransform.position.y < -15) {
            theTransform.position = startPos;
        }
	}
}
