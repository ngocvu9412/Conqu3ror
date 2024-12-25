using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipmentUI : MonoBehaviour
{
    [SerializeField] Image EquipmentImage;
    [SerializeField] Text EquipmentLevel;
    
    private RectTransform rectTransform ;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
}
