using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Next LEVEL Script

public class next : MonoBehaviour
{
    public float displayTime = 5.0f;    // level title card duration
    public Text lvltext;                // displayed text

    public void Setup(int lvl)
    {
        lvltext.text = "LVL " + lvl.ToString();
        StartCoroutine(HideBackgroundAfterTime(displayTime));
    }

    // Enable and Disable level screen

    System.Collections.IEnumerator HideBackgroundAfterTime(float time)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
