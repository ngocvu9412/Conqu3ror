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

}


