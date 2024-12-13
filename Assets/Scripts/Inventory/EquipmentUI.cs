using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] Image EquipmentImage;
    public void SetEquipmentPos (Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    
    public void SetEquipmentImage (Sprite sprite)
    {
        EquipmentImage.sprite = sprite;
    }

    public void SetEquipmentPurchasedStatus (bool purchase)
    {
        if ( !purchase ) EquipmentImage.color = Color.gray;
    }
}
