using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;


public class ShapesManager : Singleton<ShapesManager>
{
    public ShapesArray shapes;

    public readonly Vector2 BottomRight = new Vector2(-3.5f, -3.5f);
    public readonly Vector2 CandySize = new Vector2(1f, 1f);

    public GameState state = GameState.None;
    private GameObject hitGo = null;
    private Vector2[] SpawnPositions;
    public GameObject[] CandyPrefabs;
    public GameObject[] ExplosionPrefabs;
    public GameObject[] BonusPrefabs;

    private IEnumerator CheckPotentialMatchesCoroutine;
    private IEnumerator AnimatePotentialMatchesCoroutine;

    IEnumerable<GameObject> potentialMatches;

    public bool isMyTurn = true;  // Biến để theo dõi lượt chơi của người chơi, true = người chơi, false = AI
    public int turnCount = 1;     // Biến đếm số lượt của cả hai bên

    public CharacterInCombat playerCharacter; // Nhân vật người chơi
    public CharacterInCombat enemyCharacter;  // Nhân vật đối thủ

    public SoundManager soundManager;

    private Coroutine playerCountdownCoroutine;
    private Coroutine enemyCountdownCoroutine;

    private bool isUsingSkill = false; // Đánh dấu trạng thái skill đang được sử dụng

    public override void Awake()
    {
        MakeSingleton(false);
    }

    // Use this for initialization
    override public void Start()
    {
        InitializeTypesOnPrefabShapesAndBonuses();

        InitializeCandyAndSpawnPositions();

        StartCheckForPotentialMatches();

        //InitMyCharacter();
        playerCharacter = new CharacterInCombat
        {
            MaxHealth = 1000,
            CurrentHealth = 100,
            BaseAttack = 10,
            CurrentAttack = 10,
            MaxEnergy = 300,
            CurrentEnergy = 300,
            CurrentTime = 45,
            MaxTime = 90,
            Gold = 0,
            Experience = 0
        };

        playerCharacter.Skills = MinervaSkills.GetSkills();

        if (GameplayUIController.Ins)
        {
            GameplayUIController.Ins.UpdateHealth(isMyTurn, playerCharacter.CurrentHealth, playerCharacter.MaxHealth);
            GameplayUIController.Ins.UpdateEnergy(isMyTurn, playerCharacter.CurrentEnergy, playerCharacter.MaxEnergy);
            GameplayUIController.Ins.UpdateTime(isMyTurn, playerCharacter.CurrentTime, playerCharacter.MaxTime);
            GameplayUIController.Ins.UpdateAttack(true, playerCharacter.CurrentAttack);
            GameplayUIController.Ins.UpdateCharacter(true, Resources.Load<Sprite>("Character/Minerva/Minerva"));
            GameplayUIController.Ins.UpdateSkills(true, playerCharacter.Skills[0].Icon, playerCharacter.Skills[1].Icon, playerCharacter.Skills[2].Icon);
        }

        enemyCharacter = new CharacterInCombat
        {
            MaxHealth = 1200,
            CurrentHealth = 100,
            BaseAttack = 5,
            CurrentAttack = 12,
            MaxEnergy = 300,
            CurrentEnergy = 0,
            CurrentTime = 90,
            MaxTime = 90,
            Gold = 0,
            Experience = 0
        };

        enemyCharacter.Skills = MariusSkills.GetSkills();
        if (GameplayUIController.Ins)
        {
            GameplayUIController.Ins.UpdateHealth(!isMyTurn, enemyCharacter.CurrentHealth, enemyCharacter.MaxHealth);
            GameplayUIController.Ins.UpdateEnergy(!isMyTurn, enemyCharacter.CurrentEnergy, enemyCharacter.MaxEnergy);
            GameplayUIController.Ins.UpdateTime(!isMyTurn, enemyCharacter.CurrentTime, enemyCharacter.MaxTime);
            GameplayUIController.Ins.UpdateAttack(false, enemyCharacter.CurrentAttack);
            GameplayUIController.Ins.UpdateCharacter(false, Resources.Load<Sprite>("Character/Marius/Marius"));
            GameplayUIController.Ins.UpdateSkills(false, enemyCharacter.Skills[0].Icon, enemyCharacter.Skills[1].Icon, enemyCharacter.Skills[2].Icon);
        }

        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);

