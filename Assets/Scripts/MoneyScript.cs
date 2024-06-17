using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public int playerMoney;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = GameObject.FindWithTag("money").GetComponent<TextMeshProUGUI>();
        if (playerMoney == null)
        {
            playerMoney = 0;
        }
        else
        {
            playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        }
        
    }

    private void Update()
    {
        moneyText.text = "Bani: " + playerMoney.ToString();
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        SaveMoney();
    }

    public void SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            SaveMoney();
        }
        else
        {
            Debug.Log("Nu ai suficien?i bani!");
        }
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
    }

    void OnApplicationQuit()
    {
        SaveMoney();
    }
}
