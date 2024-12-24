using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable] public class PlayerData
{
    public int coins = 20000;
	public int HealthEnergy;
	public int currentHealthEnergy;
    public List<int> UnlockedCharactersIndexes =new List<int> {0,1};
    public int selectedCharacterIndex = 0;
    public List<Equipment> PlayerEquipments = new List<Equipment>();
	public Equipment[] UsingEquipments = new Equipment[4];
}
public class GameDataManager : MonoBehaviour
{
    static PlayerData playerData = new PlayerData();
   	static Character selectedCharacter;
    static GameDataManager ()
	{
		LoadPlayerData ();
	}
	//Player Data Methods 
	//Character
	public static void AddUnlockedCharacter (int characterIndex)
	{
		playerData.UnlockedCharactersIndexes.Add (characterIndex);
		SavePlayerData ();
	}
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
	public static List<int> GetPlayerListUnlockedChar()
	{
		return playerData.UnlockedCharactersIndexes;
	}
	//Health Funcs
	public static int GetMaxHealthEner()
	{
		return playerData.HealthEnergy;
	}
	public static int GetCurrentHealthEner()
	{
		return playerData.currentHealthEnergy;
	}
	public static void SetCurrentHealthEner(int healthEnergy)
	{
		playerData.currentHealthEnergy = healthEnergy;
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
        playerData.PlayerEquipments.Add (equip);
		SortPlayerEquipments();
        SavePlayerData ();
    }
	public static void AddEquipmentToSlot (int slot_Index,int equipment_Index)
	{
		Equipment tempEquip = playerData.PlayerEquipments[equipment_Index];
		tempEquip.IsUsing = true;
		playerData.PlayerEquipments[equipment_Index] = tempEquip;
		playerData.UsingEquipments[slot_Index] = playerData.PlayerEquipments[equipment_Index];
		SavePlayerData();
	}
	public static List<Equipment> GetPlayerEquipments()
	{
		return playerData.PlayerEquipments;
	}
	public static Equipment[] GetPlayerUsedEquips()
	{
		return playerData.UsingEquipments;
	}
	public static bool CanUseEquipment(int slot_Index, int equipment_Index)
	{
		if( playerData.UsingEquipments[slot_Index].Equals(default(Equipment)))
		{
	    	var equipmentToCheck = playerData.PlayerEquipments[equipment_Index];
	    	string equipmentName = equipmentToCheck.name;

	    	bool isDuplicateInUse = playerData.PlayerEquipments
	    	    .Any(e => e.name == equipmentName && e.IsUsing);
		    return !isDuplicateInUse;
		}
		else return false;
	}
	public static int IsEquipmentUsed(Equipment equipment)
	{
	    int index = 0; // Biến để theo dõi vị trí

	    foreach (var equip in playerData.UsingEquipments)
	    {
	        if (equipment.Equals(equip))
	        {
	            return index; // Trả về vị trí nếu tìm thấy
	        }
	        index++; // Tăng chỉ số
	    }

	    return -1; // Trả về -1 nếu không tìm thấy
	}
	public static void RemoveEquipmentUsed(int EquipmentIndex)
	{
		int EquipmentSlot=IsEquipmentUsed(playerData.PlayerEquipments[EquipmentIndex]);
		if(EquipmentSlot != -1)
		{
			playerData.UsingEquipments[EquipmentSlot] = default(Equipment);
			Equipment tempEquip = playerData.PlayerEquipments[EquipmentIndex];
			tempEquip.IsUsing = false;
			playerData.PlayerEquipments[EquipmentIndex] = tempEquip;

		}
		SavePlayerData();
	}