        AIController.Ins.character = enemyCharacter;
        StartCountdown(isMyTurn);
    }

    private void InitMyCharacter()
    {
        GameDataManager gameData = GameDataManager.Ins;
        if(gameData)
        playerCharacter = new CharacterInCombat
        {
            MaxHealth = gameData.GetSelectedCharacter().health,
            CurrentHealth = gameData.GetSelectedCharacter().health,
            BaseAttack = gameData.GetSelectedCharacter().attack,
            CurrentAttack = gameData.GetSelectedCharacter().attack,
            MaxEnergy = 300,
            CurrentEnergy = 300,
            CurrentTime = 90,
            MaxTime = 90,
            Gold = 0,
            Experience = 0
        };
        if(gameData.GetSelectedCharacter().name == "Minerva")
        {
            playerCharacter.Skills = MinervaSkills.GetSkills();
        }
        else if (gameData.GetSelectedCharacter().name == "Phoenix")
        {
            playerCharacter.Skills = PhoenixSkills.GetSkills();
        }
        else if (gameData.GetSelectedCharacter().name == "Jasmine")
        {
            playerCharacter.Skills = JasmineSkills.GetSkills();
        }
        else if (gameData.GetSelectedCharacter().name == "Marius")
        {
            playerCharacter.Skills = MariusSkills.GetSkills();
        }
        if (GameplayUIController.Ins)
        {
            GameplayUIController.Ins.UpdateHealth(true, playerCharacter.CurrentHealth, playerCharacter.MaxHealth);
            GameplayUIController.Ins.UpdateEnergy(true, playerCharacter.CurrentEnergy, playerCharacter.MaxEnergy);
            GameplayUIController.Ins.UpdateTime(true, playerCharacter.CurrentTime, playerCharacter.MaxTime);
            GameplayUIController.Ins.UpdateAttack(true, playerCharacter.CurrentAttack);
            GameplayUIController.Ins.UpdateCharacter(true, gameData.GetSelectedCharacter().Image);
            GameplayUIController.Ins.UpdateSkills(true, playerCharacter.Skills[0].Icon, playerCharacter.Skills[1].Icon, playerCharacter.Skills[2].Icon);
        }

    }

    /// ////////////////////////////////////////////////////
    public void OnSkillButtonClicked(int skillIndex)
    {
        if (isMyTurn)
        {
            UseSkill(skillIndex, playerCharacter);
        }
    }

    public IEnumerator ExecuteSkillLogic(List<Vector2Int> destroyedPositions)
    {
        Dictionary<string, float> destroyedSymbols = new Dictionary<string, float>
    {
        { "Sword", 0 },
        { "Heart", 0 },
        { "Gold", 0 },
        { "Energy", 0 },
        { "Scroll", 0 },
        { "Time", 0 },
    };
        List<int> affectedColumns = new List<int>();

        foreach (var pos in destroyedPositions)
        {
            int row = pos.x;
            int column = pos.y;

            if (row >= 0 && row < Constants.Rows && column >= 0 && column < Constants.Columns)
            {
                var shape = shapes[row, column];
                if (shape != null)
                {
                    // Cập nhật số lượng biểu tượng
                    if (destroyedSymbols.ContainsKey(shape.tag))
                    {
                        destroyedSymbols[shape.tag]++;
                    }

                    // Xóa biểu tượng
                    RemoveFromScene(shape);
                    shapes.Remove(shape);
                    soundManager.PlayCrincle();
                    affectedColumns.Add(column);
                }
            }
        }

        affectedColumns = affectedColumns.Distinct().ToList();

        // Xử lý rơi xuống và làm đầy
        yield return StartCoroutine(HandleCollapseAndRefill(affectedColumns));

        // Cập nhật chỉ số nhân vật
        UpdateCharacterStats(destroyedSymbols);
    }



    public IEnumerator HandleCollapseAndRefill(IEnumerable<int> affectedColumns)
    {
        int chainCount = 1; // Bắt đầu với chuỗi đầu tiên
        bool isCollapseComplete = false; // Cờ để kiểm tra hoàn tất collapse
        while (!isCollapseComplete)
        {
            // Dictionary để lưu số lượng biểu tượng thu thập được trong chain
            Dictionary<string, float> collectiblesInChain = new Dictionary<string, float>
    {
        { "Sword", 0 },
        { "Heart", 0 },
        { "Gold", 0 },
        { "Energy", 0 },
        { "Scroll", 0 },
        { "Time", 0 },
    };
            // Xử lý rơi xuống
            var collapsedCandyInfo = shapes.Collapse(affectedColumns);

            // Tạo các biểu tượng mới
            var newCandyInfo = CreateNewCandyInSpecificColumns(affectedColumns);

            // Tạo hoạt ảnh
            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);
            MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);

            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

            // Kiểm tra các trận đấu mới
            var totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy)
                .Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();

            if (totalMatches.Count() < Constants.MinimumMatches)
            {
                isCollapseComplete = true;
                // Không có trận đấu mới, thoát khỏi vòng lặp
                break;
            }
            isCollapseComplete = false;
            // Tăng số chuỗi
            chainCount++;
            soundManager.PlayCrincle();
            // Xử lý các trận đấu mới
            foreach (var match in totalMatches)
            {
                var shape = match.GetComponent<Shape>();
                if (shape != null)
                {
                    string tag = shape.tag;

                    // Cập nhật số lượng biểu tượng trong collectiblesInChain
                    if (collectiblesInChain.ContainsKey(tag))
                    {
                        collectiblesInChain[tag]++;
                    }

                    // Xóa biểu tượng khỏi scene và mảng shapes
                    RemoveFromScene(match);
                    shapes.Remove(match);
                }
            }
            // Áp dụng hệ số chain và cộng lượt
            foreach (var key in collectiblesInChain.Keys.ToList())
            {
                if (collectiblesInChain[key] >= 5)
                {
                    // Nếu có từ 5 biểu tượng trở lên, nhân với 2
                    collectiblesInChain[key] *= 2f;
                    AddTurn();
                }
                else if (collectiblesInChain[key] == 4)
                {
                    // Nếu có 4 biểu tượng, nhân với 1.5
                    collectiblesInChain[key] *= 1.5f;
                    AddTurn();
                }

                // Nhân với chainCount (hệ số chuỗi)
                collectiblesInChain[key] *= (chainCount * 0.5f + 0.5f); // chain
            }

            // Cập nhật chỉ số nhân vật sau khi xử lý xong
            UpdateCharacterStats(collectiblesInChain);
            // Cập nhật cột bị ảnh hưởng
            affectedColumns = totalMatches.Select(m => m.GetComponent<Shape>().Column).Distinct().ToList();
        }

        yield return new WaitForSeconds(1f);
        // Giảm lượt chơi
        turnCount--;
        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);

        state = GameState.None;

        if (!isMyTurn)
            AIController.Ins.EndAIActions();

        isUsingSkill = false; // Tắt cờ sau khi skill hoàn tất
    }

    public void UseSkill(int skillIndex, CharacterInCombat character)
    {
        if (isUsingSkill)
        {
            Debug.LogWarning("Skill is already being used. Please wait.");
            return; // Nếu skill đang được sử dụng, không làm gì cả
        }
        state = GameState.Animating;
        if (character.Skills != null && skillIndex < character.Skills.Count)
        {
            var skill = character.Skills[skillIndex];

            if (character.CurrentEnergy >= skill.EnergyCost)
            {
                isUsingSkill = true; // Bật cờ trước khi thực hiện skill
                character.CurrentEnergy -= skill.EnergyCost;

                // Cập nhật năng lượng trên UI (nếu có UI)
                if (GameplayUIController.Ins)
                {
                    GameplayUIController.Ins.UpdateEnergy(isMyTurn, character.CurrentEnergy, character.MaxEnergy);
                }

                // Khóa bàn cờ khi sử dụng kỹ năng
                isBoardLocked = true;
                state = GameState.Animating;

                // Thực hiện kỹ năng
                skill.Execute(character, this);

                // Mở khóa bàn cờ sau khi xử lý xong
                StartCoroutine(UnlockBoardAfterSkill());

            }
            else
            {
                Debug.Log("Not enough energy to use skill!");
                state = GameState.None;
            }
        }
    }



    IEnumerator UnlockBoardAfterSkill()
    {
        yield return new WaitForSeconds(1f);
        isBoardLocked = false;
        state = GameState.None;
    }


    /////////////////////

    private void StartCountdown(bool isPlayerTurn)
    {
        // Dừng countdown hiện tại
        if (playerCountdownCoroutine != null)
        {
            StopCoroutine(playerCountdownCoroutine);
        }
        if (enemyCountdownCoroutine != null)
        {
            StopCoroutine(enemyCountdownCoroutine);
        }

        // Bắt đầu countdown tương ứng
        if (isPlayerTurn)
        {
            playerCountdownCoroutine = StartCoroutine(PlayerCountdown());
        }
        else
        {
            enemyCountdownCoroutine = StartCoroutine(EnemyCountdown());
        }
    }


    private IEnumerator PlayerCountdown()
    {
        while (playerCharacter.CurrentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            playerCharacter.CurrentTime--;

            // Cập nhật giao diện
            if (GameplayUIController.Ins)
            {
                GameplayUIController.Ins.UpdateTime(true, playerCharacter.CurrentTime, playerCharacter.MaxTime);
            }
        }

        Debug.Log("Player's time is up!");
    }

    private IEnumerator EnemyCountdown()
    {
        while (enemyCharacter.CurrentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            enemyCharacter.CurrentTime--;

            // Cập nhật giao diện
            if (GameplayUIController.Ins)
            {
                GameplayUIController.Ins.UpdateTime(false, enemyCharacter.CurrentTime, enemyCharacter.MaxTime);
            }
        }

        Debug.Log("Enemy's time is up!");
    }



    private void InitializeTypesOnPrefabShapesAndBonuses()
    {
        //just assign the name of the prefab
        foreach (var item in CandyPrefabs)
        {
            item.GetComponent<Shape>().Type = item.name;

        }

        //assign the name of the respective "normal" candy as the type of the Bonus
        foreach (var item in BonusPrefabs)
        {
            item.GetComponent<Shape>().Type = CandyPrefabs.
                Where(x => x.GetComponent<Shape>().Type.Contains(item.name.Split('_')[1].Trim())).Single().name;
        }
    }

    public void InitializeCandyAndSpawnPositions()
    {
        Debug.Log("Restart The Map.");

        if (shapes != null)
            DestroyAllCandy();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = GetRandomCandy();

                //check if two previous horizontal are of the same type
                while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }

                //check if two previous vertical are of the same type
                while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row - 2, column].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }

                InstantiateAndPlaceNewCandy(row, column, newCandy);

            }
        }

        SetupSpawnPositions();
    }

    private void InstantiateAndPlaceNewCandy(int row, int column, GameObject newCandy)
    {
        GameObject go = Instantiate(newCandy,
            BottomRight + new Vector2(column * CandySize.x, row * CandySize.y), Quaternion.identity)
            as GameObject;

        //assign the specific properties
        go.GetComponent<Shape>().Assign(newCandy.GetComponent<Shape>().Type, row, column);
        shapes[row, column] = go;
    }

    private void SetupSpawnPositions()
    {
        //create the spawn positions for the new shapes (will pop from the 'ceiling')
        for (int column = 0; column < Constants.Columns; column++)
        {
            SpawnPositions[column] = BottomRight
                + new Vector2(column * CandySize.x, Constants.Rows * CandySize.y);
        }
    }

    private void DestroyAllCandy()
    {
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                if (shapes[row, column])
                    Destroy(shapes[row, column]);
            }
        }
    }


    public void ChangeTurn()
    {
        if (isUsingSkill)
        {
            Debug.Log("Cannot change turn while skill is in use.");
            return;
        }
        isMyTurn = !isMyTurn;

        turnCount = 1;
        state = GameState.None;
        
        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);
        StartCountdown(isMyTurn);
        Debug.Log(isMyTurn ? "Player's turn started!" : "Enemy's turn started!");
    }
    private void AddTurn()
    {
        turnCount++;
        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);
    }

    

    private bool isBoardLocked = false; // Biến kiểm tra trạng thái bàn cờ

    private IEnumerator HandleEndGame(bool isPlayerDefeated)
    {
        // Đợi 1 giây để đảm bảo UI đã cập nhật máu về 0
        yield return new WaitForSeconds(1f);

        if (isPlayerDefeated)
        {
            // Hiện dialog thua
            GameplayUIController.Ins.UpdateLoseDialog(GameDataManager.Ins.GetCurrentHealthEner() - 1);
            GameplayUIController.Ins.ShowLoseDialog();
        }
        else
        {
            GameDataManager gameData = GameDataManager.Ins;
            if(gameData)
            // Hiện dialog thắng
            GameplayUIController.Ins.UpdateWinDialog(gameData.GetSelectedCharacter().Image, gameData.GetSelectedCharacter().name, gameData.GetSelectedCharacter().Level, gameData.GetSelectedCharacter().attack, gameData.GetSelectedCharacter().health,gameData.GetSelectedCharacter().Exp,gameData.GetSelectedCharacter().MaxExp,playerCharacter.Gold,playerCharacter.Experience);
            GameplayUIController.Ins.ShowWinDialog();
        }
    }


    void Update()
    {
        if (isUsingSkill) return; // Không làm gì nếu đang sử dụng skill

        if (playerCharacter.CurrentHealth <= 0 || playerCharacter.CurrentTime <= 0)
        {
            // Nếu người chơi thua
            state = GameState.Lose;
            StartCoroutine(HandleEndGame(true));
            return;
        }
        else if (enemyCharacter.CurrentHealth <= 0 || enemyCharacter.CurrentTime <= 0)
        {
            // Nếu người chơi thắng
            state = GameState.Win;
            StartCoroutine(HandleEndGame(false));
            return;
        }

        // Kiểm tra và reset bàn cờ nếu cần thiết, nhưng không xử lý khi bàn cờ bị khóa
        if (!isBoardLocked)
        {
            CheckAndResetBoardIfNeeded();
        }

        if (turnCount <= 0 && state == GameState.None)
        {
            ChangeTurn();
        }


        // Trạng thái AI thực hiện nước đi
        if (state == GameState.None && !isMyTurn)
        {
            state = GameState.Animating;
            if (!isBoardLocked)
            {
                AIController.Ins.ExecuteAIMove(AIController.Ins.aiDifficulty.ToString());
            }
            return;
        }
        //
        if (state == GameState.None && isMyTurn)
        {
            // Người dùng đã nhấp chuột hoặc chạm màn hình
            if (Input.GetMouseButtonDown(0) && !isBoardLocked)
            {
                // Lấy vị trí va chạm
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null) // Nếu có va chạm!
                {
                    hitGo = hit.collider.gameObject;

                    // Kiểm tra xem hitGo có hợp lệ hay không (không phải null)
                    if (hitGo != null)
                    {
                        state = GameState.SelectionStarted;
                    }
                    else
                    {
                        // Nếu hitGo bị null, tránh tiếp tục xử lý
                        Debug.LogWarning("Selected GameObject is null.");
                    }
                }
            }
        }
        else if (state == GameState.SelectionStarted && isMyTurn)
        {
            // Người dùng kéo chuột
            if (Input.GetMouseButton(0) && !isBoardLocked)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                // Kiểm tra nếu có va chạm và đối tượng va chạm khác với đối tượng đã chọn
                if (hit.collider != null && hitGo != hit.collider.gameObject)
                {
                    // Người dùng đã thực hiện một thao tác, không cần hiển thị gợi ý
                    StopCheckForPotentialMatches();

                    // Kiểm tra nếu hai hình có thể hoán đổi (nằm cạnh nhau)
                    Shape hitShape = hit.collider.gameObject.GetComponent<Shape>();
                    Shape hitGoShape = hitGo.GetComponent<Shape>();

                    // Kiểm tra nếu cả hai đối tượng vẫn hợp lệ và có thể hoán đổi
                    if (hitShape != null && hitGoShape != null && Utilities.AreVerticalOrHorizontalNeighbors(hitGoShape, hitShape))
                    {
                        state = GameState.Animating;

                        // Điều chỉnh layer sắp xếp để hiển thị đúng thứ tự
                        FixSortingLayer(hitGo, hit.collider.gameObject);

                        // Bắt đầu coroutine tìm kiếm và xử lý các trận đấu
                        StartCoroutine(FindMatchesAndCollapse(hit));

                    }
                    else
                    {
                        state = GameState.None;  // Đặt lại trạng thái trò chơi
                    }
                }
            }
        }

    }

    // Kiểm tra và reset bàn cờ nếu không còn nước đi hợp lệ
    private void CheckAndResetBoardIfNeeded()
    {
        if (!HasValidMoves() && !isBoardLocked)
        {
            LockBoard();
            // Đợi 1 giây trước khi reset bàn cờ
            StartCoroutine(WaitBeforeResetBoard());
        }
    }

    // Kiểm tra nếu có bước đi hợp lệ nào
    private bool HasValidMoves()
    {
        return Utilities.GetPotentialMatches(shapes) != null;  // Kiểm tra bước đi hợp lệ
    }

    // Khóa bàn cờ (không cho phép người chơi thao tác)
    private void LockBoard()
    {
        isBoardLocked = true; // Đánh dấu bàn cờ bị khóa
    }

    // Mở khóa bàn cờ khi có bước đi hợp lệ
    private void UnlockBoard()
    {
        isBoardLocked = false; // Bàn cờ được mở khóa
    }

    // Coroutine chờ 1 giây trước khi reset bàn cờ
    private IEnumerator WaitBeforeResetBoard()
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(1f);

        // Kiểm tra lại lần nữa nếu không có bước đi hợp lệ
        if (!HasValidMoves())
        {
            Debug.Log("Resetting board...");
            InitializeCandyAndSpawnPositions();  // Reset bàn cờ
        }

        UnlockBoard();  // Mở lại bàn cờ sau khi reset
        
    }

    private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
    {
        SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
        SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
        if (sp1.sortingOrder <= sp2.sortingOrder)
        {
            sp1.sortingOrder = 1;
            sp2.sortingOrder = 0;
        }
    }

    public IEnumerator PerformSwapAndCollapse(GameObject go1, GameObject go2)
    {
        state = GameState.Animating;
        // Lấy đối tượng thứ hai trong việc hoán đổi

        shapes.Swap(go1, go2);

        // Di chuyển các đối tượng đã hoán đổi
        go1.transform.DOMove(go2.transform.position, Constants.AnimationDuration);
        go2.transform.DOMove(go1.transform.position, Constants.AnimationDuration);
        yield return new WaitForSeconds(Constants.AnimationDuration);

        // Lấy các match từ hai đối tượng đã hoán đổi
        var hitGoMatchesInfo = shapes.GetMatches(go1);
        var hitGo2MatchesInfo = shapes.GetMatches(go2);

        var totalMatches = hitGoMatchesInfo.MatchedCandy
            .Union(hitGo2MatchesInfo.MatchedCandy).Distinct();

        // Nếu số lượng match ít hơn 3, quay lại và không thực hiện hoán đổi
        if (totalMatches.Count() < Constants.MinimumMatches)
        {
            go1.transform.DOMove(go2.transform.position, Constants.AnimationDuration);
            go2.transform.DOMove(go1.transform.position, Constants.AnimationDuration);
            yield return new WaitForSeconds(Constants.AnimationDuration);

            shapes.UndoSwap();
        }

        int timesRun = 1;

        while (totalMatches.Count() >= Constants.MinimumMatches)
        {
            // Tính điểm cho trận đấu và biểu tượng thu thập
            Dictionary<string, float> collectiblesInChain = new Dictionary<string, float>
        {
            { "Sword", 0 },
            { "Heart", 0 },
            { "Gold", 0 },
            { "Energy", 0 },
            { "Scroll", 0 },
            { "Time", 0 },
        };

            foreach (var match in totalMatches)
            {
                string collectibleTag = match.tag; // Lấy Tag của đối tượng (sword, heart, gold, energy)
                if (!collectiblesInChain.ContainsKey(collectibleTag))
                {
                    collectiblesInChain[collectibleTag] = 0; // Nếu chưa có, khởi tạo bằng 0
                }

                collectiblesInChain[collectibleTag]++;  // Tăng số lượng biểu tượng thu thập
            }

            foreach (var key in collectiblesInChain.Keys.ToList())
            {
                if (collectiblesInChain[key] >= 5)
                {
                    // Nếu có từ 5 biểu tượng trở lên, nhân với 2
                    collectiblesInChain[key] *= 2f;
                    AddTurn();
                }
                else if (collectiblesInChain[key] == 4)
                {
                    // Nếu có 4 biểu tượng, nhân với 1.5
                    collectiblesInChain[key] *= 1.5f;
                    AddTurn();
                }

                // Nhân với timesRun (hệ số chuỗi)
                collectiblesInChain[key] *= (timesRun * 0.5f + 0.5f);  // chain 

            }


            soundManager.PlayCrincle();

            List<int> affectedColumns = new List<int>();

            Dictionary<string, float> collectiblesInExplode = new Dictionary<string, float>
        {
            { "Sword", 0 },
            { "Heart", 0 },
            { "Gold", 0 },
            { "Energy", 0 },
            { "Scroll", 0 },
            { "Time", 0 },
        };


            foreach (var item in totalMatches)
            {
                var shape = item.GetComponent<Shape>();

                if (shape.Bonus == BonusType.SpecialSword)
                {
                    // Xử lý nổ 3x3 và thu thập các biểu tượng bị ảnh hưởng
                    var affectedShapes = HandleSpecialSword(item);

                    foreach (var affected in affectedShapes)
                    {
                        string collectibleTag = affected.tag;

                        if (collectiblesInExplode.ContainsKey(collectibleTag))
                        {
                            collectiblesInExplode[collectibleTag]++;
                        }
                        else
                        {
                            collectiblesInExplode[collectibleTag] = 1;
                        }

                        shapes.Remove(affected);
                        RemoveFromScene(affected);

                        int column = affected.GetComponent<Shape>().Column;
                        if (!affectedColumns.Contains(column))
                        {
                            affectedColumns.Add(column);
                        }
                    }
                }
                else
                {
                    // Xử lý biểu tượng thường
                    shapes.Remove(item);
                    RemoveFromScene(item);

                    int column = item.GetComponent<Shape>().Column;
                    if (!affectedColumns.Contains(column))
                    {
                        affectedColumns.Add(column);
                    }
                }
            }

            foreach (var key in collectiblesInExplode.Keys.ToList())
            {
                // Nhân với timesRun (hệ số chuỗi)
                collectiblesInExplode[key] *= (timesRun * 0.5f + 0.5f);  // chain 
                collectiblesInChain[key] += collectiblesInExplode[key];

            }

            UpdateCharacterStats(collectiblesInChain);
            // In ra số lượng biểu tượng thu thập được sau mỗi chuỗi (chain)
            Debug.Log("Chain: " + timesRun);

            // Loại bỏ cột trùng lặp
            affectedColumns = affectedColumns.Distinct().ToList();
            // Xử lý các cột bị ảnh hưởng
            var collapsedCandyInfo = shapes.Collapse(affectedColumns);
            var newCandyInfo = CreateNewCandyInSpecificColumns(affectedColumns);

            // Di chuyển và hoạt ảnh
            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);
            MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);

            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy)
                .Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();

            //////
            // Nếu không có match mới, thoát vòng lặp
            if (totalMatches.Count() < Constants.MinimumMatches)
                break;

            timesRun++;

        }
        turnCount--;/////////
        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);
        Debug.Log(turnCount + " turn remain");

        AIController.Ins.EndAIActions();

    }

    private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    {
        // Lấy đối tượng thứ hai trong việc hoán đổi
        var hitGo2 = hit2.collider.gameObject;
        shapes.Swap(hitGo, hitGo2);

        // Di chuyển các đối tượng đã hoán đổi
        hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
        hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
        yield return new WaitForSeconds(Constants.AnimationDuration);

        // Lấy các match từ hai đối tượng đã hoán đổi
        var hitGoMatchesInfo = shapes.GetMatches(hitGo);
        var hitGo2MatchesInfo = shapes.GetMatches(hitGo2);

        var totalMatches = hitGoMatchesInfo.MatchedCandy
            .Union(hitGo2MatchesInfo.MatchedCandy).Distinct();

        // Nếu số lượng match ít hơn 3, quay lại và không thực hiện hoán đổi
        if (totalMatches.Count() < Constants.MinimumMatches)
        {
            hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
            hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
            yield return new WaitForSeconds(Constants.AnimationDuration);

            shapes.UndoSwap();
        }

        int timesRun = 1;

        while (totalMatches.Count() >= Constants.MinimumMatches)
        {
            // Tính điểm cho trận đấu và biểu tượng thu thập
            Dictionary<string, float> collectiblesInChain = new Dictionary<string, float>
        {
            { "Sword", 0 },
            { "Heart", 0 },
            { "Gold", 0 },
            { "Energy", 0 },
            { "Scroll", 0 },
            { "Time", 0 },
        };

            foreach (var match in totalMatches)
            {
                string collectibleTag = match.tag; // Lấy Tag của đối tượng (sword, heart, gold, energy)
                if (!collectiblesInChain.ContainsKey(collectibleTag))
                {
                    collectiblesInChain[collectibleTag] = 0; // Nếu chưa có, khởi tạo bằng 0
                }

                collectiblesInChain[collectibleTag]++;  // Tăng số lượng biểu tượng thu thập
            }

            foreach (var key in collectiblesInChain.Keys.ToList())
            {
                if (collectiblesInChain[key] >= 5)
                {
                    // Nếu có từ 5 biểu tượng trở lên, nhân với 2
                    collectiblesInChain[key] *= 2f;
                    AddTurn();
                }
                else if (collectiblesInChain[key] == 4)
                {
                    // Nếu có 4 biểu tượng, nhân với 1.5
                    collectiblesInChain[key] *= 1.5f;
                    AddTurn();
                }

                // Nhân với timesRun (hệ số chuỗi)
                collectiblesInChain[key] *= (timesRun * 0.5f + 0.5f);  // chain 

            }

            
            soundManager.PlayCrincle();

            List<int> affectedColumns = new List<int>();

            Dictionary<string, float> collectiblesInExplode = new Dictionary<string, float>
        {
            { "Sword", 0 },
            { "Heart", 0 },
            { "Gold", 0 },
            { "Energy", 0 },
            { "Scroll", 0 },
            { "Time", 0 },
        };


            foreach (var item in totalMatches)
            {
                var shape = item.GetComponent<Shape>();

                if (shape.Bonus == BonusType.SpecialSword)
                {
                    // Xử lý nổ 3x3 và thu thập các biểu tượng bị ảnh hưởng
                    var affectedShapes = HandleSpecialSword(item);

                    foreach (var affected in affectedShapes)
                    {
                        string collectibleTag = affected.tag;

                        if (collectiblesInExplode.ContainsKey(collectibleTag))
                        {
                            collectiblesInExplode[collectibleTag]++;
                        }
                        else
                        {
                            collectiblesInExplode[collectibleTag] = 1;
                        }

                        shapes.Remove(affected);
                        RemoveFromScene(affected);

                        int column = affected.GetComponent<Shape>().Column;
                        if (!affectedColumns.Contains(column))
                        {
                            affectedColumns.Add(column);
                        }
                    }
                }
                else
                {
                    // Xử lý biểu tượng thường
                    shapes.Remove(item);
                    RemoveFromScene(item);

                    int column = item.GetComponent<Shape>().Column;
                    if (!affectedColumns.Contains(column))
                    {
                        affectedColumns.Add(column);
                    }
                }
            }

            foreach (var key in collectiblesInExplode.Keys.ToList())
            {
                // Nhân với timesRun (hệ số chuỗi)
                collectiblesInExplode[key] *= (timesRun * 0.5f + 0.5f);  // chain 
                collectiblesInChain[key] += collectiblesInExplode[key];

            }

            UpdateCharacterStats(collectiblesInChain);
            // In ra số lượng biểu tượng thu thập được sau mỗi chuỗi (chain)
            Debug.Log("Chain: " + timesRun);

            // Loại bỏ cột trùng lặp
            affectedColumns = affectedColumns.Distinct().ToList();
            // Xử lý các cột bị ảnh hưởng
            var collapsedCandyInfo = shapes.Collapse(affectedColumns);
            var newCandyInfo = CreateNewCandyInSpecificColumns(affectedColumns);

            // Di chuyển và hoạt ảnh
            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);
            MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);

            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy)
                .Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();

            //////
            // Nếu không có match mới, thoát vòng lặp
            if (totalMatches.Count() < Constants.MinimumMatches)
                break;

            timesRun++;
            
        }
        turnCount--;/////////
        if (TurnCounterUI.Ins)
            TurnCounterUI.Ins.UpdateTurnCounter(isMyTurn, turnCount);
        Debug.Log(turnCount + " turn remain");

        state = GameState.None;
        StartCheckForPotentialMatches();
    }

    private void UpdateCharacterStats(Dictionary<string, float> collectibles)
    {
        // Xác định nhân vật hiện tại và đối thủ
        CharacterInCombat currentCharacter = isMyTurn ? playerCharacter : enemyCharacter;
        CharacterInCombat opponentCharacter = isMyTurn ? enemyCharacter : playerCharacter;

        Debug.Log($"Updating stats for {(isMyTurn ? "Player" : "Enemy")}'s turn:");

        foreach (var key in collectibles.Keys)
        {
            float value = collectibles[key]; // Giữ nguyên float để tính toán

            switch (key)
            {
                case "Sword":
                    // Tính sát thương dựa trên CurrentAttack và làm tròn kết quả
                    float rawDamage = currentCharacter.CurrentAttack * value;
                    int damage = Mathf.RoundToInt(rawDamage);
                    opponentCharacter.CurrentHealth -= damage;
                    opponentCharacter.CurrentHealth = Mathf.Max(0, opponentCharacter.CurrentHealth); // Giới hạn không dưới 0
                    Debug.Log($" - {key}: {value} -> Dealt {damage} damage to {(isMyTurn ? "Enemy" : "Player")}");
                    if (GameplayUIController.Ins)
                        GameplayUIController.Ins.UpdateHealth(!isMyTurn, opponentCharacter.CurrentHealth, opponentCharacter.MaxHealth);
                    break;

                case "Heart":
                    // Hồi máu dựa trên 2% MaxHealth và làm tròn kết quả
                    float rawHeal = 0.02f * value * currentCharacter.MaxHealth;
                    int healAmount = Mathf.RoundToInt(rawHeal);
                    currentCharacter.CurrentHealth = Mathf.Min(currentCharacter.MaxHealth, currentCharacter.CurrentHealth + healAmount);
                    Debug.Log($" - {key}: {value} -> Restored {healAmount} health to {(isMyTurn ? "Player" : "Enemy")}");
                    if (GameplayUIController.Ins)
                        GameplayUIController.Ins.UpdateHealth(isMyTurn, currentCharacter.CurrentHealth, currentCharacter.MaxHealth);
                    break;

                case "Gold":
                    // Tăng vàng và làm tròn kết quả
                    float rawGold = value * 10f;
                    int goldGained = Mathf.RoundToInt(rawGold);
                    currentCharacter.Gold += goldGained;
                    Debug.Log($" - {key}: {value} -> Gained {goldGained} gold for {(isMyTurn ? "Player" : "Enemy")}");
                    break;

                case "Energy":
                    // Tăng năng lượng và làm tròn kết quả
                    float rawEnergy = value * 10f;
                    int energyGained = Mathf.RoundToInt(rawEnergy);
                    currentCharacter.CurrentEnergy = Mathf.Min(currentCharacter.MaxEnergy, currentCharacter.CurrentEnergy + energyGained);
                    Debug.Log($" - {key}: {value} -> Restored {energyGained} energy for {(isMyTurn ? "Player" : "Enemy")}");
                    if (GameplayUIController.Ins)
                        GameplayUIController.Ins.UpdateEnergy(isMyTurn, currentCharacter.CurrentEnergy, currentCharacter.MaxEnergy);
                    break;

                case "Time":
                    // Tăng thời gian (lưu ý giá trị thời gian vẫn là float)
                    float rawTime = value * 2f;
                    currentCharacter.CurrentTime = Mathf.Min(currentCharacter.MaxTime, currentCharacter.CurrentTime + rawTime);
                    Debug.Log($" - {key}: {value} -> Added {rawTime} seconds to {(isMyTurn ? "Player" : "Enemy")}'s timer");
                    if (GameplayUIController.Ins)
                        GameplayUIController.Ins.UpdateTime(isMyTurn, currentCharacter.CurrentTime, currentCharacter.MaxTime);
                    break;

                case "Scroll":
                    // Tăng kinh nghiệm và làm tròn kết quả
                    float rawExp = value * 10f;
                    int expGained = Mathf.RoundToInt(rawExp);
                    currentCharacter.Experience += expGained;
                    Debug.Log($" - {key}: {value} -> Gained {expGained} experience for {(isMyTurn ? "Player" : "Enemy")}");
                    break;
            }
        }

        Debug.Log($"Updated stats for {(isMyTurn ? "Player" : "Enemy")}: Health: {currentCharacter.CurrentHealth}/{currentCharacter.MaxHealth}, Energy: {currentCharacter.CurrentEnergy}/{currentCharacter.MaxEnergy}, Gold: {currentCharacter.Gold}, Experience: {currentCharacter.Experience}");
    }


    List<GameObject> HandleSpecialSword(GameObject specialSword)
    {
        // Danh sách chứa tất cả các biểu tượng bị ảnh hưởng bởi nổ lan
        List<GameObject> affectedShapes = new List<GameObject>();

        // Hàng và cột của SpecialSword hiện tại
        var shape = specialSword.GetComponent<Shape>();
        int row = shape.Row;
        int column = shape.Column;

        // Danh sách hàng đợi cho hiệu ứng nổ lan
        Queue<GameObject> explosionQueue = new Queue<GameObject>();
        explosionQueue.Enqueue(specialSword);

        while (explosionQueue.Count > 0)
        {
            // Lấy một biểu tượng từ hàng đợi để xử lý nổ
            var currentSpecialSword = explosionQueue.Dequeue();
            var currentShape = currentSpecialSword.GetComponent<Shape>();
            int currentRow = currentShape.Row;
            int currentColumn = currentShape.Column;

            // Gây nổ khu vực 3x3 xung quanh biểu tượng hiện tại
            for (int r = currentRow - 1; r <= currentRow + 1; r++)
            {
                for (int c = currentColumn - 1; c <= currentColumn + 1; c++)
                {
                    // Kiểm tra giới hạn mảng
                    if (r >= 0 && r < Constants.Rows && c >= 0 && c < Constants.Columns)
                    {
                        var affected = shapes[r, c];
                        if (affected != null && !affectedShapes.Contains(affected))
                        {
                            // Thêm biểu tượng bị ảnh hưởng vào danh sách
                            affectedShapes.Add(affected);

                            // Nếu biểu tượng bị ảnh hưởng cũng là SpecialSword, thêm vào hàng đợi
                            var affectedShape = affected.GetComponent<Shape>();
                            if (affectedShape.Bonus == BonusType.SpecialSword)
                            {
                                explosionQueue.Enqueue(affected);
                            }
                        }
                    }
                }
            }
        }

        return affectedShapes; // Trả về tất cả biểu tượng bị phá hủy bởi nổ lan
    }




    private AlteredCandyInfo CreateNewCandyInSpecificColumns(IEnumerable<int> columnsWithMissingCandy)
    {
        AlteredCandyInfo newCandyInfo = new AlteredCandyInfo();

        //find how many null values the column has
        foreach (int column in columnsWithMissingCandy)
        {
            var emptyItems = shapes.GetEmptyItemsOnColumn(column);
            foreach (var item in emptyItems)
            {
                var go = GetRandomCandy();
                GameObject newCandy;

                if (UnityEngine.Random.value < Constants.SpecialSwordRate)
                {
                    go = GetSpecialSwordCandy();
                    newCandy = Instantiate(go, SpawnPositions[column], Quaternion.identity) as GameObject;
                    newCandy.GetComponent<Shape>().Bonus = BonusType.SpecialSword; // Gán BonusType
                }
                else
                {
                    go = GetRandomCandy();
                    newCandy = Instantiate(go, SpawnPositions[column], Quaternion.identity) as GameObject;
                }

                

                newCandy.GetComponent<Shape>().Assign(go.GetComponent<Shape>().Type, item.Row, item.Column);

                if (Constants.Rows - item.Row > newCandyInfo.MaxDistance)
                    newCandyInfo.MaxDistance = Constants.Rows - item.Row;

                shapes[item.Row, item.Column] = newCandy;
                newCandyInfo.AddCandy(newCandy);
            }
        }
        return newCandyInfo;
    }

    private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
    {
        foreach (var item in movedGameObjects)
        {
            item.transform.DOMove(
                BottomRight + new Vector2(item.GetComponent<Shape>().Column * CandySize.x,
                    item.GetComponent<Shape>().Row * CandySize.y),
                Constants.MoveAnimationMinDuration * distance
            );
        }
    }

    public void RemoveFromScene(GameObject item)
    {
        GameObject explosion = GetRandomExplosion();
        var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
        Destroy(newExplosion, Constants.ExplosionDuration);
        Destroy(item);
    }


    private GameObject GetRandomCandy()
    {
        return CandyPrefabs[Random.Range(0, CandyPrefabs.Length)];
    }

    private GameObject GetSpecialSwordCandy()
    {
        return BonusPrefabs[0];
    }

    private GameObject GetRandomExplosion()
    {
        return ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)];
    }

    private void StartCheckForPotentialMatches()
    {
        StopCheckForPotentialMatches();
        //get a reference to stop it later
        CheckPotentialMatchesCoroutine = CheckPotentialMatches();
        StartCoroutine(CheckPotentialMatchesCoroutine);
    }

    private void StopCheckForPotentialMatches()
    {
        if (AnimatePotentialMatchesCoroutine != null)
            StopCoroutine(AnimatePotentialMatchesCoroutine);
        if (CheckPotentialMatchesCoroutine != null)
            StopCoroutine(CheckPotentialMatchesCoroutine);
        ResetOpacityOnPotentialMatches();
    }

    private void ResetOpacityOnPotentialMatches()
    {
        if (potentialMatches != null)
            foreach (var item in potentialMatches)
            {
                if (item == null) break;

                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = 1.0f;
                item.GetComponent<SpriteRenderer>().color = c;
            }
    }

    private IEnumerator CheckPotentialMatches()
    {
        yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
        potentialMatches = Utilities.GetPotentialMatches(shapes);
        if (potentialMatches != null)
        {
            while (true)
            {
                AnimatePotentialMatchesCoroutine = Utilities.AnimatePotentialMatches(potentialMatches);
                StartCoroutine(AnimatePotentialMatchesCoroutine);
                yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
            }
        }
    }

}
