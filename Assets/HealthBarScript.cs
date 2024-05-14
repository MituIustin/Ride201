using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public void changeHealth(int health)
    {
        Slider slider = GetComponent<Slider>();
        slider.value = health;
    }
}
