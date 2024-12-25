using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ESlot On Drop");
        if (eventData.pointerDrag != null)
        {
            if(eventData.pointerDrag.GetComponent<EquipmentUI>().InfoValid)
            {
                switch (gameObject.name)
                {
                    case "Slot1": // Phải khớp chính xác với "Slot1"
                    {
                        SetEquipToSlot(0,eventData);
                    }
                        break;
                    case "Slot2":
                    {
                        SetEquipToSlot(1,eventData);
                    }
                        break;
                    case "Slot3":
                    {
                        SetEquipToSlot(2,eventData);  
                    }
                        break;
                    case "Slot4":
                    {
                        SetEquipToSlot(3,eventData);  
                    }
                        break;
                }
            }
        }
    }
    private void SetEquipToSlot(int slot_index,PointerEventData eventData)
    {
        if(GameDataManager.Ins.CanUseEquipment(slot_index,eventData.pointerDrag.GetComponent<EquipmentUI>().EquipmentIndex))
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            GameDataManager.Ins.AddEquipmentToSlot(slot_index,eventData.pointerDrag.GetComponent<EquipmentUI>().EquipmentIndex);
        }
        else eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<EquipmentUI>().previous_position;
    }
}
