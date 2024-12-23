using UnityEngine;

[System.Serializable]
public struct Equipment 
{
    public string SpriteName;
    public string name;
    public string description;
    public int price;
    public int Level;
    public string StatTypeEffect;
    public float EffectStat;
    
    public bool IsUsing;
    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Equipment/"+SpriteName); // hoặc cách khác nếu bạn lưu sprite theo cách khác
    }
    public void SetUsedStatus ( bool UsedStatus )
    {
        IsUsing = UsedStatus;
    }
    public string GetStatTypeEffect()
    {
        return StatTypeEffect;
    }
    public float GetEffectStat()
    {
        return EffectStat;
    }
}
