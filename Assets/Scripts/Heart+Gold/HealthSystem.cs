using System;
using UnityEngine;
using TMPro;

public class HealthSystem : Singleton<HealthSystem>
{
    public float lifeRegenerateTime = 900f; // Thời gian hồi 1 mạng (30 phút)
    private DateTime lastSaveTime; // Thời gian lần cuối cùng sử dụng mạng
    [SerializeField] TMP_Text CurrentHealth;
    [SerializeField] TMP_Text Time;

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
        CurrentHealth.text = GameDataManager.Ins.GetCurrentHealthEner().ToString();
    }

    public void UseLife()
    {
        if (GameDataManager.Ins.GetCurrentHealthEner() > 0)
        {
            if (GameDataManager.Ins.GetCurrentHealthEner() == GameDataManager.Ins.GetMaxHealthEner())
            {
                lastSaveTime = DateTime.Now; // Lưu thời gian nếu sử dụng mạng khi đầy
            }
            GameDataManager.Ins.SetCurrentHealthEner(GameDataManager.Ins.GetCurrentHealthEner() - 1);
            UpdateUI();
        }
        else
        {
            Debug.Log("Không đủ mạng!");
        }
    }

    void Update()
    {
        UpdateHealthAndTime();
    }

    void UpdateHealthAndTime()
{
    if (GameDataManager.Ins.GetCurrentHealthEner() < GameDataManager.Ins.GetMaxHealthEner())
    {
        // Tính thời gian đã trôi qua kể từ lần cuối cùng sử dụng mạng/hồi máu
        float timePassed = (float)(DateTime.Now - lastSaveTime).TotalSeconds;

        // Hồi máu nếu đủ thời gian
        int livesToRegain = Mathf.FloorToInt(timePassed / lifeRegenerateTime);
        if (livesToRegain > 0)
        {
            GameDataManager.Ins.SetCurrentHealthEner(Mathf.Min(
            GameDataManager.Ins.GetCurrentHealthEner() + livesToRegain,
            GameDataManager.Ins.GetMaxHealthEner()));

            // Reset thời gian dư
            lastSaveTime = DateTime.Now.AddSeconds(-(timePassed % lifeRegenerateTime));
            UpdateUI(); // Cập nhật hiển thị số mạng
        }

        // Tính thời gian còn lại để hồi mạng tiếp theo
        float timeRemaining = lifeRegenerateTime - (timePassed % lifeRegenerateTime);

        // Định dạng thời gian dưới dạng phút:giây
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        string timeText = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

        // Hiển thị thời gian còn lại
        Time.text = timeText;
    }
    else
    {
        // Nếu máu đầy, hiển thị "Full"
        Time.text = "Full";
    }
}

}
