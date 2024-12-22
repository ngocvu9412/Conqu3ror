using UnityEngine;
using UnityEngine.UI;

public class SkillDialogController : MonoBehaviour
{
    public Text skillNameText;
    public Text skillDescriptionText;
    public Image skillIconImage;

    public void SetSkillDetails(string name, string description, Sprite icon)
    {
        if (skillNameText != null)
            skillNameText.text = name;

        if (skillDescriptionText != null)
            skillDescriptionText.text = description;

        if (skillIconImage != null)
            skillIconImage.sprite = icon;
    }
}
