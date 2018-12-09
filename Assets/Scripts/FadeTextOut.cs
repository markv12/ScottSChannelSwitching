using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeTextOut : MonoBehaviour {

    [SerializeField] private Text clickText;

    void Start() {
        StartCoroutine(FadeOutClickText());
    }

    private const float CLICK_FADE_TIME = 2f;
    private IEnumerator FadeOutClickText() {
        yield return null;
        Color startColor = Color.white;
        Color endColor = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.666f);
        float elapsedTime = 0;
        float progress = 0;

        while (progress < 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / CLICK_FADE_TIME;
            float easedProgress = Easing.easeInOutSine(0, 1, progress);
            clickText.color = Color.Lerp(endColor, startColor, easedProgress);
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        elapsedTime = 0;
        progress = 0;
        while (progress < 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / CLICK_FADE_TIME;
            float easedProgress = Easing.easeInOutSine(0, 1, progress);
            clickText.color = Color.Lerp(startColor, endColor, easedProgress);
            yield return null;
        }
        Destroy(clickText);
    }
}
