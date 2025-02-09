using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RevealImage : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 2f;
    public float displayDuration = 4f;
    public string nextSceneName;

    void Start()
    {
        if (image == null)
        {
            Debug.LogError("Image is not assigned! Please assign an Image in the Inspector.");
            return;
        }

        StartCoroutine(FadeInAndChangeScene());
    }

    IEnumerator FadeInAndChangeScene()
    {
        float elapsedTime = 0f;
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = 1f;
        image.color = color;

        yield return new WaitForSeconds(displayDuration);

        if (!string.IsNullOrEmpty(nextSceneName)) {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
