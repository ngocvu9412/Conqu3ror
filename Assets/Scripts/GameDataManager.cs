using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable] public class PlayerData
{
    public List<int> UnlockedCharactersIndexes =new List<int> ();
    public int selectedCharacterIndex = 0;
    public List<Tuple<int, int>> ItemList = new List<Tuple<int, int>>();
    public List<Equipment> PlayerEquipments = new List<Equipment>();
}
public class GameDataManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
