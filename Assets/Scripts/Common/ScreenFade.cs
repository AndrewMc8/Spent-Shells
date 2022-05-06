using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image image = null;
    [SerializeField] float time;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] bool startOnAwake = true;
    

    public bool isDone { get; set; } = false;

    void Start()
    {
        if (startOnAwake)
        {
            FadeIn();
        }
    }

    public void FadeIn()
	{
        isDone = false;
        image.gameObject.SetActive(true);

        StartCoroutine(FadeRoutine(startColor, endColor, time));
    }

    public void FadeOut(bool deactivate = true)
    {
        isDone = false;
        image.gameObject.SetActive(true);

        StartCoroutine(FadeRoutine(endColor, startColor, time, deactivate));
    }

    IEnumerator FadeRoutine(Color color1, Color color2, float time, bool deactivate = true)
    {
        float timer = 0;
        while (timer < time)
        {
            timer = timer + Time.unscaledDeltaTime;
            image.color = Color.Lerp(color1, color2, timer/time);

            yield return null;
        }

        if(deactivate) image.gameObject.SetActive(false);
        isDone = true;
    }
}
