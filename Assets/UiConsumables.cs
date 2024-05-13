using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiConsumables : MonoBehaviour
{

    public Sprite[] ConsumablesItems;

    public void AddItemUi(int index)
    {
        

        GameObject newImage = new GameObject("My Image");
        newImage.AddComponent<Image>();

        newImage.GetComponent<Image>().sprite = ConsumablesItems[index];


        //newImage.GetComponent<Image>().sprite = UIimage;

        
       
        newImage.transform.SetParent(GameObject.FindWithTag("scrollView").transform, false);
       
    }
}
