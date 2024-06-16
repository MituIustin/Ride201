using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClassCharacter : MonoBehaviour
{
    public string ClassName { get; set; }
    public int Health { get; set; }
    public float Speed { get; set; }
    public int Damage { get; set; }
    public bool Attack { get; set; }
    public bool Hostile { get; set; }

    float health = 300;

    public BaseClassCharacter()
    {
        health = 300;

    }

    public void getPunched(float enemy_damage)
    {
        health -= enemy_damage;

    }

    public float getHealth()
    {
        return health;

    }

    public float getDamage()
    {
        return Damage;
    }

    public void setHealth(int heal)
    {
        health = heal;
    }




}