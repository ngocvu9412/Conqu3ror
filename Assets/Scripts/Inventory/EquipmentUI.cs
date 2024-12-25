using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] Image EquipmentImage;
    [SerializeField] Text EquipmentLevel;
    public int EquipmentIndex;
    public Vector2 previous_position;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float lastClickTime = 0f; // Thời gian lần nhấp trước
    private float doubleClickThreshold = 0.3f; // Ngưỡng thời gian để coi là double-click
    private bool CanDrag = false;
    public bool InfoValid = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetEquipmentPos(Vector2 pos)
    {
        rectTransform.anchoredPosition += pos;
    }

    public void SetEquipmentImage(Sprite sprite)
    {
        EquipmentImage.color = Color.white;
        EquipmentImage.sprite = sprite;
    }

    public void SetLevelText(int Level)
    {
        EquipmentLevel.text = "LV" + Level.ToString();
    }

    public void SetIndex(int index)
    {
        EquipmentIndex = index;
    }

    public void SetCanDrag(bool DragStatus)
    {
        CanDrag = DragStatus;
    }
    public void SetInfoValid( bool InfoStatus)
    {
        InfoValid = InfoStatus;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down on object: " + gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            Debug.LogWarning("Dragging is disabled for this object.");
            return;
        }

        Debug.Log("Begin Drag on object: " + gameObject.name);
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;

        Debug.Log("Dragging object: " + gameObject.name);
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;

        Debug.Log("End Drag on object: " + gameObject.name);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!IsPointerOverDropArea(eventData))
        {
            rectTransform.anchoredPosition = previous_position;
        }
    }

    private bool IsPointerOverDropArea(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<IDropHandler>() != null)
            {
                if( result.gameObject.GetComponent<EquipmentUI>() != null && result.gameObject.GetComponent<MergeEquipmentUI>())
                return true;
                if( result.gameObject.GetComponent<EquipmentSlot>() != null) 
                    if( CheckIfSlotInValid(result.gameObject))
                    return true;
            }
        }
        return false;
    }
    bool CheckIfSlotInValid(GameObject gameObject)
    {
        Equipment[] PlayerUsedEquips = GameDataManager.Ins.GetPlayerUsedEquips();
        switch (gameObject.name)
        {
            case "Slot1":
            {
                if(PlayerUsedEquips[0].Equals(default(Equipment)))
                {
                    return false;
                }
            }
            break;
            case "Slot2":
            {
                if(PlayerUsedEquips[0].Equals(default(Equipment)))
                {
                    return false;
                }
            }
            break;
            case "Slot3":
            {
                if(PlayerUsedEquips[0].Equals(default(Equipment)))
                {
                    return false;
                }
            }
            break;
            case "Slot4":
            {
                if(PlayerUsedEquips[0].Equals(default(Equipment)))
                {
                    return false;
                }
            }
            break;
        }
        return true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop on object: " + gameObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            HandleDoubleClick();
        }

        lastClickTime = Time.time;
    }

    private void HandleDoubleClick()
    {
        rectTransform.anchoredPosition = previous_position;
        GameDataManager.Ins.RemoveEquipmentUsed(EquipmentIndex);
    }
}
