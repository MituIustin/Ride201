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

    // Update Time Slider of picked items

    private IEnumerator consume(GameObject item)
    {
        Slider slider = GetComponent<Slider>();
        slider.value = 2000f;
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
