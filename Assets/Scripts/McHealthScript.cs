using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class McHealthScript : MonoBehaviour
{
    void Update()
    {
        Slider slider = GetComponent<Slider>();
        GameObject player = GameObject.FindWithTag("player");
        if(player != null)
            slider.value = player.GetComponent<BaseClassCharacter>().getHealth();
    }
}
