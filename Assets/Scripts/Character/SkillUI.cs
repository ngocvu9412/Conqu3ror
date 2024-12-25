using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Image SkillImage;
    [SerializeField] Text SkillName;
    [SerializeField] Text SkillDes;
    public void SetItemPos (Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    
    public void SetSkillImage (Sprite sprite)
    {
        SkillImage.sprite = sprite;
    }

    public void SetSkillName (string name)
    {
        SkillName.text = name;
    }

    public void SetSkillDes (string des)
    {
        SkillDes.text = des;
    }
}
