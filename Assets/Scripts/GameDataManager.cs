using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int coins = 20000;
    public int HealthEnergy = 5;
    public int currentHealthEnergy = 5;
    public List<int> UnlockedCharactersIndexes = new List<int> { 0, 1 };
    public int selectedCharacterIndex = 0;
    public List<Equipment> PlayerEquipments = new List<Equipment>();
    public Equipment[] UsingEquipments = new Equipment[4];
}

public class GameDataManager : Singleton<GameDataManager>
{
    private static PlayerData playerData = new PlayerData();
    private static Character selectedCharacter;
    public static PointInfo EnemyInfo;

    // Được gọi khi GameDataManager được khởi tạo lần đầu
    public override void Awake()
    {
        base.Awake(); // Bảo đảm Singleton hoạt động đúng
        LoadPlayerData(); // Tải dữ liệu ngay khi khởi tạo
		selectedCharacter = CharacterDatabaseManager.GetDatabase().GetCharacter(playerData.selectedCharacterIndex);
	}
    //PointData
    public void SetPointData(PointInfo CurrentPointData)
    {
        EnemyInfo = CurrentPointData;
    }
    // Player Data Methods
    public  void AddUnlockedCharacter(int characterIndex)
    {
        playerData.UnlockedCharactersIndexes.Add(characterIndex);
        SavePlayerData();
    }

    public  Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public  void SetSelectedCharacter(Character character, int index)
    {
        selectedCharacter = character;
        playerData.selectedCharacterIndex = index;
        SavePlayerData();
    }

    public void LevelUp(int expEarn)
    {
        int total = expEarn + selectedCharacter.Exp;
        while (total >= selectedCharacter.MaxExp)
        {
            total -= selectedCharacter.MaxExp;
            selectedCharacter.Level++;
            selectedCharacter.health += Constants.healthPerLevel;
            selectedCharacter.attack += Constants.atkPerLevel;
            selectedCharacter.MaxExp += Constants.expPerLevel;
        }
        selectedCharacter.Exp = total;
        Debug.Log("CharLv: "+ selectedCharacter.Level);
        SavePlayerData();
    }

    public  int GetSelectedCharacterIndex()
    {
        return playerData.selectedCharacterIndex;
    }

    public  List<int> GetPlayerListUnlockedChar()
    {
        return playerData.UnlockedCharactersIndexes;
    }

    // Health Funcs
    public  int GetMaxHealthEner()
    {
        return playerData.HealthEnergy;
    }

    public  int GetCurrentHealthEner()
    {
        return playerData.currentHealthEnergy;
    }

    public  void SetCurrentHealthEner(int healthEnergy)
    {
        playerData.currentHealthEnergy = healthEnergy;
		SavePlayerData();
    }

    // Coins Funcs
    public  int GetCoins()
    {
        return playerData.coins;
    }

    public  void AddCoins(int amount)
    {
        playerData.coins += amount;
        SavePlayerData();
    }

    public  bool CanSpendCoins(int amount)
    {
        return playerData.coins >= amount;
    }

    public  void SpendCoins(int amount)
    {
        playerData.coins -= amount;
        SavePlayerData();
    }

    // Equipment Funcs
    public  void AddEquipment(Equipment equipment)
    {
        Equipment equip = equipment;
        playerData.PlayerEquipments.Add(equip);
        SortPlayerEquipments();
        SavePlayerData();
    }

    public  void AddEquipmentToSlot(int slot_Index, int equipment_Index)
    {
        Equipment tempEquip = playerData.PlayerEquipments[equipment_Index];
        tempEquip.IsUsing = true;
        playerData.PlayerEquipments[equipment_Index] = tempEquip;
        playerData.UsingEquipments[slot_Index] = playerData.PlayerEquipments[equipment_Index];
        SavePlayerData();
    }

    public  List<Equipment> GetPlayerEquipments()
    {
        return playerData.PlayerEquipments;
    }

    public  Equipment[] GetPlayerUsedEquips()
    {
        return playerData.UsingEquipments;
    }

    public  bool CanUseEquipment(int slot_Index, int equipment_Index)
    {
        if (playerData.UsingEquipments[slot_Index].Equals(default(Equipment)))
        {
            var equipmentToCheck = playerData.PlayerEquipments[equipment_Index];
            string equipmentName = equipmentToCheck.name;

            bool isDuplicateInUse = playerData.PlayerEquipments
                .Any(e => e.name == equipmentName && e.IsUsing);
            return !isDuplicateInUse;
        }
        else return false;
    }

