using UnityEngine;

public class Spinner : MonoBehaviour {

    [SerializeField] Transform t;
    [SerializeField] float speed = 0;
	void Update () {
        t.Rotate(new Vector3(0, speed, 0));
	}
}
