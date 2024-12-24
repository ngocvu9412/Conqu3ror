using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Singleton<AIController>
{
    public CharacterInCombat character; // AI character info
    private ShapesManager shapesManager; // Board manager
    public DifficultyLevel aiDifficulty; // AI difficulty level
    private bool isExecutingAction = false; // Đảm bảo AI chỉ thực hiện một hành động tại một thời điểm

    // AI difficulty levels
    public enum DifficultyLevel
    {
        Easy,   // Easy
        Medium, // Medium
        Hard    // Hard
    }

    public override void Start()
    {
        shapesManager = ShapesManager.Ins; // Get ShapesManager instance
    }

    public void ExecuteAIMove(string difficulty)
    {
        // Step 1: Attempt to use a skill if appropriate
        if (isExecutingAction)
        {
            Debug.Log("AI is already performing an action.");
            return; // Không làm gì nếu AI đang bận
        }

        isExecutingAction = true; // Đặt cờ bắt đầu hành động

        if (TryUseSkill(difficulty))
        {
            return; // Nếu đã sử dụng skill, không thực hiện nước đi
        }

        // Step 2: Perform the best move based on difficulty
        var bestMove = GetBestMoveByDifficulty(difficulty);

        if (bestMove.HasValue)
        {
            StartCoroutine(PerformAIMove(bestMove.Value.Item1, bestMove.Value.Item2));
        }
        else
        {
            Debug.Log("AI could not find a valid move.");
            shapesManager.state = GameState.None;
        }
    }


    public (GameObject, GameObject)? GetBestMoveByDifficulty(string difficulty)
    {
        switch (difficulty.ToLower())
        {
            case "easy":
                return GetEasyMove();
            case "medium":
                return GetMediumMove();
            case "hard":
                return GetHardMove();
            default:
                Debug.LogError("Invalid AI difficulty level: " + difficulty);
                return null;
        }
    }

    private bool TryUseSkill(string difficulty)
    {
        Debug.Log("AI is attempting to use a skill...");

        // Sắp xếp skill theo năng lượng từ cao xuống thấp
        character.Skills.Sort((a, b) => b.EnergyCost.CompareTo(a.EnergyCost));

        foreach (var skill in character.Skills)
        {
            if (character.CurrentEnergy >= skill.EnergyCost)
            {
                // Easy: Randomly decide whether to use a skill
                if (difficulty == "easy" && Random.value > 0.5f)
                {
                    StartCoroutine(ActivateSkill(skill));
                    return true; // Skill được sử dụng
                }

                // Medium/Hard: Use skill if it's the last turn
                if (difficulty != "easy" && shapesManager.turnCount == 1)
                {
                    StartCoroutine(ActivateSkill(skill));
                    return true; // Skill được sử dụng
                }
            }
        }

        return false; // Không sử dụng skill
    }


    private IEnumerator ActivateSkill(Skill skill)
    {
        yield return new WaitForSeconds(1f); // Chờ trước khi sử dụng skill
        Debug.Log("AI uses skill: " + skill.Name);
        character.CurrentEnergy -= skill.EnergyCost;

        // Cập nhật năng lượng trên UI
        if (GameplayUIController.Ins)
        {
            GameplayUIController.Ins.UpdateEnergy(false, character.CurrentEnergy, character.MaxEnergy);
        }

        skill.Execute(character, shapesManager);

        //EndAIActions();
    }


    private (GameObject, GameObject)? GetEasyMove()
    {
        var symbolPriority = GetSymbolPriorityByDifficulty();

        // Calculate all valid moves
        var allMoves = Utilities.CalculateMovesAndScores(shapesManager.shapes, symbolPriority);

        // Randomly choose a move
        if (allMoves.Count > 0)
        {
            var randomMove = allMoves[Random.Range(0, allMoves.Count)];
            return (randomMove.Item1, randomMove.Item2);
        }

        return null;
    }

    private (GameObject, GameObject)? GetMediumMove()
    {
        var symbolPriority = GetSymbolPriorityByDifficulty();

        // Calculate match-4 moves first
        var match4Moves = Utilities.CalculateMovesAndScoresForMatch4(shapesManager.shapes, symbolPriority);

        if (match4Moves.Count > 0)
        {
            var randomMatch4 = match4Moves[Random.Range(0, match4Moves.Count)];
            return (randomMatch4.Item1, randomMatch4.Item2);
        }

        // If no match-4, choose a random match-3 move
        var regularMoves = Utilities.CalculateMovesAndScores(shapesManager.shapes, symbolPriority);
        if (regularMoves.Count > 0)
        {
            var randomMove = regularMoves[Random.Range(0, regularMoves.Count)];
            return (randomMove.Item1, randomMove.Item2);
        }

        return null;
    }

    private (GameObject, GameObject)? GetHardMove()
    {
        var symbolPriority = GetSymbolPriorityByDifficulty();

        // Calculate the best move
        var bestMoves = Utilities.CalculateMovesAndScores(shapesManager.shapes, symbolPriority);

        if (bestMoves.Count > 0)
        {
            return (bestMoves[0].Item1, bestMoves[0].Item2);
        }

        return null;
    }

    private Dictionary<string, int> GetSymbolPriorityByDifficulty()
    {
        // Set symbol priorities based on difficulty level
        switch (aiDifficulty)
        {
            case DifficultyLevel.Easy:
                return new Dictionary<string, int>
                {
                    { "Sword", 10 },
                    { "Heart", 10 },
                    { "Gold", 10 },
                    { "Energy", 10 },
                    { "Time", 10 },
                    { "Scroll", 10 }
                };

            case DifficultyLevel.Medium:
                return new Dictionary<string, int>
                {
                    { "Sword", 30 },
                    { "Heart", 20 },
                    { "Gold", 15 },
                    { "Energy", 25 },
                    { "Time", 10 },
                    { "Scroll", 15 }
                };

            case DifficultyLevel.Hard:
                return new Dictionary<string, int>
                {
                    { "Sword", 50 },
                    { "Heart", 30 },
                    { "Gold", 20 },
                    { "Energy", 40 },
                    { "Time", 10 },
                    { "Scroll", 15 }
                };

            default:
                return new Dictionary<string, int>();
        }
    }

    private IEnumerator PerformAIMove(GameObject go1, GameObject go2)
    {
        Debug.Log($"AI performs move: {go1.name} <-> {go2.name}");
        yield return new WaitForSeconds(1f); // Chờ để tạo hiệu ứng mượt mà
        yield return shapesManager.PerformSwapAndCollapse(go1, go2); // Thực hiện nước đi và xử lý match
        //EndAIActions();
    }


    public void EndAIActions()
    {
        isExecutingAction = false; // Đặt lại cờ
        shapesManager.state = GameState.None; // Trả về trạng thái chờ
        Debug.Log("AI turn completed.");
    }

}
