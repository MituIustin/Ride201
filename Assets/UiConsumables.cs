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

    public Sprite[] ConsumablesItems;

    public List<GameObject> buffs = new List<GameObject>();

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


        GameObject newImage = new GameObject("buff " + buffs.Count());
        newImage.AddComponent<Image>();

        newImage.GetComponent<Image>().sprite = ConsumablesItems[index];
               
        newImage.transform.SetParent(GameObject.FindWithTag("scrollView").transform, false);

        RectTransform pos = newImage.GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(0, 30 - 100 * buffs.Count());

        pos.sizeDelta = new Vector2(80, 80);

        buffs.Add(newImage);

        Debug.Log(buffs.Count());

        GameObject test = Instantiate(slider, new Vector3(105,-0,0), Quaternion.identity);

        test.transform.SetParent(newImage.transform, false);

        
        test.GetComponent<DurationBarScript>().TimeLeft(newImage);



        
    }
}
