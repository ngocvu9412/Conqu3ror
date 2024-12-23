using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] Image CharacterImage;
    [SerializeField] Button CharacterChosingButton;
    public void SetCharacterPos (Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    
    public void SetCharacterImage (Sprite sprite)
    {
        CharacterImage.sprite = sprite;
    }
    public void SetCharacterUnlockedStatus (bool unlocked)
    {
        CharacterChosingButton.interactable = unlocked;
    }
    public void OnCharacterChosing(int index, UnityAction< int> action)
    {
        CharacterChosingButton.onClick.RemoveAllListeners();
        CharacterChosingButton.onClick.AddListener(() => action.Invoke(index));
    }
}
