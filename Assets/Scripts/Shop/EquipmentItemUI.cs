using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EquipmentItemUI : MonoBehaviour
{
    [SerializeField] Image EquipmentImage;
    [SerializeField] TMP_Text EquipmentName;
    [SerializeField] TMP_Text EquipmentDes;
    [SerializeField] TMP_Text EquipmentPrice;
    [SerializeField] Button EquipmentPurchaseButton;
    [SerializeField] TMP_Text EquipmentLevel;
    public void SetItemPos (Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    
    public void SetEquipmentImage (Sprite sprite)
    {
        EquipmentImage.sprite = sprite;
    }

    public void SetEquipmentName (string name)
    {
        EquipmentName.text = name;
    }

    public void SetEquipmentDes (string des)
    {
        EquipmentDes.text = des;
    }

    public void SetEquipmentPrice (int price)
    {
        EquipmentPrice.text = price.ToString();
    }
    public void SetEquipmentLevel (int level)
    {
        EquipmentLevel.text = level.ToString();
    }

    public void OnItemPurchase (Equipment equipment, UnityAction<Equipment> action)
    {
        EquipmentPurchaseButton.onClick.RemoveAllListeners ();
        EquipmentPurchaseButton.onClick.AddListener (() => action.Invoke (equipment));
    }
}
