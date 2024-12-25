using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField] Image CharacterImage;
    [SerializeField] Text CharacterName;
    [SerializeField] Text CharacterLevel;
    [SerializeField] Text CharacterHealth;
    [SerializeField] Text CharacterAttack;
    [SerializeField] int Exp;
    [SerializeField] Image ExpFill;
    [SerializeField] int LvlExp;
    [SerializeField] Text ExpText;
    [Space(20)]
    [SerializeField] CharactersDatabase CharacterDB;

    void Start()
    {
        ShowCharacterInfoUI(GameDataManager.Ins.GetSelectedCharacterIndex());
    }
    public void ShowCharacterInfoUI(int index)
    {
        Character characterInUse = CharacterDB.GetCharacter(index);

        // Gán thông tin vào UI
        CharacterImage.sprite = characterInUse.Image;
        CharacterName.text = characterInUse.name;
        CharacterLevel.text = "Lv: " + characterInUse.Level.ToString();
        CharacterHealth.text = characterInUse.health.ToString();
        CharacterAttack.text = characterInUse.attack.ToString();
        // Tính Exp
        Exp = characterInUse.Exp;
        LvlExp = characterInUse.MaxExp;
        // Cập nhật thanh Exp
        ExpFill.fillAmount = (float)Exp / LvlExp;
        // Cập nhật văn bản Exp
        ExpText.text = Exp + "/" + LvlExp;
    }

    void Update()
    {
        // Nếu cần cập nhật trong mỗi khung hình, thì đặt mã vào đây.
    }
}
