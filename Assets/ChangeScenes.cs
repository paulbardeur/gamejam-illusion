using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void goToLevelOne()
    {
        SceneManager.LoadScene("Level1");
    }

    public void goToHorloge()
    {
        SceneManager.LoadScene("Horloge");
    }

    public void goToAnimationOne()
    {
        SceneManager.LoadScene("Animation1");
    }

    public void goToAnimationTwo()
    {
        SceneManager.LoadScene("Animation2");
    }

    public void goToAnimationThree()
    {
        SceneManager.LoadScene("Animation3");
    }

    public void goToLevelTwo()
    {
        SceneManager.LoadScene("Level2");
    }

    public void goToLevelThree()
    {
        SceneManager.LoadScene("Level3");
    }

    public void goToLevelFour()
    {
        SceneManager.LoadScene("Level4");
    }

    public void goToLevelSix()
    {
        SceneManager.LoadScene("Level6");
    }

    public void goToLevelSeven()
    {
        SceneManager.LoadScene("Level7");
    }

    public void goToLevelHeight()
    {
        SceneManager.LoadScene("Level8");
    }
}
