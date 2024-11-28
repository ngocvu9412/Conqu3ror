using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;


public class ShapesManager : MonoBehaviour
{
    public Text DebugText, ScoreText, CollectibleCountText;
    public bool ShowDebugInfo = false;
                                       

    public ShapesArray shapes;

    private int score;

    public readonly Vector2 BottomRight = new Vector2(-2.37f, -4.27f);
    public readonly Vector2 CandySize = new Vector2(0.7f, 0.7f);

    private GameState state = GameState.None;
    private GameObject hitGo = null;
    private Vector2[] SpawnPositions;
    public GameObject[] CandyPrefabs;
    public GameObject[] ExplosionPrefabs;
    public GameObject[] BonusPrefabs;

    private IEnumerator CheckPotentialMatchesCoroutine;
    private IEnumerator AnimatePotentialMatchesCoroutine;

    IEnumerable<GameObject> potentialMatches;

    public SoundManager soundManager;
    void Awake()
    {
        DebugText.enabled = ShowDebugInfo;
    }

    // Use this for initialization
    void Start()
    {
        InitializeTypesOnPrefabShapesAndBonuses();

        InitializeCandyAndSpawnPositions();

        StartCheckForPotentialMatches();
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

    public void InitializeCandyAndSpawnPositionsFromPremadeLevel()
    {
        InitializeVariables();

        var premadeLevel = DebugUtilities.FillShapesArrayFromResourcesData();

        if (shapes != null)
            DestroyAllCandy();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = null;

                newCandy = GetSpecificCandyOrBonusForPremadeLevel(premadeLevel[row, column]);

                InstantiateAndPlaceNewCandy(row, column, newCandy);

            }
        }

