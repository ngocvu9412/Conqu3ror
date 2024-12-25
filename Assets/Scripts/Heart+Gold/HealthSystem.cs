using System;
using UnityEngine;

public class HealthSystem : Singleton<HealthSystem>
{
    public float lifeRegenerateTime = 900f; // Thời gian hồi 1 mạng (30 phút)
    private DateTime lastSaveTime; // Thời gian lần cuối cùng đóng ứng dụng
    [SerializeField] GameObject Heart1;
    [SerializeField] GameObject Heart2;
    [SerializeField] GameObject Heart3;
    [SerializeField] GameObject Heart4;
    [SerializeField] GameObject Heart5;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        switch (GameDataManager.Ins.GetCurrentHealthEner())
        {
            case 0:
            {
                SetHeartActive(new bool[] { false, false, false, false, false });
            }
            break;
            case 1:
            {
                SetHeartActive(new bool[] { true, false, false, false, false });
            }
            break;
            case 2:
            {
                SetHeartActive(new bool[] { true, true, false, false, false });
            }
            break;
            case 3:
            {
                SetHeartActive(new bool[] { true, true, true, false, false });
            }
            break;
            case 4:
            {
                SetHeartActive(new bool[] { true, true, true, true, false });
            }
            break;
            case 5:
            {
                SetHeartActive(new bool[] { true, true, true, true, true });
            }
            break;
        }
    }
    void SetHeartActive(bool[] activeStatus)
    {
        Heart1.SetActive(activeStatus[0]);
        Heart2.SetActive(activeStatus[1]);
        Heart3.SetActive(activeStatus[2]);
        Heart4.SetActive(activeStatus[3]);
        Heart5.SetActive(activeStatus[4]);
    }
    public void UseLife()
    {
        if (GameDataManager.Ins.GetCurrentHealthEner() > 0)
        {
            if(GameDataManager.Ins.GetCurrentHealthEner() == 5)
            {
                lastSaveTime = DateTime.Now;
            }
            GameDataManager.Ins.SetCurrentHealthEner(GameDataManager.Ins.GetCurrentHealthEner()-1);
            UpdateUI();
        }
        else
        {
            Debug.Log("Không đủ mạng!");
        }
    }
    void Update()
    {
        RegenerateLives();
    }
    void RegenerateLives()
    {
        if (GameDataManager.Ins.GetCurrentHealthEner() < GameDataManager.Ins.GetMaxHealthEner())
        {
            float timePassed = (float)(DateTime.Now - lastSaveTime).TotalSeconds;

            int livesToRegain = Mathf.FloorToInt(timePassed / lifeRegenerateTime);
            if (livesToRegain > 0)
            {
                GameDataManager.Ins.SetCurrentHealthEner(Mathf.Min(GameDataManager.Ins.GetCurrentHealthEner() + livesToRegain, GameDataManager.Ins.GetMaxHealthEner())) ;
                lastSaveTime = DateTime.Now.AddSeconds(-(timePassed % lifeRegenerateTime));
                UpdateUI();
            }
        }
    }
}