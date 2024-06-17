using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class McHealthScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        Slider slider = GetComponent<Slider>();
        GameObject player = GameObject.FindWithTag("player");
        slider.value = player.GetComponent<BaseClassCharacter>().getHealth();
    }
}
