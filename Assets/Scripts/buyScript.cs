using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shop Script 

    /*
     *      You can buy just one
     *   item of both types per run. 
     * 
     * */

public class buyScript : MonoBehaviour
{

    private int boughtKit1;     // heaven
    private int boughtKit2;     // protein

    void Start()
    {
        boughtKit1 = PlayerPrefs.GetInt("kit1",0);
        boughtKit2 = PlayerPrefs.GetInt("kit2",0);
        if (boughtKit1 == 1)
        {
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(false);
        }
        if (boughtKit2 == 1)
        {
            GameObject.FindWithTag("soldProtein").transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            GameObject.FindWithTag("soldProtein").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void buyEnergy()
    {
        int money = PlayerPrefs.GetInt("PlayerMoney", 0);
        if (money >= 20 && boughtKit1==0)  
        {
            boughtKit1 = 1;
            PlayerPrefs.SetInt("kit1", 1);
            GameObject.FindWithTag("money").GetComponent<CurrencyManager>().SpendMoney(20);
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void buyProtein()
    {
        int money = PlayerPrefs.GetInt("PlayerMoney",0);
        if (money >= 40 && boughtKit2==0)
        {
            boughtKit2 = 1;
            PlayerPrefs.SetInt("kit2", 1);
            GameObject.FindWithTag("money").GetComponent<CurrencyManager>().SpendMoney(40);           
            GameObject.FindWithTag("soldProtein").transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