    public  int IsEquipmentUsed(Equipment equipment)
    {
        int index = 0;
        foreach (var equip in playerData.UsingEquipments)
        {
            if (equipment.Equals(equip))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    public  void RemoveEquipmentUsed(int EquipmentIndex)
    {
        int EquipmentSlot = IsEquipmentUsed(playerData.PlayerEquipments[EquipmentIndex]);
        if (EquipmentSlot != -1)
        {
            playerData.UsingEquipments[EquipmentSlot] = default(Equipment);
            Equipment tempEquip = playerData.PlayerEquipments[EquipmentIndex];
            tempEquip.IsUsing = false;
            playerData.PlayerEquipments[EquipmentIndex] = tempEquip;
        }
        SavePlayerData();
    }

    public  void MergeEquipment(int index1, int index2)
    {
        if (index1 < 0 || index2 < 0 || index1 >= playerData.PlayerEquipments.Count || index2 >= playerData.PlayerEquipments.Count)
        {
            Debug.Log("Invalid equipment indices");
            return;
        }

        Equipment equip1 = playerData.PlayerEquipments[index1];
        Equipment equip2 = playerData.PlayerEquipments[index2];

        if (equip1.Level == equip2.Level && equip1.name == equip2.name)
        {
            Equipment newEquip = new Equipment
            {
                SpriteName = equip1.SpriteName,
                name = equip1.name,
                description = equip1.description,
                price = equip1.price + equip2.price,
                Level = equip1.Level + 1
            };

            playerData.PlayerEquipments.RemoveAt(index1);
            playerData.PlayerEquipments.RemoveAt(index2 > index1 ? index2 - 1 : index2);

            playerData.PlayerEquipments.Add(newEquip);

            Debug.Log($"Merged {equip1.name} and {equip2.name} into {newEquip.name}");
        }
        else
        {
            Debug.Log("Equipments must have the same level to merge");
        }
        SortPlayerEquipments();
        SavePlayerData();
    }

    public  void SortPlayerEquipments()
    {
        List<string> nameOrder = new List<string>
        {
            "Trái tim hoàng gia",
            "Gươm ánh sáng",
            "Quả cầu phù thủy",
            "Nhẫn phù thủy"
        };

        Dictionary<string, int> namePriority = new Dictionary<string, int>();
        for (int i = 0; i < nameOrder.Count; i++)
        {
            namePriority[nameOrder[i]] = i;
        }

        playerData.PlayerEquipments.Sort((equip1, equip2) =>
        {
            int nameComparison = namePriority[equip1.name].CompareTo(namePriority[equip2.name]);
            if (nameComparison != 0)
            {
                return nameComparison;
            }

            int levelComparison = equip2.Level.CompareTo(equip1.Level);
            if (levelComparison != 0)
            {
                return levelComparison;
            }

            bool equip1Using = equip1.IsUsing;
            bool equip2Using = equip2.IsUsing;
            if (equip1Using != equip2Using)
            {
                return equip2Using.CompareTo(equip1Using);
            }

            return 0;
        });

        Debug.Log("PlayerEquipments sorted successfully by name order, descending level, and IsUsing status.");
    }

    public  List<(Equipment, int)> GetHighestLevelEquipments()
    {
        var highestLevelEquipmentsWithIndices = playerData.PlayerEquipments
            .GroupBy(equipment => equipment.name)
            .Select(group =>
            {
                var highestLevelEquipment = group.OrderByDescending(e => e.Level).First();
                int index = playerData.PlayerEquipments.FindIndex(e =>
                    e.name == highestLevelEquipment.name && e.Level == highestLevelEquipment.Level);
                return (highestLevelEquipment, index);
            })
            .ToList();

        return highestLevelEquipmentsWithIndices;
    }

    static void LoadPlayerData()
    {
        try
        {
            playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
            Debug.Log("<color=green>[PlayerData] Loaded.</color>");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading player data: {ex.Message}");
        }
    }

    static void SavePlayerData()
    {
        try
        {
            if (playerData == null)
            {
                Debug.LogError("Player data is null. Cannot save.");
                return;
            }

            BinarySerializer.Save(playerData, "player-data.txt");
            Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving player data: {ex.Message}");
        }
    }
}
