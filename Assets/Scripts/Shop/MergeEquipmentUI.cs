using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MergeEquipmentUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image EquipmentImage;
    [SerializeField] TMP_Text EquipmentLevel;
    public int EquipmentIndex;
    public Vector2 previous_position;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool CanDrag;
    public bool InfoValid;
    public bool CanMerge;
    public void Initialize()
    {
        EquipmentImage.color = new Color(0.408f, 0.337f, 0.286f, 1.000f);
        EquipmentImage.sprite=null;
        EquipmentLevel.text="";
        EquipmentIndex = -1;
        CanDrag = false;
        InfoValid = false;
        CanMerge = false;
    }
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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            Debug.LogWarning("Dragging is disabled for this object.");
            return;
        }

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;

        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!IsPointerOverDropArea(eventData))
        {
            rectTransform.anchoredPosition = previous_position;
        }
        MergeEquipment(eventData);
    }

    private bool IsPointerOverDropArea(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<IDropHandler>() != null && result.gameObject.GetComponent<EquipmentSlot>() != null && result.gameObject.GetComponent<EquipmentUI>() != null && result.gameObject.GetComponent<MergeEquipmentUI>() != null)
            {
                return true;
            }
        }
        return false;
    }
    private void MergeEquipment(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<MergeEquipmentUI>() != null && (result.gameObject.GetComponent<MergeEquipmentUI>().CanMerge = CanMerge = true)) 
            {
                GameDataManager.MergeEquipment(EquipmentIndex, result.gameObject.GetComponent<MergeEquipmentUI>().EquipmentIndex);
                gameObject.GetComponentInParent<MergeEquipmentShowUI>().ResetMergeEquipUI();
                return;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop on object: " + gameObject.name);
    }
}