	public static void MergeEquipment(int index1, int index2)
    {
        // Kiểm tra chỉ số hợp lệ
        if (index1 < 0 || index2 < 0 || index1 >= playerData.PlayerEquipments.Count || index2 >= playerData.PlayerEquipments.Count)
        {
            Debug.Log("Invalid equipment indices");
            return;
        }

        // Lấy 2 trang bị cần ghép
        Equipment equip1 = playerData.PlayerEquipments[index1];
        Equipment equip2 = playerData.PlayerEquipments[index2];

        // Kiểm tra xem chúng có cùng cấp không
        if (equip1.Level == equip2.Level && equip1.name == equip2.name)
        {
            // Tạo trang bị mới với cấp độ tăng thêm 1
            Equipment newEquip = new Equipment
            {
                SpriteName = equip1.SpriteName, // Hoặc thay đổi hình ảnh mới
                name = equip1.name, // Đặt tên mới
                description = equip1.description,
                price = equip1.price + equip2.price, // Tăng giá trị
                Level = equip1.Level + 1
            };

            // Xóa 2 trang bị cũ khỏi danh sách
            playerData.PlayerEquipments.RemoveAt(index1);
            playerData.PlayerEquipments.RemoveAt(index2 > index1 ? index2 - 1 : index2); // Điều chỉnh chỉ số sau khi xóa

            // Thêm trang bị mới vào danh sách
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
	public static void SortPlayerEquipments()
{
    // Định nghĩa thứ tự tên cụ thể
    List<string> nameOrder = new List<string>
    {
        "Trái tim hoàng gia",
        "Gươm ánh sáng",
        "Quả cầu phù thủy",
        "Nhẫn phù thủy"
    };

    // Tạo từ điển ánh xạ tên với thứ tự ưu tiên
    Dictionary<string, int> namePriority = new Dictionary<string, int>();
    for (int i = 0; i < nameOrder.Count; i++)
    {
        namePriority[nameOrder[i]] = i; // Gán chỉ số thứ tự
    }

    // Sắp xếp danh sách
    playerData.PlayerEquipments.Sort((equip1, equip2) =>
    {
        // So sánh theo thứ tự tên
        int nameComparison = namePriority[equip1.name].CompareTo(namePriority[equip2.name]);
        if (nameComparison != 0)
        {
            return nameComparison;
        }

        // Nếu cùng thứ tự tên, sắp xếp theo cấp độ giảm dần
        int levelComparison = equip2.Level.CompareTo(equip1.Level);
        if (levelComparison != 0)
        {
            return levelComparison;
        }

        // Nếu cùng loại và cùng cấp độ, ưu tiên IsUsing
        bool equip1Using = equip1.IsUsing;
        bool equip2Using = equip2.IsUsing;
        if (equip1Using != equip2Using)
        {
            return equip2Using.CompareTo(equip1Using); // true > false
        }

        // Không có sự khác biệt, giữ nguyên thứ tự
        return 0;
    });

    Debug.Log("PlayerEquipments sorted successfully by name order, descending level, and IsUsing status.");
}


	public static List<(Equipment, int)> GetHighestLevelEquipments()
	{
	    // Nhóm theo name và chọn phần tử có level cao nhất
	    var highestLevelEquipmentsWithIndices = playerData.PlayerEquipments
	        .GroupBy(equipment => equipment.name) // Nhóm theo `name`
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

    // Load Save Data
	static void LoadPlayerData()
	{
	    try
	    {
	        playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
	        UnityEngine.Debug.Log("<color=green>[PlayerData] Loaded.</color>");
	    }
	    catch (System.Exception ex)
	    {
	        UnityEngine.Debug.LogError($"Error loading player data: {ex.Message}");
	    }
	}

	static void SavePlayerData()
	{
	    try
	    {
	        // Kiểm tra dữ liệu trước khi lưu
	        if (playerData == null)
	        {
	            Debug.LogError("Player data is null. Cannot save.");
	            return;
	        }

	        // Sử dụng BinarySerializer để lưu dữ liệu
	        BinarySerializer.Save(playerData, "player-data.txt");
	        UnityEngine.Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
	    }
	    catch (System.Exception ex)
	    {
	        Debug.LogError($"Error saving player data: {ex.Message}");
	    }
	}

}
