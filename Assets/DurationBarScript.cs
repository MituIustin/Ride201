using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationBarScript : MonoBehaviour
{
    
    public void TimeLeft(GameObject item)
    {
        StartCoroutine(consume(item));
    }

    private IEnumerator consume(GameObject item)
    {
        Debug.Log("START");
        Slider slider = GetComponent<Slider>();
        slider.value = 2000f;
        Debug.Log(slider.value);
        float totalTime = 20f;
        float endTime = Time.time + totalTime;

        while (Time.time < endTime)
        {
            float remainingTime = endTime - Time.time; 
            slider.value = 2000f * (remainingTime / totalTime); 
            yield return null; 
        }

        Destroy(slider);
        Destroy(item);
 
    }

}
