using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void ShowWinDialog()
    {
        if (winDialog != null)
        {
            winDialog.SetActive(true); // Hiển thị dialog thắng
            Time.timeScale = 0;        // Dừng thời gian trong game
        }
    }
    public void ShowLoseDialog()
    {
        if (loseDialog != null)
        {
            loseDialog.SetActive(true); // Hiển thị dialog thua
            Time.timeScale = 0;         // Dừng thời gian trong game
        }
    }

    public void ReloadGame()
    {
        Time.timeScale = 1; // Đặt lại thời gian về bình thường
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload lại màn chơi
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
                myTimeBar.fillAmount = rate;
            if (myTimeText)
                myTimeText.text = curTime.ToString();
        }
        else
        {
            if (enemyTimeBar)
                enemyTimeBar.fillAmount = rate;
            if (enemyTimeText)
                enemyTimeText.text = curTime.ToString();
        }
    }

    public void UpdateHealth(bool isMyTurn, int curHealth, int maxHealth)
    {
        float rate = (float)curHealth / (float)maxHealth;
        if (isMyTurn)
        {
            if (myHealthBar)
                myHealthBar.fillAmount = rate;
            if (myHealthText)
                myHealthText.text = curHealth + "/" + maxHealth;
        }
        else
        {
            if (enemyHealthBar)
                enemyHealthBar.fillAmount = rate;
            if(enemyHealthText)
                enemyHealthText.text = curHealth + "/" + maxHealth;

        }
    }

    public void UpdateEnergy(bool isMyTurn, int curEnergy, int maxEnergy)
    {
        float rate = (float)curEnergy / (float)maxEnergy;
        if (isMyTurn)
        {
            if (myEnergyBar)
                myEnergyBar.fillAmount = rate;
            if (myEnergyText)
                myEnergyText.text = curEnergy + "/" + maxEnergy;
        }
        else
        {
            if (enemyEnergyBar)
                enemyEnergyBar.fillAmount = rate;
            if (enemyEnergyText)
                enemyEnergyText.text = curEnergy + "/" + maxEnergy;
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


