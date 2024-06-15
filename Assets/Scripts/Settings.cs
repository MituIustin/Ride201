using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void setEasy()
    {
        DifficultyClass.damageMultiplier = 0.75f; //8 lovituri
        SceneManager.LoadSceneAsync(0); //main menu
    }

    public void setMedium()
    {
        DifficultyClass.damageMultiplier = 1; //6 lovituri
        SceneManager.LoadSceneAsync(0); //main menu
    }

    public void setHard()
    {
        DifficultyClass.damageMultiplier = 1.5f; //4 lovituri
        SceneManager.LoadSceneAsync(0); //main menu
    }
}
