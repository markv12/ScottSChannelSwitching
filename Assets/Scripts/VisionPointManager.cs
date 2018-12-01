using System.Collections;
using UnityEngine;

public class VisionPointManager : MonoBehaviour {

    [Range(0, 125)]
    public float distanceLimit = 10;

    private const int POINT_COUNT = 25;

    private int currentPointIndex = 0;
    private Vector4[] points = new Vector4[POINT_COUNT];
    private Coroutine[] pointRoutines = new Coroutine[POINT_COUNT];

    public void AddVisionPoint(Vector3 point) {
        points[currentPointIndex] = new Vector4(point.x, point.y, point.z, 0);
        this.EnsureCoroutineStopped(ref pointRoutines[currentPointIndex]);
        pointRoutines[currentPointIndex] = StartCoroutine(VisionPointRoutine(currentPointIndex));

        currentPointIndex++;
        currentPointIndex %= POINT_COUNT;
    }

    void Update() {
        //distanceLimit = 15f + Mathf.Sin(Time.time) * 2;
        Shader.SetGlobalVectorArray("VisionPoints", points);
    }

    private const float EXPAND_TIME = 0.5f;
    private const float CONTRACT_TIME = 20f;
    private IEnumerator VisionPointRoutine(int index) {
        float elapsedTime = 0;
        float progress = 0;
        while(progress < 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / EXPAND_TIME;
            float easedProgress = Easing.easeOutQuint(0, 1, progress);
            SetPointExpandAmount(index, (easedProgress * distanceLimit));
            yield return null;
        }
        SetPointExpandAmount(index, distanceLimit);

        yield return new WaitForSeconds(0.666f);
        elapsedTime = 0;
        progress = 0;
        while (progress < 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / CONTRACT_TIME;
            float easedProgress = Easing.easeInSine(1, 0, progress);
            SetPointExpandAmount(index, (easedProgress * distanceLimit));

            yield return null;
        }
        pointRoutines[index] = null;
    }

    private void SetPointExpandAmount(int index, float amount) {
        Vector4 thePoint = points[index];
        points[index] = new Vector4(thePoint.x, thePoint.y, thePoint.z, amount);
    }
}