        SetupSpawnPositions();
    }


    public void InitializeCandyAndSpawnPositions()
    {
        InitializeVariables();

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

    private bool isBoardLocked = false; // Biến kiểm tra trạng thái bàn cờ

    void Update()
    {
        // Kiểm tra và reset bàn cờ nếu cần thiết, nhưng không xử lý khi bàn cờ bị khóa
        if (!isBoardLocked)
        {
            CheckAndResetBoardIfNeeded();
        }

        // Hiển thị thông tin debug nếu cần
        if (ShowDebugInfo)
            DebugText.text = DebugUtilities.GetArrayContents(shapes);

        if (state == GameState.None)
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
        else if (state == GameState.SelectionStarted)
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
                        // Nếu thao tác không hợp lệ, quay lại trạng thái ban đầu mà không reset bảng
                        Debug.Log("Invalid move, no reset.");
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

    //private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    //{
    //    //get the second item that was part of the swipe
    //    var hitGo2 = hit2.collider.gameObject;
    //    shapes.Swap(hitGo, hitGo2);

    //    //move the swapped ones
    //    hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
    //    hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
    //    yield return new WaitForSeconds(Constants.AnimationDuration);

    //    //get the matches via the helper methods
    //    var hitGomatchesInfo = shapes.GetMatches(hitGo);
    //    var hitGo2matchesInfo = shapes.GetMatches(hitGo2);

    //    var totalMatches = hitGomatchesInfo.MatchedCandy
    //        .Union(hitGo2matchesInfo.MatchedCandy).Distinct();

    //    //if user's swap didn't create at least a 3-match, undo their swap
    //    if (totalMatches.Count() < Constants.MinimumMatches)
    //    {
    //        hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
    //        hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
    //        yield return new WaitForSeconds(Constants.AnimationDuration);

    //        shapes.UndoSwap();
    //    }

    //    int timesRun = 1;
    //    while (totalMatches.Count() >= Constants.MinimumMatches)
    //    {
    //        //increase score
    //        IncreaseScore((totalMatches.Count() - 2) * Constants.Match3Score);

    //        if (timesRun >= 2)
    //            IncreaseScore(Constants.SubsequentMatchScore);

    //        soundManager.PlayCrincle();

    //        foreach (var item in totalMatches)
    //        {
    //            shapes.Remove(item);
    //            RemoveFromScene(item);
    //        }

    //        //get the columns that we had a collapse
    //        var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();

    //        //the order the 2 methods below get called is important!!!
    //        //collapse the ones gone
    //        var collapsedCandyInfo = shapes.Collapse(columns);
    //        //create new ones
    //        var newCandyInfo = CreateNewCandyInSpecificColumns(columns);

    //        int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);

    //        MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
    //        MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);

    //        //will wait for both of the above animations
    //        yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

    //        //search if there are matches with the new/collapsed items
    //        totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy).
    //            Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();

    //        timesRun++;
    //    }

    //    state = GameState.None;
    //    StartCheckForPotentialMatches();
    //}

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
            int totalChainPoints = 0;
            Dictionary<string, float> collectiblesInChain = new Dictionary<string, float>
        {
            { "Sword", 0 },
            { "Heart", 0 },
            { "Gold", 0 },
            { "Energy", 0 },
            { "Scroll", 0 },
            { "Time", 0 },
        };

            // Tính số lượng biểu tượng thu thập trong mỗi trận đấu (chain)
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
                }
                else if (collectiblesInChain[key] == 4)
                {
                    // Nếu có 4 biểu tượng, nhân với 1.5
                    collectiblesInChain[key] *= 1.5f;
                }

                // Nhân với timesRun (hệ số chuỗi)
                collectiblesInChain[key] *= (timesRun * 0.5f + 0.5f);  // chain 

            }

            // Tính điểm cho chuỗi (chain) và cộng dồn
            totalChainPoints = CalculateMatchPoints(timesRun);
            IncreaseScore(totalChainPoints);

            // In ra số lượng biểu tượng thu thập được sau mỗi chuỗi (chain)
            Debug.Log("Chain: " + timesRun);
            Debug.Log("Sword Count: " + collectiblesInChain["Sword"]);
            Debug.Log("Heart Count: " + collectiblesInChain["Heart"]);
            Debug.Log("Gold Count: " + collectiblesInChain["Gold"]);
            Debug.Log("Energy Count: " + collectiblesInChain["Energy"]);
            Debug.Log("Scroll Count: " + collectiblesInChain["Scroll"]);
            Debug.Log("Time Count: " + collectiblesInChain["Time"]);
            Debug.Log("");


            soundManager.PlayCrincle();

            foreach (var item in totalMatches)
            {
                shapes.Remove(item);
                RemoveFromScene(item);
            }

            var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();

            var collapsedCandyInfo = shapes.Collapse(columns);
            var newCandyInfo = CreateNewCandyInSpecificColumns(columns);

            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);

            MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);

            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy)
                .Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();

            // Nếu không có match mới, thoát vòng lặp
            if (totalMatches.Count() < Constants.MinimumMatches)
                break;

            timesRun++;
        }

        state = GameState.None;
        StartCheckForPotentialMatches();
    }

    // Tính điểm cho trận đấu dựa trên số lượng match
    private int CalculateMatchPoints(int timesRun)
    {
        int basePoints = Constants.Match3Score; // Điểm gốc cho một trận đấu cơ bản (3 match)

        // Tính điểm theo chuỗi (chain) dựa trên timesRun
        float multiplier = 1f + (0.5f * (timesRun - 1)); // Mỗi lần chain mới tăng 50% điểm (chain 2 = 1.5x, chain 3 = 2x,...)
        float totalPoints = basePoints * multiplier;

        return Mathf.FloorToInt(totalPoints); // Làm tròn số điểm
    }

    //private void CreateBonus(Shape hitGoCache)
    //{
    //    GameObject Bonus = Instantiate(GetBonusFromType(hitGoCache.Type), BottomRight
    //        + new Vector2(hitGoCache.Column * CandySize.x,
    //            hitGoCache.Row * CandySize.y), Quaternion.identity)
    //        as GameObject;
    //    shapes[hitGoCache.Row, hitGoCache.Column] = Bonus;
    //    var BonusShape = Bonus.GetComponent<Shape>();
    //    //will have the same type as the "normal" candy
    //    BonusShape.Assign(hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
    //    //add the proper Bonus type
    //    BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;
    //}

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
                GameObject newCandy = Instantiate(go, SpawnPositions[column], Quaternion.identity)
                    as GameObject;

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

    private void RemoveFromScene(GameObject item)
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

    private void InitializeVariables()
    {
        score = 0;
        ShowScore();
    }

    private void IncreaseScore(int amount)
    {
        score += amount;
        ShowScore();
    }

    private void ShowScore()
    {
        ScoreText.text = "Score: " + score.ToString();
    }

    private GameObject GetRandomExplosion()
    {
        return ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)];
    }

    private GameObject GetBonusFromType(string type)
    {
        string color = type.Split('_')[1].Trim();
        foreach (var item in BonusPrefabs)
        {
            if (item.GetComponent<Shape>().Type.Contains(color))
                return item;
        }
        throw new System.Exception("Wrong type");
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

    private GameObject GetSpecificCandyOrBonusForPremadeLevel(string info)
    {
        var tokens = info.Split('_');

        if (tokens.Count() == 1)
        {
            foreach (var item in CandyPrefabs)
            {
                if (item.GetComponent<Shape>().Type.Contains(tokens[0].Trim()))
                    return item;
            }

        }
        else if (tokens.Count() == 2 && tokens[1].Trim() == "B")
        {
            foreach (var item in BonusPrefabs)
            {
                if (item.name.Contains(tokens[0].Trim()))
                    return item;
            }
        }

        throw new System.Exception("Wrong type, check your premade level");
    }

   



}
