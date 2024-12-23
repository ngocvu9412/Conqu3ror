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
    public Action<CharacterInCombat, ShapesManager> Execute { get; set; }

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
                Description = "Sát thương gây ra bằng 7 lần sát thương hiện tại đồng thời phá hủy 1 vùng 3x3 ngẫu nhiên trên bàn cờ",
                Icon = Resources.Load<Sprite>("Character/Phoenix/Phoenix_Skill_1"),
                EnergyCost = 100,
                DamageScale = 7f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 7f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Xác định vùng phá hủy 3x3 ngẫu nhiên
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int randomRow = UnityEngine.Random.Range(1, Constants.Rows-1);
                    int randomColumn = UnityEngine.Random.Range(1, Constants.Columns-1);

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

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                }
            },

            // Skill 2: Mưa thiên thạch
            new Skill
            {
                Name = "Mưa thiên thạch",
                Description = "Sát thương gây ra bằng 15 lần sát thương hiện tại đồng thời phá hủy 5 vùng 2x2 ngẫu nhiên trên bàn cờ",
                Icon = Resources.Load<Sprite>("Character/Phoenix/Phoenix_Skill_2"),
                EnergyCost = 200,
                DamageScale = 15f,
                Execute = (character, shapesManager) =>
                {
                    // Gây sát thương lên đối thủ
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 15f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    Debug.Log($"Skill 'Mưa thiên thạch' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage!");

                    // Xác định các vùng phá hủy
                    HashSet<Vector2Int> destroyedCenters = new HashSet<Vector2Int>();
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int destroyedCount = 0;

                    while (destroyedCount < 5)
                    {
                        int randomRow = UnityEngine.Random.Range(0, Constants.Rows - 1); // Giới hạn để tránh tràn vùng 2x2
                        int randomColumn = UnityEngine.Random.Range(0, Constants.Columns - 1);
                        Vector2Int center = new Vector2Int(randomRow, randomColumn);

                        // Kiểm tra xem vùng đã được xử lý chưa
                        bool overlap = false;
                        for (int row = randomRow; row < randomRow + 2; row++)
                        {
                            for (int col = randomColumn; col < randomColumn + 2; col++)
                            {
                                if (destroyedPositions.Contains(new Vector2Int(row, col)))
                                {
                                    overlap = true;
                                    break;
                                }
                            }
                            if (overlap) break;
                        }

                        // Nếu vùng chưa bị xử lý, thêm vào danh sách
                        if (!overlap)
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
                    // Bắt đầu xử lý logic phá hủy
                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                }
            },

            // Skill 3: Lốc xoáy lửa
            new Skill
            {
                Name = "Lốc xoáy lửa",
                Description = "Sát thương gây ra bằng 30 lần sát thương hiện tại đồng thời phá hủy 2 vùng 4x4 ngẫu nhiên trên bàn cờ ",
                Icon = Resources.Load<Sprite>("Character/Phoenix/Phoenix_Skill_3"),
                EnergyCost = 300,
                DamageScale = 30f,
                Execute = (character, shapesManager) =>
                {
                    // Gây sát thương lên đối thủ
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 30f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    Debug.Log($"Skill 'Lốc xoáy lửa' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage!");

                    // Xác định các vùng phá hủy
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();

                    // Vùng đầu tiên: Cột 0
                    int startRow1 = UnityEngine.Random.Range(0, 4); // Random từ hàng 0 đến 3
                    int startColumn1 = 0; // Cột cố định
                    for (int row = startRow1; row < startRow1 + 4; row++)
                    {
                        for (int col = startColumn1; col < startColumn1 + 4; col++)
                        {
                            if (row >= 0 && row < Constants.Rows && col >= 0 && col < Constants.Columns)
                            {
                                destroyedPositions.Add(new Vector2Int(row, col));
                            }
                        }
                    }

                    // Vùng thứ hai: Cột 4
                    int startRow2 = UnityEngine.Random.Range(0, 4); // Random từ hàng 0 đến 3
                    int startColumn2 = 4; // Cột cố định
                    for (int row = startRow2; row < startRow2 + 4; row++)
                    {
                        for (int col = startColumn2; col < startColumn2 + 4; col++)
                        {
                            if (row >= 0 && row < Constants.Rows && col >= 0 && col < Constants.Columns)
                            {
                                destroyedPositions.Add(new Vector2Int(row, col));
                            }
                        }
                    }
                    // Bắt đầu xử lý logic phá hủy
                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                }
            }
        };
    }
}

