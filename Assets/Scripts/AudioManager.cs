using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace


public class AudioManager : MonoBehaviour
{
    [SerializeField] Image musicOnIcon;
    [SerializeField] Image musicOffIcon;
    [SerializeField] TextMeshProUGUI musicOnText ;
    [SerializeField] TextMeshProUGUI musicOffText;
    private bool muted = false;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        } else
        {
            Load();
        }
        UpdateButton();
        AudioListener.pause = muted;
    }

    public void OnButtonPress()
    {
        if(muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
        Save();
        UpdateButton();
    }

    private void UpdateButton()
    {
        if(muted == false)
        {
            musicOnIcon.enabled = true;
            musicOnText.enabled = true;

            musicOffIcon.enabled = false;
            musicOffText.enabled = false;

        }
        else
        {
            musicOnIcon.enabled = false;
            musicOnText.enabled = false;

            musicOffIcon.enabled = true;
            musicOffText.enabled = true;
        }
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}
