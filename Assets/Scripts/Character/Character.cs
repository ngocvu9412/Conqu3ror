using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public Sprite Image;
    public string name;
    public float health;
    public float health_per_lv;
    public float attack;
    public float attack_per_lv;

    public int Exp;
    public static int LvlExp = 100;
    public static int LvlExpPerLv = 20;
    public int Level;
    public bool Unlocked;

    public float GetMaxHealth()
    {
        return (health+(health_per_lv*Level));
    }
    
    public float GetMaxAttack()
    {
        return (attack+(attack_per_lv*Level));
    }
    public int GetExpLevel()
    {
        return (LvlExp+LvlExpPerLv*Level);
    }
}
