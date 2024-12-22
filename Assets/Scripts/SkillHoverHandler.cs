using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject dialog; // Tham chiếu tới dialog
    public int skillIndex;    // Chỉ số của skill (0, 1, 2,...)
    public bool isPlayerSkill; // true: Kỹ năng của player, false: Kỹ năng của enemy

    private Coroutine showDialogCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Bắt đầu Coroutine hiển thị dialog sau 1 giây
        if (showDialogCoroutine == null)
        {
            showDialogCoroutine = StartCoroutine(ShowDialogWithDelay());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hủy Coroutine khi rời chuột
        if (showDialogCoroutine != null)
        {
            StopCoroutine(showDialogCoroutine);
            showDialogCoroutine = null;
        }

        // Ẩn dialog
        if (dialog != null)
        {
            dialog.SetActive(false);
        }
    }

    private IEnumerator ShowDialogWithDelay()
    {
        // Trì hoãn 0.3 giây
        yield return new WaitForSeconds(0.3f);

        // Lấy thông tin kỹ năng từ ShapeManager
        var shapesManager = ShapesManager.Ins;
        if (shapesManager != null)
        {
            var currentCharacter = isPlayerSkill ? shapesManager.playerCharacter : shapesManager.enemyCharacter;
            if (currentCharacter.Skills != null && skillIndex < currentCharacter.Skills.Count)
            {
                var skill = currentCharacter.Skills[skillIndex];

                // Hiển thị dialog với thông tin kỹ năng
                if (dialog != null)
                {
                    dialog.SetActive(true);
                    var dialogController = dialog.GetComponent<SkillDialogController>();
                    if (dialogController != null)
                    {
                        dialogController.SetSkillDetails(skill.Name, skill.Description, skill.Icon);
                    }
                }
            }
        }

        // Đặt Coroutine về null sau khi hoàn thành
        showDialogCoroutine = null;
    }
}
