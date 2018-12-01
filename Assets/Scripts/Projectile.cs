using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private Transform t;
    [SerializeField] private MeshRenderer meshR;

    private float speed = 35f;
    private Material theMat;
    private float timeAlive = 0;
    private const float LIFESPAN = 4f;

    private void Awake() {
        theMat = meshR.material;
    }

    void Update () {
        if (!exploded) {
            if (speed < 55f) {
                speed += Time.deltaTime*15;
            }
            t.position += t.forward * speed * Time.deltaTime;

            timeAlive += Time.deltaTime;
            if(timeAlive > LIFESPAN) {
                Destroy(gameObject);
            }
        }
	}

    private bool exploded = false;
    private void OnTriggerEnter(Collider other) {
        if (!exploded) {
            VisionPointManager.instance.AddVisionPoint(t.position);
            StartCoroutine(Explode());
            exploded = true;
        }
    }

    private IEnumerator Explode() {
        theMat.color = new Color(6, 6, 6);
        yield return null;
        theMat.color = new Color(10, 10, 10);
        yield return null;
        theMat.color = new Color(5, 5, 5);
        yield return null;
        theMat.color = new Color(3, 3, 3);
        yield return null;
        Destroy(gameObject);
    }
}
