using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Image SkillImage;
    [SerializeField] TMP_Text SkillName;
    [SerializeField] TMP_Text SkillDes;
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
