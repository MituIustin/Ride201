﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update Healthbars

public class HealthBarScript : MonoBehaviour
{
    public void changeHealth(float health)
    {
        Slider slider = GetComponent<Slider>();
        slider.value = health;
    }
}
