using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButtonUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image image;
    [SerializeField] TMP_Text Level;
    void Start()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        image.sprite=GameDataManager.Ins.GetSelectedCharacter().Image;
        Level.text="Level "+GameDataManager.Ins.GetSelectedCharacter().Level.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
