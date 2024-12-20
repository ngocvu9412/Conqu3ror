using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Icon { get; set; }
    public int EnergyCost { get; set; }
    public float DamageScale { get; set; }
    public Action<CharacterInCombat, ShapesManager, Dictionary<string, float>> Execute { get; set; }

}

public static class PhoenixSkills
{
    public static List<Skill> GetSkills()
    {
        return new List<Skill>
        {
            // Skill 1: Quả cầu lửa
            new Skill
            {
                Name = "Quả cầu lửa",
                Description = "Gây sát thương 8 * CurrentAttack, đồng thời phá hủy 1 vùng 3x3 ngẫu nhiên trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Icons/Fireball"),
                EnergyCost = 100,
                DamageScale = 8f,
                Execute = (character, shapesManager, destroyedSymbols) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 8f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Xác định vùng phá hủy 3x3 ngẫu nhiên
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                    int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);

                    for (int row = randomRow - 1; row <= randomRow + 1; row++)
                    {
                        for (int col = randomColumn - 1; col <= randomColumn + 1; col++)
                        {
                            if (row >= 0 && row < Constants.Rows && col >= 0 && col < Constants.Columns)
                            {
                                destroyedPositions.Add(new Vector2Int(row, col));
                            }
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions, destroyedSymbols));
                }
            },

            // Skill 2: Mưa thiên thạch
            new Skill
            {
                Name = "Mưa thiên thạch",
                Description = "Gây sát thương 15 * CurrentAttack, đồng thời phá hủy 5 vùng 2x2 ngẫu nhiên trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Icons/MeteorShower"),
                EnergyCost = 200,
                DamageScale = 15f,
                Execute = (character, shapesManager, destroyedSymbols) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 15f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    HashSet<Vector2Int> destroyedCenters = new HashSet<Vector2Int>();
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int destroyedCount = 0;

                    // Phá hủy 5 vùng 2x2 ngẫu nhiên
                    while (destroyedCount < 5)
                    {
                        int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                        int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);
                        Vector2Int center = new Vector2Int(randomRow, randomColumn);

                        if (!destroyedCenters.Contains(center))
                        {
                            destroyedCenters.Add(center);
                            for (int row = randomRow; row < randomRow + 2; row++)
                            {
                                for (int col = randomColumn; col < randomColumn + 2; col++)
                                {
                                    if (row >= 0 && row < Constants.Rows && col >= 0 && col < Constants.Columns)
                                    {
                                        destroyedPositions.Add(new Vector2Int(row, col));
                                    }
                                }
                            }
                            destroyedCount++;
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions, destroyedSymbols));
                }
            },

            // Skill 3: Lốc xoáy lửa
            new Skill
            {
                Name = "Lốc xoáy lửa",
                Description = "Gây sát thương 30 * CurrentAttack, đồng thời phá hủy 2 vùng 4x4 ngẫu nhiên trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Icons/FireTornado"),
                EnergyCost = 300,
                DamageScale = 30f,
                Execute = (character, shapesManager, destroyedSymbols) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 30f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    HashSet<Vector2Int> destroyedCenters = new HashSet<Vector2Int>();
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int destroyedCount = 0;

                    // Phá hủy 2 vùng 4x4 ngẫu nhiên
                    while (destroyedCount < 2)
                    {
                        int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                        int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);
                        Vector2Int center = new Vector2Int(randomRow, randomColumn);

                        if (!destroyedCenters.Contains(center))
                        {
                            destroyedCenters.Add(center);
                            for (int row = randomRow; row < randomRow + 4; row++)
                            {
                                for (int col = randomColumn; col < randomColumn + 4; col++)
                                {
                                    if (row >= 0 && row < Constants.Rows && col >= 0 && col < Constants.Columns)
                                    {
                                        destroyedPositions.Add(new Vector2Int(row, col));
                                    }
                                }
                            }
                            destroyedCount++;
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions, destroyedSymbols));
                }
            }
        };
    }
}
