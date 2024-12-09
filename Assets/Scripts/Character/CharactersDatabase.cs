using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersDatabase", menuName = "Characters/Characters database")]
public class CharacterDatabase : ScriptableObject
{
    public Character[] Characters;

    public int CharactersCount{
        get { return Characters.Length; }
    }

    public Character GetCharacter (int index)
    {
        return Characters[index];
    }

    public void ChoseCharacter (int index)
    {
        Characters[index].isUsing = true;
    } 
}
