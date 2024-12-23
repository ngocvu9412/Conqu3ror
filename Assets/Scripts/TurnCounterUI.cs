using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TurnCounterUI : Singleton<TurnCounterUI>
{
    [Header("UI References")]
    public RectTransform counterRect;  // RectTransform của toàn bộ UI Counter
    public Image rubyIcon;            // Hình ảnh của viên ruby
    public Image arrowIcon;           // Hình ảnh của mũi tên
    public Text turnCountText;        // Text hiển thị số lượt

    [Header("Settings")]
    public Color playerColor = Color.blue;   // Màu của lượt người chơi
    public Color enemyColor = Color.red;  // Màu của lượt đối thủ
    public float animationDuration = 0.5f; // Thời gian thực hiện animation

    private int currentTurnCount = 0;       // Số lượt hiện tại

    /// <summary>
    /// Cập nhật giao diện Turn Counter
    /// </summary>
    /// <param name="isPlayerTurn">Lượt hiện tại là của người chơi</param>
    /// <param name="turnCount">Số lượt còn lại</param>
    public void UpdateTurnCounter(bool isPlayerTurn, int turnCount)
    {
        // Cập nhật số lượt
        currentTurnCount = turnCount;
        if (turnCountText != null)
        {
            turnCountText.text = currentTurnCount.ToString();
        }

        // Đặt màu sắc
        Color targetColor = isPlayerTurn ? playerColor : enemyColor;

        // Xoay viên ruby 180 độ
        float targetRotation = isPlayerTurn ? -180 : 0;

        // Thực hiện hiệu ứng
        AnimateTurnCounter(targetColor, targetRotation);
    }

    /// <summary>
    /// Thực hiện animation bằng DoTween
    /// </summary>
    /// <param name="targetColor">Màu đích</param>
    /// <param name="targetRotation">Góc xoay đích</param>
    private void AnimateTurnCounter(Color targetColor, float targetRotation)
    {
        // Đổi màu viên ruby
        if (rubyIcon != null)
        {
            rubyIcon.DOColor(targetColor, animationDuration);
        }

        // Đổi màu mũi tên
        if (arrowIcon != null)
        {
            arrowIcon.DOColor(targetColor, animationDuration);
        }

        // Xoay viên ruby
        if (counterRect != null)
        {
            counterRect.DOLocalRotate(new Vector3(0, 0, targetRotation), animationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);
        }
    }
}

