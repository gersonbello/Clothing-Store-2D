using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFadeCanvasAnimation : MonoBehaviour
{
    public void StartFade(bool fadeIn)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (fadeIn) StartCoroutine(FadeIn(canvasGroup));
        else StartCoroutine(FadeOut(canvasGroup));
    }

    public IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        while(canvasGroup.alpha > 0.015f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, 5 * Time.fixedDeltaTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    public IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha < 0.95f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, 5 * Time.fixedDeltaTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}
