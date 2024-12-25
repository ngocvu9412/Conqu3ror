using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EquipmentItemUI : MonoBehaviour
{
    [SerializeField] Image EquipmentImage;
    [SerializeField] Text EquipmentName;
    [SerializeField] Text EquipmentDes;
    [SerializeField] Text EquipmentPrice;
    [SerializeField] Button EquipmentPurchaseButton;
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

    public void OnItemPurchase (Equipment equipment, UnityAction<Equipment> action)
    {
        EquipmentPurchaseButton.onClick.RemoveAllListeners ();
        EquipmentPurchaseButton.onClick.AddListener (() => action.Invoke (equipment));
    }
}
