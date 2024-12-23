using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnCounterUI : Singleton<TurnCounterUI>
{
    public Image rubyIcon;            // Icon của viên ruby
    public Image arrowIcon;           // Icon của mũi tên
    public Text turnCountText;        // Hiển thị số lượt còn lại

    public Color playerColor;         // Màu của người chơi
    public Color enemyColor;          // Màu của đối thủ
    public float smoothDuration = 1f; // Thời gian mượt mà (xoay và đổi màu)

    private RectTransform rectTransform;

    override public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Cập nhật Turn Counter UI khi đổi lượt
    /// </summary>
    /// <param name="isPlayerTurn">true nếu là lượt của người chơi, false nếu là lượt của đối thủ</param>
    /// <param name="turnCount">Số lượt còn lại</param>
    private Coroutine transitionCoroutine;

    public void UpdateTurnCounter(bool isPlayerTurn, int turnCount)
    {
        // Cập nhật số lượt
        if (turnCountText != null)
        {
            turnCountText.text = turnCount.ToString();
        }

        // Dừng Coroutine trước đó
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        // Bắt đầu Coroutine mới
        Color targetColor = isPlayerTurn ? playerColor : enemyColor;
        float targetRotation = isPlayerTurn ? -180 : 0;

        transitionCoroutine = StartCoroutine(SmoothTransition(targetColor, targetRotation));
    }


    /// <summary>
    /// Coroutine xử lý hiệu ứng mượt mà cho đổi màu và xoay
    /// </summary>
    private IEnumerator SmoothTransition(Color targetColor, float targetRotation)
    {
        float elapsedTime = 0;
        Quaternion startRotation = rectTransform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, targetRotation);

        Color startRubyColor = rubyIcon.color;
        Color startArrowColor = arrowIcon.color;

        while (elapsedTime < smoothDuration)
        {
            float t = elapsedTime / smoothDuration;

            // Xoay dần
            rectTransform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);

            // Đổi màu dần
            if (rubyIcon != null)
                rubyIcon.color = Color.Lerp(startRubyColor, targetColor, t);
            if (arrowIcon != null)
                arrowIcon.color = Color.Lerp(startArrowColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo trạng thái cuối cùng là chính xác
        rectTransform.localRotation = endRotation;
        if (rubyIcon != null)
            rubyIcon.color = targetColor;
        if (arrowIcon != null)
            arrowIcon.color = targetColor;
       
    }
}
