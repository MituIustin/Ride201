using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BASE CLASS FOR NPCS AND MAIN CHARACTER

public class BaseClassCharacter : MonoBehaviour
{
    public string ClassName { get; set; }
    public int Health { get; set; }
    public float Speed { get; set; }
    public int Damage { get; set; }
    public bool Attack { get; set; }
    public bool Hostile { get; set; }

    float health = 300;

    // Constructor

    public BaseClassCharacter()
    {
        health = 300;
    }

    public void getPunched(float enemy_damage)
    {
        health -= enemy_damage;
    }
    
    // Getters

    public float getHealth()
    {
        return health;
    }

    public float getDamage()
    {
        return Damage;
    }


    // Setters

    public void setHealth(int heal)
    {
        health = heal;
    }
}