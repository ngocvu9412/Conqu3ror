using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame:MonoBehaviour
{
    public void StartAGame()
    {
        SceneManager.LoadScene("Main Game");
    }
}
