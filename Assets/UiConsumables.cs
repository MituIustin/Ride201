using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiConsumables : MonoBehaviour
{

    public Sprite[] ConsumablesItems;

    public GameObject[] buffs;

    public GameObject slider;

    public void AddItemUi(int index)
    {
        

        GameObject newImage = new GameObject("buff " + buffs.Length);
        newImage.AddComponent<Image>();

        newImage.GetComponent<Image>().sprite = ConsumablesItems[index];
               
        newImage.transform.SetParent(GameObject.FindWithTag("scrollView").transform, false);

        RectTransform pos = newImage.GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(-80, 30 - 100 * buffs.Length);

        pos.sizeDelta = new Vector2(80, 80);

        buffs.Append(newImage);

        GameObject test = Instantiate(slider, new Vector3(105,-0,0), Quaternion.identity);

        test.transform.SetParent(newImage.transform, false);

        test.GetComponent<DurationBarScript>().TimeLeft();



        //newImage.
    }
}
