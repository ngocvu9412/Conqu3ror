using UnityEngine;
using UnityEngine.UI;

public class GoldSharedUI : Singleton<GoldSharedUI>
{

	public static GoldSharedUI Instance {get;private set;}

	[SerializeField] Text[] coinsUIText;

	public override void Awake()
	{
		base.Awake();
	}
	public override void Start ()
	{
		UpdateCoinsUIText ();
	}

	public void UpdateCoinsUIText ()
	{
		for (int i = 0; i < coinsUIText.Length; i++) {
			SetCoinsText (coinsUIText [i], GameDataManager.Ins.GetCoins());
		}
	}

	public void SetCoinsText (Text textMesh, int value)
	{
		// if (value >= 1000000)...
		// .....

		if (value >= 1000)
			textMesh.text = string.Format ("{0}K.{1}", (value / 1000), GetFirstDigitFromNumber (value % 1000));
		else
			textMesh.text = value.ToString ();
	}

	int GetFirstDigitFromNumber (int num)
	{
		return int.Parse (num.ToString () [0].ToString ());
	}
}