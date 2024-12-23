using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterChosingUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] Transform CharacterContainer;
    [SerializeField] GameObject CharacterPrefab;
    [Space(20)]
    [SerializeField] CharactersDatabase CharacterDB;
    [SerializeField] GameObject characterInfoUI;
    [SerializeField] GameObject characterInfoFull;
    [SerializeField] GameObject SkillLoadUI;

    void Start ()
    {
        GenerateCharactersMenuUI();
    }
    void GenerateCharactersMenuUI ()
    {
        //Xóa item template
        itemWidth = CharacterPrefab.GetComponent<RectTransform>().sizeDelta.x;

        characterInfoUI.GetComponent<CharacterInfoUI>().ShowCharacterInfoUI(GameDataManager.GetSelectedCharacterIndex());
        //Tạo items
        for (int i=0; i < CharacterDB.CharactersCount; i++)
        {
            Character character = CharacterDB.GetCharacter(i);
            CharacterUI uiCharacter = Instantiate (CharacterPrefab, CharacterContainer).GetComponent <CharacterUI> ();

            //Di chuyển item đến vị trí
            Debug.Log(uiCharacter.GetComponent<RectTransform>().anchoredPosition);
            uiCharacter.SetCharacterPos(Vector2.right*(itemWidth+itemSpace)*i);

            //Thêm tên cho item trong Hierachy
            uiCharacter.gameObject.name = "Character - " + character.name; 
            
            //Thêm thông tin
            if(character.Image == null) Debug.Log("Image Set Fail");
            uiCharacter.SetCharacterImage (character.Image);

            if (CheckIfCharacterUnlocked(i)){
                uiCharacter.OnCharacterChosing(i,OnCharacterChosing);
                uiCharacter.SetCharacterUnlockedStatus(true);
            }
            else {
                uiCharacter.OnCharacterChosing(i,OnCharacterChosing);
                uiCharacter.SetCharacterUnlockedStatus(false);
            }

            //Chỉnh kích cỡ item container
            CharacterContainer.GetComponent<RectTransform> ().sizeDelta = Vector2.right*(itemWidth+itemSpace)*CharacterDB.CharactersCount;
        }
    }
    void OnCharacterChosing (int index)
	{
        GameDataManager.SetSelectedCharacter(CharacterDB.GetCharacter(index),index);
        characterInfoUI.GetComponent<CharacterInfoUI>().ShowCharacterInfoUI(GameDataManager.GetSelectedCharacterIndex());
        characterInfoFull.GetComponent<CharacterInfoUI>().ShowCharacterInfoUI(GameDataManager.GetSelectedCharacterIndex());
        SkillLoadUI.GetComponent<SkillShowUI>().SetSkillInfo(GameDataManager.GetSelectedCharacterIndex());
	}
    void OnCharacterUnlocked(int index)
    {
        CharacterContainer.GetChild(index).GetComponent<CharacterUI>().SetCharacterUnlockedStatus(true);
        GameDataManager.AddUnlockedCharacter(index);
    }
    bool CheckIfCharacterUnlocked(int index)
    {
        return GameDataManager.GetPlayerListUnlockedChar().Contains(index);
    }
}
