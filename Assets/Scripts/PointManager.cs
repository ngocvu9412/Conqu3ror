using UnityEngine;

public class PointManager : MonoBehaviour
{
    public GameObject redPoint; // Asset điểm đỏ
    public int pointIndex;
    public PointInfosDatabase pointInfo; // Thông tin trạng thái của điểm

    private void Start()
    {
        UpdatePointState(); // Kiểm tra và cập nhật trạng thái khi bắt đầu
    }

    public void UpdatePointState()
    {
        if (pointInfo.GetPointInfos(pointIndex).status) // Nếu đã thắng
        {
            redPoint.SetActive(false); // Ẩn Red Point
        }
    }

    public void SetPointState()
    {
        pointInfo.GetPointInfos(pointIndex).status = false; // Cập nhật trạng thái trong PointInfo
        UpdatePointState(); // Gọi lại hàm để cập nhật hiển thị
    }
}
