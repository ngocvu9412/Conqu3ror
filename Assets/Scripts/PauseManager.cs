using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // GameObject của Pause Panel
    public RectTransform pauseDialog; // RectTransform của dialog bên trong Panel
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
        if (pausePanel != null && pauseDialog != null)
        {
            pausePanel.SetActive(true); // Hiển thị panel

            // Hiển thị dialog với hiệu ứng trượt từ trên xuống
            pauseDialog.DOAnchorPos(Vector2.zero, 1f).From(new Vector2(0, 1000)).SetUpdate(true);

            Time.timeScale = 0; // Dừng thời gian trong game
            isGamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (pausePanel != null && pauseDialog != null)
        {
            // Ẩn dialog với hiệu ứng trượt ra khỏi màn hình
            pauseDialog.DOAnchorPos(new Vector2(0, 1000), 1f).SetUpdate(true).OnComplete(() =>
            {
                pausePanel.SetActive(false); // Tắt panel sau khi animation hoàn tất
            });

            Time.timeScale = 1; // Tiếp tục thời gian trong game
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
