using UnityEngine;

[System.Serializable]
public class Skill
{
    public string Name;                    // Tên kỹ năng
    public string Description;             // Mô tả kỹ năng
    public Sprite Icon;                    // Hình ảnh kỹ năng
    public int EnergyCost;                 // Năng lượng tiêu tốn
    public int Damage;         // Hệ số sát thương dựa trên sát thương cơ bản
    public System.Action<CharacterInCombat, ShapesManager> Execute; // Logic kỹ năng
}

