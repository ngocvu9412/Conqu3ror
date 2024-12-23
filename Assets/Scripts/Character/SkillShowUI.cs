using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShowUI : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemHeight;

    [Header("UI Elements")]
    [SerializeField] Transform SkillContainer;
    [SerializeField] GameObject SkillPrefab;
    List<Skill> Skills;   

    void Awake()
    {
        // Đảm bảo dữ liệu cần thiết được khởi tạo tại đây
        Skills = new List<Skill>();
    }

    void Start()
    {
        GenerateSkillUI();
        SetSkillInfo(GameDataManager.GetSelectedCharacterIndex());
    }

    void GenerateSkillUI()
    {
        // Lấy chiều cao của prefab
        itemHeight = SkillPrefab.GetComponent<RectTransform>().sizeDelta.y;

        // Tạo items trong UI
        for (int i = 0; i < 3; i++)
        {
            SkillUI uiItem = Instantiate(SkillPrefab, SkillContainer).GetComponent<SkillUI>();

            // Di chuyển item đến vị trí
            uiItem.SetItemPos(Vector2.down * (itemHeight + itemSpace) * i);
        }
    }

    public void SetSkillInfo(int index)
    {
        // Lấy danh sách kỹ năng dựa trên chỉ số nhân vật
        switch (index)
        {
            case 0:
                Skills = PhoenixSkills.GetSkills();
                break;
            case 1:
                Skills = MinervaSkills.GetSkills();
                break;
            case 2:
                Skills = MariusSkills.GetSkills();
                break;
            case 3:
                Skills = JasmineSkills.GetSkills();
                break;
        }

        // Cập nhật giao diện kỹ năng
        for (int i = 0; i < 3; i++)
        {
            SkillUI uiItem = SkillContainer.GetChild(i).GetComponent<SkillUI>();

            // Đặt tên cho item trong Hierarchy
            uiItem.gameObject.name = "Skill - " + Skills[i].Name;

            // Cập nhật thông tin kỹ năng
            uiItem.SetSkillName(Skills[i].Name);
            uiItem.SetSkillImage(Skills[i].Icon);
            uiItem.SetSkillDes(Skills[i].Description);
        }
    }
}
