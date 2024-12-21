using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public GameObject dialogBox1; // Hộp thoại (UI) hiển thị thông báo
    public GameObject dialogBox2; // Hộp thoại (UI) hiển thị thông báo

    public UnityEngine.UI.Image dialogImage;

    void Update()
    {
        // Kiểm tra nếu nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Phát hiện collider tại vị trí nhấn
            if (Physics.Raycast(ray, out hit))
            {
                dialogImage.gameObject.SetActive(true);
                // Kiểm tra nếu là điểm đỏ
                if (hit.collider.CompareTag("Red Point"))
                {
                    ShowDialog(dialogBox1);
                }
                // Kiểm tra nếu là điểm xanh
                else if (hit.collider.CompareTag("Green Point"))
                {
                    ShowDialog(dialogBox2);
                }
            }
        }
    }

    // Hàm hiển thị dialog
    void ShowDialog(GameObject dialog)
    {
        dialog.SetActive(true); // Bật hộp thoại
    }

    // Gọi hàm này để ẩn dialog (nút Close hoặc sau khi hoàn thành hành động)
    public void HideDialog(GameObject dialog)
    {
        dialogImage.gameObject.SetActive(false);
        dialog.SetActive(false); // Tắt hộp thoại
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
     
}
