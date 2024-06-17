using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class next : MonoBehaviour
{
    public float displayTime = 5.0f;
    public Text lvltext;
    public void Setup(int lvl)
    {
        lvltext.text = "LVL " + lvl.ToString();
        StartCoroutine(HideBackgroundAfterTime(displayTime));
    }

    System.Collections.IEnumerator HideBackgroundAfterTime(float time)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
