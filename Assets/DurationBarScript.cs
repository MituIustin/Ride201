using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationBarScript : MonoBehaviour
{
    
    public void TimeLeft()
    {
        StartCoroutine(consume());
    }

    private IEnumerator consume()
    {
        GetComponent<Slider>().value = 1f;
        Debug.Log(GetComponent<Slider>().value);
        for (int i = 0; i < 1500; i++)
        {
            GetComponent<Slider>().value -= 0.0006f;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
