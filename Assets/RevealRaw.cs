using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RevealRaw : MonoBehaviour
{
    public RawImage rawImage;
    public float fadeDuration = 2f;
    public float displayDuration = 4f;
    public string nextSceneName;

    void Start()
    {
        if (rawImage == null)
        {
            Debug.LogError("RawImage is not assigned! Please assign a RawImage in the Inspector.");
            return;
        }

        StartCoroutine(FadeInAndChangeScene());
    }

    IEnumerator FadeInAndChangeScene()
    {
        float elapsedTime = 0f;
        Color color = rawImage.color;
        color.a = 0f;
        rawImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            rawImage.color = color;
            yield return null;
        }

        color.a = 1f;
        rawImage.color = color;

        yield return new WaitForSeconds(displayDuration);

        if (!string.IsNullOrEmpty(nextSceneName)) 
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
