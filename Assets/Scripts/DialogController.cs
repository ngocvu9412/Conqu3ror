using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public UnityEngine.UI.Image dialogImage;
    public UnityEngine.UI.Image textboxImage;

    void Update()
    {
        // Kiểm tra nếu nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

            // Phát hiện collider tại vị trí nhấn
            if (hit.collider != null)
            {
                Debug.Log("Raycast trúng: " + hit.collider.gameObject.name);

                // Hiển thị dialog
                if (hit.collider.gameObject.CompareTag("RedPoint"))
                {
                    textboxImage.gameObject.SetActive(true);
                }
            }
        }
    }

    // Hàm hiển thị dialog
    public void ShowDialog()
    {
        HideTextbox();
        dialogImage.gameObject.SetActive(true);
    }

    // Gọi hàm này để ẩn dialog (nút Close hoặc sau khi hoàn thành hành động)
    public void HideDialog()
    {
        dialogImage.gameObject.SetActive(false);
    }

    public void HideTextbox()
    {
        textboxImage.gameObject.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
