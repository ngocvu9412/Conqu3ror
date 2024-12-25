using System;
using UnityEngine;


public class KnightMovement : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 lastClickedPos;
    private Vector2 moveDirection;
    public bool isMoving = false;
    public bool isActive = true;
    public static bool isShip;
    public static bool isUIClick = false;

    private Animator animator;

    public DialogController dialogManager; // Tham chiếu đến DialogManager

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (dialogManager.isOpened)
        {
            isActive = false;
            return;
        }

        float moveDirX = 0;
        float moveDirY = 0;

        if (Input.GetMouseButtonDown(0) && !isUIClick)
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;

            if (lastClickedPos.x > transform.position.x)
            {
                moveDirX = 1;
            }
            else if (lastClickedPos.x < transform.position.x)
            {
                moveDirX = -1;
            }

            if (Math.Abs(lastClickedPos.y - transform.position.y) <
                Math.Abs(lastClickedPos.x - transform.position.x))
            {
                moveDirY = 0;
            }
            else if (lastClickedPos.y > transform.position.y)
            {
                moveDirY = 1;
            }
            else if (lastClickedPos.y < transform.position.y)
            {
                moveDirY = -1;
            }

            moveDirection = new Vector2(moveDirX, moveDirY);
        }
        else
        {
            isUIClick = false;
        }

        if (isMoving && (Vector2)transform.position != lastClickedPos)
        {
            var step = speed * Time.deltaTime;
            Vector3 newPosition = new(lastClickedPos.x, lastClickedPos.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

            //if (Vector2.Distance(transform.position, lastClickedPos) < 0.1f)
            //{
            //    isMoving = false;

            //    // Kiểm tra nếu chạm đối tượng có tag "Red Point"
            //    Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f);
            //    if (hit != null && hit.CompareTag("RedPoint"))
            //    {
            //        dialogManager.ShowTextBox(); // Gọi sự kiện khi đến Red Point
            //    }
            //}
        }
        else
        {
            isMoving = false;
        }

        Animate();
        Debug.Log(moveDirection.x);
        Debug.Log(moveDirection.y);
    }

    private void Animate()
    {
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
    }

    public void EnableCharacter()
    {
        isActive = true; // Kích hoạt nhân vật
    }

    public void DisableCharacter()
    {
        isActive = false; // Vô hiệu hóa nhân vật
        isMoving = false; // Ngừng mọi chuyển động
    }


}

