using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buyScript : MonoBehaviour
{

    private int boughtKit1;
    private int boughtKit2;
    void Start()
    {
        boughtKit1 = PlayerPrefs.GetInt("kit1",0);
        boughtKit2 = PlayerPrefs.GetInt("kit2",0);
    }

    // Update is called once per frame
    void Update()
    {
        if (boughtKit1==1)
        {
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(false);

        }
        if (boughtKit2==1)
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
        if (money >= 50 && boughtKit1==0)  
        {
            boughtKit1 = 1;
            PlayerPrefs.SetInt("kit1", 1);
            GameObject.FindWithTag("money").GetComponent<CurrencyManager>().SpendMoney(2);
            GameObject.FindWithTag("soldEnergy").transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void buyProtein()
    {
        int money = PlayerPrefs.GetInt("PlayerMoney",0);
        if (money >= 70 && boughtKit2==0)
        {
            boughtKit2 = 1;
            PlayerPrefs.SetInt("kit2", 2);
            GameObject.FindWithTag("money").GetComponent<CurrencyManager>().SpendMoney(2);
            GameObject.FindWithTag("soldProtein").transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
