using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgroundMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic);
        }
        else
        {
            Destroy(backgroundMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
