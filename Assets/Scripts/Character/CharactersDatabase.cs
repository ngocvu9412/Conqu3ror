using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersDatabase", menuName = "Characters/Characters database")]
public class CharactersDatabase : ScriptableObject
{
    public Character[] Characters;

    public int CharactersCount
    {
        get { return Characters.Length; }
    }

    public Character GetCharacter(int index)
    {
        return Characters[index];
    }
}

public static class CharacterDatabaseManager
{
    private static CharactersDatabase _charDB;

    // Truy cập CharactersDatabase thông qua class tĩnh
    public static CharactersDatabase GetDatabase()
    {
        if (_charDB == null)
        {
            _charDB = Resources.Load<CharactersDatabase>("Database/CharactersDatabase");
            if (_charDB == null)
            {
                Debug.LogError("CharactersDatabase instance not found in Resources.");
            }
        }
        return _charDB;
    }
}
