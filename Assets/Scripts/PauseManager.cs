using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Tham chiếu tới Panel Pause
    private bool isGamePaused = false;

    void Start()
    {
        // Đảm bảo panel bị ẩn khi bắt đầu
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true); // Hiển thị panel
            Time.timeScale = 0;        // Dừng thời gian trong game
            isGamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Ẩn panel
            Time.timeScale = 1;          // Tiếp tục thời gian
            isGamePaused = false;
        }
    }

    public void Surrender()
    {
        Debug.Log("Player surrendered!");
        // Thực hiện logic đầu hàng, ví dụ quay về màn hình chính:
        Time.timeScale = 1; // Reset lại thời gian trước khi chuyển cảnh
        pausePanel.SetActive(false);
        SceneManager.LoadScene("Map"); // Kích hoạt nếu dùng Scene
    }
}
