﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VisionPointManager : MonoBehaviour {

    public static VisionPointManager instance;

    private const float distanceLimit = 6.4f;
    private const int POINT_COUNT = 25;

    private int currentPointIndex = 0;
    private Vector4[] points = new Vector4[POINT_COUNT];
    private Coroutine[] pointRoutines = new Coroutine[POINT_COUNT];

    private void Awake() {
        instance = this;
    }

    public void AddVisionPoint(Vector3 point) {

        points[currentPointIndex] = new Vector4(point.x, point.y, point.z, 0);
        this.EnsureCoroutineStopped(ref pointRoutines[currentPointIndex]);
        pointRoutines[currentPointIndex] = StartCoroutine(VisionPointRoutine(currentPointIndex));

        currentPointIndex++;
        currentPointIndex %= POINT_COUNT;
    }

    void Update() {
        Shader.SetGlobalVectorArray("VisionPoints", points);
    }

    private const float EXPAND_TIME = 0.666f;
    private const float CONTRACT_TIME = 18f;
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

        elapsedTime = 0;
        progress = 0;
        while (progress < 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / CONTRACT_TIME;
            float easedProgress = Easing.easeInSine(1, -0.18f, progress);
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
