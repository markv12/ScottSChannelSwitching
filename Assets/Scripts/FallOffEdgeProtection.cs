using UnityEngine;

public class FallOffEdgeProtection : MonoBehaviour {

    [SerializeField]
    private Transform theTransform;
	
	void Update () {
		if(theTransform.position.y < -12) {
            theTransform.position = Vector3.zero;
        }
	}
}
