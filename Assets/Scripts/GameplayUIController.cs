using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : Singleton<GameplayUIController>
{
    public Image myHealthBar;
    public Text myHealthText;
    public Image myEnergyBar;
    public Text myEnergyText;
    public Image myTimeBar;
    public Text myTimeText;

    public Image enemyHealthBar;
    public Text enemyHealthText;
    public Image enemyEnergyBar;
    public Text enemyEnergyText;
    public Image enemyTimeBar;
    public Text enemyTimeText;

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
}
