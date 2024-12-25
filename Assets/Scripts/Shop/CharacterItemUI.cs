using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] Image CharacterImage;
    [SerializeField] Text CharacterPrice;
    [SerializeField] GameObject CharacterPurchaseButton;
    public void SetItemPos (Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    public void SetCharacterImage (Sprite sprite)
    {
        CharacterImage.sprite = sprite;
    }

    public void SetCharacterPrice (int price)
    {
        if (price >= 1000) CharacterPrice.text = string.Format ("{0}K.{1}", (price / 1000), GetFirstDigitFromNumber (price % 1000));
		else
		CharacterPrice.text = price.ToString ();
    }
    int GetFirstDigitFromNumber (int num)
	{
		return int.Parse (num.ToString () [0].ToString ());
	}
    public void SetCharacterAsUnlocked()
    {
        CharacterPurchaseButton.SetActive(false);
    }
    public void OnItemPurchase (int index, UnityAction<int> action)
    {
        CharacterPurchaseButton.GetComponent<Button>().onClick.RemoveAllListeners ();
        CharacterPurchaseButton.GetComponent<Button>().onClick.AddListener (() => action.Invoke (index));
    }
}