public static class MinervaSkills
{
    public static List<Skill> GetSkills()
    {
        return new List<Skill>
        {
            // Skill 1: Bão tuyết
            new Skill
            {
                Name = "Bão tuyết",
                Description = "Gây sát thương bằng 4, phá hủy ngẫu nhiên 4 ô trên bàn cờ, đồng thời triệt tiêu 100 năng lượng của kẻ thù.",
                Icon = Resources.Load<Sprite>("Character/Minerva/Minerva_Skill_1"),
                EnergyCost = 100,
                DamageScale = 4f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 4f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Triệt tiêu năng lượng của đối thủ
                    opponent.CurrentEnergy = Mathf.Max(0, opponent.CurrentEnergy - 100);
                    GameplayUIController.Ins.UpdateEnergy(!shapesManager.isMyTurn, character.CurrentEnergy, character.MaxEnergy);

                    // Phá hủy ngẫu nhiên 4 ô (không trùng lặp)
                    HashSet<Vector2Int> destroyedPositions = new HashSet<Vector2Int>();
                    while (destroyedPositions.Count < 4)
                    {
                        int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                        int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);
                        destroyedPositions.Add(new Vector2Int(randomRow, randomColumn));
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions.ToList()));

                    Debug.Log($"Skill 'Bão tuyết' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and draining 100 energy!");
                }
            },

            // Skill 2: Tuyết liên
            new Skill
            {
                Name = "Tuyết liên",
                Description = "Hồi lại 20% máu tối đa, đồng thời hấp thụ toàn bộ biểu tượng trái tim trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Minerva/Minerva_Skill_2"),
                EnergyCost = 200,
                DamageScale = 0f, // Không gây sát thương
                Execute = (character, shapesManager) =>
                {
                    // Hồi cố định 20% máu tối đa
                    float healAmount = character.MaxHealth * 0.2f;
                    character.CurrentHealth = Mathf.Min(character.MaxHealth, character.CurrentHealth + (int)healAmount);

                    // Hấp thụ toàn bộ biểu tượng trái tim
                    List<Vector2Int> heartPositions = new List<Vector2Int>();
                    for (int row = 0; row < Constants.Rows; row++)
                    {
                        for (int col = 0; col < Constants.Columns; col++)
                        {
                            var shape = shapesManager.shapes[row, col];
                            if (shape != null && shape.tag == "Heart")
                            {
                                heartPositions.Add(new Vector2Int(row, col));
                            }
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(heartPositions));

                    Debug.Log($"Skill 'Tuyết liên' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")}, healing {healAmount} HP and absorbing {heartPositions.Count()} Hearts!");
                }
            },

            // Skill 3: Hàn băng vĩnh cửu
            new Skill
            {
                Name = "Hàn băng vĩnh cửu",
                Description = "Gây sát thương bằng 27 lần sát thương hiện tại, phá hủy 6 biểu tượng ngẫu nhiên, phong ấn đổi thủ trong 2 lượt và triệt tiêu toàn bộ năng lượng của kẻ địch.",
                Icon = Resources.Load<Sprite>("Character/Minerva/Minerva_Skill_3"),
                EnergyCost = 300,
                DamageScale = 28f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 28f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);
                    // Triệt tiêu năng lượng của đối thủ
                    opponent.CurrentEnergy = 0;
                    GameplayUIController.Ins.UpdateEnergy(!shapesManager.isMyTurn, character.CurrentEnergy, character.MaxEnergy);

                    // Phá hủy 6 biểu tượng ngẫu nhiên (không trùng lặp)
                    HashSet<Vector2Int> destroyedPositions = new HashSet<Vector2Int>();
                    while (destroyedPositions.Count < 6)
                    {
                        int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                        int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);
                        destroyedPositions.Add(new Vector2Int(randomRow, randomColumn));
                    }

                    // Thêm 2 lượt cho bản thân
                    shapesManager.turnCount += 2;
                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions.ToList()));

                    Debug.Log($"Skill 'Hàn băng vĩnh cửu' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")}, causing {damage} damage, destroying 6 symbols, and gaining 2 extra turns!");
                    Debug.Log(shapesManager.turnCount + " turn remain");
                }
            }
        };
    }
}

