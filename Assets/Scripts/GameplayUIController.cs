using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameplayUIController : Singleton<GameplayUIController>
{
    public Image myHealthBar;
    public Text myHealthText;
    public Image myEnergyBar;
    public Text myEnergyText;
    public Image myTimeBar;
    public Text myTimeText;
    public Text myCurrentAttack;
    public Image myCharacter;
    public Image mySkill1;
    public Image mySkill2;
    public Image mySkill3;

    public Image enemyHealthBar;
    public Text enemyHealthText;
    public Image enemyEnergyBar;
    public Text enemyEnergyText;
    public Image enemyTimeBar;
    public Text enemyTimeText;
    public Text enemyCurrentAttack;
    public Image enemyCharacter;
    public Image enemySkill1;
    public Image enemySkill2;
    public Image enemySkill3;

    public GameObject winDialog;
    public Image myCharAvatar;
    public Text myCharLevel;
    public Text myCharName;
    public Text myAtk;
    public Text myHealth;
    public Image myExpBar;
    public Text myExpText;
    public Text goldCollected;
    public Text expCollected;

    public GameObject loseDialog;
    public Text myCurrentLive;

    public RectTransform winDialogTransform; // RectTransform của WinDialog
    public RectTransform loseDialogTransform; // RectTransform của LoseDialog

    public Button skillButton1;
    public Button skillButton2;
    public Button skillButton3;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public override void Start()
    {
        InitializeSkillButtons();
    }

    public void InitializeSkillButtons()
    {
        if (skillButton1 != null)
        {
            skillButton1.onClick.RemoveAllListeners();
            skillButton1.onClick.AddListener(() => ShapesManager.Ins.UseSkill(0, ShapesManager.Ins.playerCharacter));
        }

        if (skillButton2 != null)
        {
            skillButton2.onClick.RemoveAllListeners();
            skillButton2.onClick.AddListener(() => ShapesManager.Ins.UseSkill(0, ShapesManager.Ins.playerCharacter));
        }

        if (skillButton3 != null)
        {
            skillButton3.onClick.RemoveAllListeners();
            skillButton3.onClick.AddListener(() => ShapesManager.Ins.UseSkill(0, ShapesManager.Ins.playerCharacter));
        }
    }

    public void ShowWinDialog()
    {
        if (winDialog != null && winDialogTransform != null)
        {
            winDialog.SetActive(true); // Kích hoạt dialog trước
            winDialogTransform.localScale = Vector3.zero; // Đặt kích thước ban đầu là 0
            winDialogTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true); // Tăng kích thước lên 1 với hiệu ứng mượt

            Time.timeScale = 0; // Dừng thời gian trong game
        }
    }

    public void ShowLoseDialog()
    {
        if (loseDialog != null && loseDialogTransform != null)
        {
            loseDialog.SetActive(true); // Kích hoạt dialog trước
            loseDialogTransform.localScale = Vector3.zero; // Đặt kích thước ban đầu là 0
            loseDialogTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true); // Tăng kích thước lên 1 với hiệu ứng mượt
            Time.timeScale = 0; // Dừng thời gian trong game
        }
        GameDataManager.Ins.SetCurrentHealthEner(GameDataManager.Ins.GetCurrentHealthEner() - 1);
    }

    public void HideWinDialog()
    {
        if (winDialog != null && winDialogTransform != null)
        {
            // Animation thu nhỏ dialog
            winDialogTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    winDialog.SetActive(false); // Ẩn hoàn toàn dialog sau khi animation kết thúc
                Time.timeScale = 1; // Tiếp tục thời gian
            });
        }
    }

    public void HideLoseDialog()
    {
        if (loseDialog != null && loseDialogTransform != null)
        {
            // Animation thu nhỏ dialog
            loseDialogTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    loseDialog.SetActive(false); // Ẩn hoàn toàn dialog sau khi animation kết thúc
                Time.timeScale = 1; // Tiếp tục thời gian
            });
        }
    }


    public void ReloadGame()
    {
        Time.timeScale = 1; // Đặt lại thời gian về bình thường
        HideLoseDialog();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload lại màn chơi
        ShapesManager.Ins.state = GameState.None;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Đặt lại thời gian về bình thường
        SceneManager.LoadScene("MainMenu"); // Chuyển tới màn hình chính
    }

    public void UpdateTime(bool isMyTurn, float curTime, float totalTime)
    {
        float rate = curTime / totalTime;

        if (isMyTurn)
        {
            if (myTimeBar)
                myTimeBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (myTimeText)
                myTimeText.text = Mathf.CeilToInt(curTime).ToString();
        }
        else
        {
            if (enemyTimeBar)
                enemyTimeBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (enemyTimeText)
                enemyTimeText.text = Mathf.CeilToInt(curTime).ToString();
        }
    }

    public void UpdateHealth(bool isMyTurn, int curHealth, int maxHealth)
    {
        float rate = (float)curHealth / maxHealth;

        if (isMyTurn)
        {
            if (myHealthBar)
                myHealthBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (myHealthText)
                myHealthText.text = $"{curHealth}/{maxHealth}";
        }
        else
        {
            if (enemyHealthBar)
                enemyHealthBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (enemyHealthText)
                enemyHealthText.text = $"{curHealth}/{maxHealth}";
        }
    }

    public void UpdateEnergy(bool isMyTurn, int curEnergy, int maxEnergy)
    {
        float rate = (float)curEnergy / maxEnergy;

        if (isMyTurn)
        {
            if (myEnergyBar)
                myEnergyBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (myEnergyText)
                myEnergyText.text = $"{curEnergy}/{maxEnergy}";
        }
        else
        {
            if (enemyEnergyBar)
                enemyEnergyBar.DOFillAmount(rate, 0.3f).SetEase(Ease.OutQuad); // Cập nhật thanh trạng thái mượt mà
            if (enemyEnergyText)
                enemyEnergyText.text = $"{curEnergy}/{maxEnergy}";
        }
    }

    public void UpdateAttack(bool isMyAtk, int curAtk)
    {
        if (isMyAtk)
        {
            if (myCurrentAttack)
                myCurrentAttack.text = curAtk.ToString();
        }
        else
        {
            if (enemyCurrentAttack)
                enemyCurrentAttack.text = curAtk.ToString();
        }
    }

    public void UpdateCharacter(bool isMyChar, Sprite image)
    {
        if (isMyChar)
        {
            if (myCharacter)
                myCharacter.sprite = image;
        }
        else
        {
            if (enemyCharacter)
                enemyCharacter.sprite = image;
        }
    }
    public void UpdateSkills(bool isMySkills, Sprite skill1, Sprite skill2, Sprite skill3)
    {
        if (isMySkills)
        {
            if (mySkill1)
                mySkill1.sprite = skill1;
            if (mySkill2)
                mySkill2.sprite = skill2;
            if (mySkill3)
                mySkill3.sprite = skill3;
        }
        else
        {
            if (enemySkill1)
                enemySkill1.sprite = skill1;
            if (enemySkill2)
                enemySkill2.sprite = skill2;
            if (enemySkill3)
                enemySkill3.sprite = skill3;
        }
    }

    public void UpdateWinDialog(Sprite charAvatar, string charName, int charLevel, int attack, int maxHealth, int curExp, int maxExp, int goldReward, int expReward)
    {
        if (winDialog != null)
        {
            // Activate the win dialog
            winDialog.SetActive(true);

            // Update character avatar
            if (myCharAvatar != null)
            {
                myCharAvatar.sprite = charAvatar;
            }

            // Update character name
            if (myCharName != null)
            {
                myCharName.text = charName;
            }

            // Update character level
            if (myCharLevel != null)
            {
                myCharLevel.text = charLevel.ToString();
            }

            // Update attack value
            if (myAtk != null)
            {
                myAtk.text = attack.ToString();
            }

            // Update health value
            if (myHealth != null)
            {
                myHealth.text = maxHealth.ToString();
            }

            // Update experience bar
            if (myExpBar != null)
            {
                myExpBar.fillAmount = (float)curExp / maxExp; // Expecting expRate as a value between 0 and 1
            }

            // Update experience text
            if (myExpText != null)
            {
                myExpText.text = curExp + "/" + maxExp; // Convert to percentage
            }

            // Update gold collected
            if (goldCollected != null)
            {
                goldCollected.text = goldReward.ToString();
            }

            // Update experience collected
            if (expCollected != null)
            {
                expCollected.text = expReward.ToString();
            }

            // Pause the game
            Time.timeScale = 0;

            ReduceExpWithDOTween(expReward, curExp, maxExp, charLevel, attack, maxHealth, goldReward);
        }
       
    }

    private void ReduceExpWithDOTween(int expReward, int currentExp, int maxExp, int charLevel, int attack, int maxHealth, int goldReward)
    {
        int displayedExp = expReward; // Giá trị hiển thị ban đầu
        int totalExp = currentExp + expReward; // Tổng kinh nghiệm sau khi nhận thưởng

        Debug.Log("Start updating experience with animation...");

        DOTween.To(() => displayedExp, x =>
        {
            displayedExp = x;
            if (expCollected != null)
            {
                expCollected.text = displayedExp.ToString();
            }

            // Đồng bộ tăng thanh kinh nghiệm
            UpdateExperienceBarSync(currentExp, expReward - displayedExp, maxExp, charLevel, attack, maxHealth);
        }, 0, 2f)
        .SetUpdate(true) // Đảm bảo hoạt động khi Time.timeScale = 0
        .OnComplete(() =>
        {
            Debug.Log("Exp reduced to 0, completing experience update...");
            GameDataManager.Ins.LevelUp(expReward);
            GameDataManager.Ins.AddCoins(goldReward);
        });
    }

    private void UpdateExperienceBarSync(int currentExp, int consumedExp, int maxExp, int charLevel, int attack, int maxHealth)
    {
        int totalExp = currentExp + consumedExp; // Tổng kinh nghiệm hiện tại

        // Xử lý logic lên cấp nếu vượt qua maxExp
        while (totalExp >= maxExp)
        {
            totalExp -= maxExp; // Trừ kinh nghiệm đã dùng để lên cấp
            charLevel++;        // Tăng cấp
            attack += Constants.atkPerLevel;
            maxHealth += Constants.healthPerLevel;
            maxExp += Constants.expPerLevel; // Tăng giá trị MaxExp (có thể tùy chỉnh)

            // Cập nhật hiển thị cấp độ
            if (myCharLevel != null)
            {
                myCharLevel.text = charLevel.ToString();
            }
            // Tăng chỉ số hiển thị khác như sức mạnh và máu tối đa
            if (myAtk != null)
            {
                myAtk.text = attack.ToString();
            }

            if (myHealth != null)
            {
                myHealth.text = maxHealth.ToString();
            }
        }

        // Cập nhật thanh kinh nghiệm
        if (myExpBar != null)
        {
            float fillAmount = (float)totalExp / maxExp;
            myExpBar.fillAmount = fillAmount;
        }

        // Cập nhật kinh nghiệm còn lại trên UI
        if (myExpText != null)
        {
            myExpText.text = totalExp + "/" + maxExp;
        }
    }






    public void UpdateLoseDialog(int curLive)
    {
        if (loseDialog != null)
        {
            if (myCurrentLive)
                myCurrentLive.text = curLive.ToString();
        }
    }

}


