using UnityEngine;

public class PointInteraction : MonoBehaviour
{
    public GameObject dialogBox; // Hộp thoại (UI) hiển thị thông báo
    public UnityEngine.UI.Text dialogText; // Văn bản hiển thị trong hộp thoại

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
                // Kiểm tra nếu là điểm đỏ
                if (hit.collider.CompareTag("RedPoint"))
                {
                    ShowDialog("Chiến đấu!");
                }
                // Kiểm tra nếu là điểm xanh
                else if (hit.collider.CompareTag("GreenPoint"))
                {
                    ShowDialog("Đã qua ải!");
                }
            }
        }
    }

    // Hàm hiển thị dialog
    void ShowDialog(string message)
    {
        dialogBox.SetActive(true); // Bật hộp thoại
        dialogText.text = message; // Cập nhật nội dung
    }

    // Gọi hàm này để ẩn dialog (nút Close hoặc sau khi hoàn thành hành động)
    public void HideDialog()
    {
        dialogBox.SetActive(false); // Tắt hộp thoại
    }
}
