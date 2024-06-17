using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiConsumables : MonoBehaviour
{

    
    public List<GameObject> buffs = new List<GameObject>();

    
    public Sprite speedBuffSprite;
    public Sprite powerBuffSprite;

    public GameObject slider;

    
    void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (i >= buffs.Count)
            {
                continue;
            }

            if (buffs[i] == null)
            {
                buffs.RemoveAt(i);
                backInPlace(i);
                i++;
            }
        }
    }

    private void Start()
    {
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("consumable");
        foreach(GameObject consumable in consumables)
        {
            if (consumable.name == "Hell")
            {
                speedBuffSprite = consumable.GetComponent<SpriteRenderer>().sprite;
            }
            else if(consumable.name== "Protein")
            {

                powerBuffSprite = consumable.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void backInPlace(int index)
    {
        if (index < 0 || index >= buffs.Count)
        {
            //Debug.LogError("Invalid index passed to backInPlace");
            return;
        }

        for (int i = index; i < buffs.Count(); i++)
        {
            GameObject buff=buffs[i];

            buff.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 30 - 100 * i);


        }
    }

    public void AddItemUi(int index)
    {

        Debug.Log(buffs);

        if (speedBuffSprite == null)
        {
            GameObject[] consumables = GameObject.FindGameObjectsWithTag("consumable");
            foreach (GameObject consumable in consumables)
            {
                if (consumable.name == "Hell")
                {
                    speedBuffSprite = consumable.GetComponent<SpriteRenderer>().sprite;
                }
                else if (consumable.name == "Protein")
                {

                    powerBuffSprite = consumable.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }



        GameObject newImage = new GameObject("buff " + buffs.Count());
        newImage.AddComponent<Image>();

        if (index == 0)
        {

            newImage.GetComponent<Image>().sprite = speedBuffSprite;
        }
        else
        {

            newImage.GetComponent<Image>().sprite = powerBuffSprite;
        }


        newImage.transform.SetParent(GameObject.FindWithTag("scrollView").transform, false);

        RectTransform pos = newImage.GetComponent<RectTransform>();

        
        for(int i = buffs.Count - 1; i >= 0; i--)
        {
            if (newImage.GetComponent<Image>().sprite == buffs[i].GetComponent<Image>().sprite)
            {
                Destroy(buffs[i]);
                Update();


                break;
            }
        }
        int y = buffs.Count();

        
        pos.anchoredPosition = new Vector2(0, 30 - 100 * buffs.Count());

        pos.sizeDelta = new Vector2(80, 80);

        buffs.Add(newImage);

        Debug.Log(buffs.Count());

        GameObject test = Instantiate(slider, new Vector3(105,-0,0), Quaternion.identity);

        test.transform.SetParent(newImage.transform, false);

        
        test.GetComponent<DurationBarScript>().TimeLeft(newImage);

        
    }
}
