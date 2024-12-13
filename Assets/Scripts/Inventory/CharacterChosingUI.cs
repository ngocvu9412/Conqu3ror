using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterChosingUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] GameObject CharacterMenuUI;
    [SerializeField] Transform CharacterContainer;
    [SerializeField] GameObject CharacterPrefab;
    [Space(20)]
    [SerializeField] CharactersDatabase CharacterDB;
    void Start ()
    {
        GenerateCharactersMenuUI();
    }
    void GenerateCharactersMenuUI ()
    {
        //Xóa item template
        itemWidth = CharacterContainer.GetChild (0).GetComponent<RectTransform>().sizeDelta.x;
        Destroy(CharacterContainer.GetChild (0).gameObject);
        CharacterContainer.DetachChildren();

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

            if (character.Unlocked){
                uiCharacter.SetCharacterUnlockedStatus(true);
            }
            else {
                uiCharacter.SetCharacterUnlockedStatus(false);
            }

            //Chỉnh kích cỡ item container
            CharacterContainer.GetComponent<RectTransform> ().sizeDelta = Vector2.right*(itemWidth+itemSpace)*CharacterDB.CharactersCount;
        }
    }
    // void OnCharacterChosing (int index)
	// {
	// }
}
