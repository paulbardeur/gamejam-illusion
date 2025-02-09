using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaitImage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitAndChangeScene(1f)); 
    }

    IEnumerator WaitAndChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Level2");
    }
}
