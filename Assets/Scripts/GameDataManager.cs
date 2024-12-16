using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable] public class PlayerData
{
    public int coins = 0;
    public List<int> UnlockedCharactersIndexes =new List<int> ();
    public int selectedCharacterIndex = 0;
    public List<int> ItemList = new List<int>();
    public List<Equipment> PlayerEquipments = new List<Equipment>();
}
public class GameDataManager : MonoBehaviour
{
    static PlayerData playerData = new PlayerData();
   	static Character selectedCharacter;

    static GameDataManager ()
	{
		LoadPlayerData ();
	}

	//Player Data Methods -----------------------------------------------------------------------------
	public static Character GetSelectedCharacter ()
	{
		return selectedCharacter;
	}

	public static void SetSelectedCharacter (Character character, int index)
	{
		selectedCharacter = character;
		playerData.selectedCharacterIndex = index;
		SavePlayerData ();
	}

	public static int GetSelectedCharacterIndex ()
	{
		return playerData.selectedCharacterIndex;
	}
    //Coins Funcs
	public static int GetCoins ()
	{
		return playerData.coins;
	}

	public static void AddCoins (int amount)
	{
		playerData.coins += amount;
		SavePlayerData ();
	}
	public static bool CanSpendCoins (int amount)
	{
		return (playerData.coins >= amount);
	}

	public static void SpendCoins (int amount)
	{
		playerData.coins -= amount;
		SavePlayerData ();
	}
    // Equipment Funcs
    public static void AddEquipment (Equipment equipment)
    {
        Equipment equip = equipment;
        if( playerData.PlayerEquipments.Count == 0)
        {
            equip.SetEquipmentID(0);
        }
        else equip.SetEquipmentID(playerData.PlayerEquipments.Count);
        playerData.PlayerEquipments.Add (equip);
        SavePlayerData ();
    }
    public static void RemoveEquipment (int ID)
    {
        playerData.PlayerEquipments.Remove (playerData.PlayerEquipments[ID]);
        SavePlayerData ();
    }
    // Characters List Data
    public static void AddUnlockedCharacter (int characterIndex)
	{
		playerData.UnlockedCharactersIndexes.Add (characterIndex);
		SavePlayerData ();
	}
    public static void AddUnlockedCharacter (Equipment equipment)
	{
		playerData.PlayerEquipments.Add (equipment);
		SavePlayerData ();
	}
    // Load Save Data
	static void LoadPlayerData ()
	{
		playerData = BinarySerializer.Load<PlayerData> ("player-data.txt");
		UnityEngine.Debug.Log ("<color=green>[PlayerData] Loaded.</color>");
	}

	static void SavePlayerData ()
	{
		BinarySerializer.Save (playerData, "player-data.txt");
		UnityEngine.Debug.Log ("<color=magenta>[PlayerData] Saved.</color>");
	}
}
