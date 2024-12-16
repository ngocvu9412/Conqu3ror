using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInCombat
{
    public float MaxHealth;
    public float currentHealth;
    public float MaxAttack;
    public float CurrentAttack;
    public int Energy;
    public int CharacterIndex = 0;
    public static int time = 90;
    public int CurrentTime;


}