public static class JasmineSkills
{
    public static List<Skill> GetSkills()
    {
        return new List<Skill>
        {
            // Skill 1: Truy sát
            new Skill
            {
                Name = "Truy sát",
                Description = "Gây sát thương bằng 10 lần sát thương hiện tại, phá hủy ngẫu nhiên 1 hàng ngang của bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Jasmine/Jasmine_Skill_1"),
                EnergyCost = 100,
                DamageScale = 10f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 10f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Phá hủy ngẫu nhiên 1 hàng ngang
                    int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    for (int col = 0; col < Constants.Columns; col++)
                    {
                        destroyedPositions.Add(new Vector2Int(randomRow, col));
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));

                    Debug.Log($"Skill 'Truy sát' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and destroying row {randomRow}!");
                }
            },

            // Skill 2: Phục kích
            new Skill
            {
                Name = "Phục kích",
                Description = "Gây sát thương bằng 20 lần sát thương hiện tại, phá hủy 2 đường chéo lớn của bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Jasmine/Jasmine_Skill_2"),
                EnergyCost = 200,
                DamageScale = 20f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 20f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Phá hủy 2 đường chéo lớn
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    for (int i = 0; i < Constants.Rows; i++)
                    {
                        if (i < Constants.Columns)
                        {
                            destroyedPositions.Add(new Vector2Int(i, i)); // Đường chéo từ trên trái xuống dưới phải
                            destroyedPositions.Add(new Vector2Int(i, Constants.Columns - 1 - i)); // Đường chéo từ trên phải xuống dưới trái
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));

                    Debug.Log($"Skill 'Phục kích' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and destroying both diagonals!");
                }
            },

            // Skill 3: Chí mạng
            new Skill
            {
                Name = "Chí mạng",
                Description = "Gây sát thương bằng 32 lần sát thương hiện tại, đồng thời hấp thụ toàn bộ biểu tượng thanh kiếm trên bàn cờ và hồi phục lượng máu tương đương.",
                Icon = Resources.Load<Sprite>("Character/Jasmine/Jasmine_Skill_3"),
                EnergyCost = 300,
                DamageScale = 32f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 36f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Hấp thụ toàn bộ biểu tượng thanh kiếm trên bàn cờ
                    List<Vector2Int> swordPositions = new List<Vector2Int>();
                    foreach (var row in Enumerable.Range(0, Constants.Rows))
                    {
                        foreach (var col in Enumerable.Range(0, Constants.Columns))
                        {
                            var shape = shapesManager.shapes[row, col];
                            if (shape != null && shape.tag == "Sword")
                            {
                                swordPositions.Add(new Vector2Int(row, col));
                            }
                        }
                    }
                    Debug.Log(swordPositions.Count());
                    // Hồi máu dựa trên số lượng kiếm
                    float healAmount = swordPositions.Count() * character.MaxHealth * 0.02f;
                    character.CurrentHealth = Mathf.Min(character.MaxHealth, character.CurrentHealth + (int)healAmount);

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(swordPositions));

                    Debug.Log($"Skill 'Chí mạng' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage, absorbing {swordPositions.Count()} Swords, and healing {healAmount} HP!");
                }
            }
        };
    }
}
public static class MariusSkills
{
    public static List<Skill> GetSkills()
    {
        return new List<Skill>
        {
            // Skill 1: Lôi lực
            new Skill
            {
                Name = "Lôi lực",
                Description = "Gây sát thương bằng 6 lần sát thương hiện tại, đồng thời phá hủy ngẫu nhiên 5 ~ 8 biểu tượng giống nhau trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Marius/Marius_Skill_1"),
                EnergyCost = 100,
                DamageScale = 6f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 6f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Chọn một loại biểu tượng ngẫu nhiên từ các biểu tượng hiện tại trên bàn cờ
                    List<string> symbolTypes = new List<string>();
                    for (int row = 0; row < Constants.Rows; row++)
                    {
                        for (int col = 0; col < Constants.Columns; col++)
                        {
                            var shape = shapesManager.shapes[row, col];
                            if (shape != null)
                            {
                                string type = shape.GetComponent<Shape>().Type;
                                if (!symbolTypes.Contains(type))
                                {
                                    symbolTypes.Add(type);
                                }
                            }
                        }
                    }

                    if (symbolTypes.Count == 0)
                    {
                        Debug.LogWarning("No symbols found on the board!");
                        return;
                    }

                    string randomType = symbolTypes[UnityEngine.Random.Range(0, symbolTypes.Count)];

                    // Phá hủy ngẫu nhiên 5 ~ 8 biểu tượng cùng loại
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int destroyedCount = UnityEngine.Random.Range(5, 9);

                    for (int row = 0; row < Constants.Rows; row++)
                    {
                        for (int col = 0; col < Constants.Columns; col++)
                        {
                            var shape = shapesManager.shapes[row, col];
                            if (shape != null && shape.GetComponent<Shape>().Type == randomType)
                            {
                                destroyedPositions.Add(new Vector2Int(row, col));
                                if (destroyedPositions.Count >= destroyedCount) break;
                            }
                        }
                        if (destroyedPositions.Count >= destroyedCount) break;
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                    Debug.Log($"Skill 'Lôi lực' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and destroying {destroyedCount} {randomType} symbols!");
                }
            },

            // Skill 2: Giông tố
            new Skill
            {
                Name = "Giông tố",
                Description = "Gây sát thương bằng 22 lần sát thương hiện tại, phá hủy ngẫu nhiên 2 đường thẳng và ngang trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Marius/Marius_Skill_2"),
                EnergyCost = 200,
                DamageScale = 22f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 22f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Phá hủy 2 đường thẳng và ngang
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    int randomRow = UnityEngine.Random.Range(0, Constants.Rows);
                    int randomColumn = UnityEngine.Random.Range(0, Constants.Columns);

                    // Phá hủy hàng ngang
                    for (int col = 0; col < Constants.Columns; col++)
                    {
                        var shape = shapesManager.shapes[randomRow, col];
                        if (shape != null)
                        {
                            destroyedPositions.Add(new Vector2Int(randomRow, col));
                        }
                    }

                    // Phá hủy cột dọc
                    for (int row = 0; row < Constants.Rows; row++)
                    {
                        var shape = shapesManager.shapes[row, randomColumn];
                        if (shape != null)
                        {
                            destroyedPositions.Add(new Vector2Int(row, randomColumn));
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                    Debug.Log($"Skill 'Giông tố' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and destroying row {randomRow} and column {randomColumn}!");
                }
            },

            // Skill 3: Cuồng nộ
            new Skill
            {
                Name = "Cuồng nộ",
                Description = "Gây sát thương bằng 28 lần sát thương hiện tại, đồng thời phá hủy 32 ô xen kẽ nhau trên bàn cờ.",
                Icon = Resources.Load<Sprite>("Character/Marius/Marius_Skill_3"),
                EnergyCost = 300,
                DamageScale = 28f,
                Execute = (character, shapesManager) =>
                {
                    var opponent = shapesManager.isMyTurn ? shapesManager.enemyCharacter : shapesManager.playerCharacter;
                    int damage = Mathf.RoundToInt(character.CurrentAttack * 28f);
                    opponent.CurrentHealth = Mathf.Max(0, opponent.CurrentHealth - damage);

                    // Phá hủy 32 ô xen kẽ nhau
                    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
                    for (int row = 0; row < Constants.Rows; row++)
                    {
                        for (int col = 0; col < Constants.Columns; col++)
                        {
                            if ((row + col) % 2 == 0 && destroyedPositions.Count < 32) // Xen kẽ
                            {
                                var shape = shapesManager.shapes[row, col];
                                if (shape != null)
                                {
                                    destroyedPositions.Add(new Vector2Int(row, col));
                                }
                            }
                        }
                    }

                    shapesManager.StartCoroutine(shapesManager.ExecuteSkillLogic(destroyedPositions));
                    Debug.Log($"Skill 'Cuồng nộ' used by {(shapesManager.isMyTurn ? "Player" : "Enemy")} causing {damage} damage and destroying 32 alternating cells!");
                }
            }
        };
    }
}

