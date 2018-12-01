using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private Transform t;
    [SerializeField] private MeshRenderer meshR;

    [SerializeField] public AudioSource explosionSource;

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
            StartCoroutine(Explode());
            exploded = true;
        }
    }

    private IEnumerator Explode() {
        explosionSource.Play();
        yield return null;
        theMat.color = new Color(3, 3, 3);
        yield return null;
        t.localScale *= 2;
        theMat.color = new Color(5, 5, 5);
        t.localScale *= 2;
        yield return null;
        t.localScale *= 0.5f;
        theMat.color = new Color(3, 3, 3);
        yield return null;
        VisionPointManager.instance.AddVisionPoint(t.position);
        yield return null;
        meshR.enabled = false;
        while (explosionSource.isPlaying) {
            yield return null;
        }
        Destroy(gameObject);
    }
}
