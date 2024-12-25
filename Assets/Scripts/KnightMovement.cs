using System;
using System.Collections.Generic;
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

    private Rigidbody2D rb2d;
    private Animator animator;

    public Transform[] walkablePoints; // Danh sách các điểm hợp lệ trên đường đi
    private Transform currentWalkablePoint; // Vị trí hiện tại của nhân vật
    private int pointsIndex;
    public DialogController dialogManager; // Tham chiếu đến DialogManager

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        
        transform.position = walkablePoints[pointsIndex].transform.position;

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
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            // Kiểm tra xem điểm nhấn có thuộc danh sách điểm hợp lệ không
            if (IsPointWalkable(mouseWorldPos, out Transform targetPoint))
            {
                lastClickedPos = targetPoint.position; // Cập nhật vị trí mục tiêu hợp lệ
                isMoving = true;

                // Cập nhật hướng di chuyển
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
                Debug.Log("Điểm nhấn không hợp lệ!");
            }
        }
        else
        {
            isUIClick = false;
        }

        if (isMoving && (Vector2)transform.position != lastClickedPos)
        {
            var step = speed * Time.deltaTime;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, lastClickedPos, step);
            rb2d.MovePosition(newPosition);

            if (Vector2.Distance(transform.position, lastClickedPos) < 0.1f)
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }

        Animate();
    }

    private bool IsPointWalkable(Vector3 mousePos, out Transform walkablePoint)
    {
        walkablePoint = null;

        foreach (Transform point in walkablePoints)
        {
            if (Vector3.Distance(mousePos, point.position) < 0.5f) // Phạm vi hợp lệ là 0.5f
            {
                walkablePoint = point; // Gán điểm hợp lệ
                return true;
            }
        }

        return false; // Không tìm thấy điểm hợp lệ
    }


    //private void Update()
    //{
    //    if(pointsIndex <= walkablePoints.Length - 1)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, walkablePoints[pointsIndex].transform.position, speed * Time.deltaTime);

    //        if(transform.position == walkablePoints[pointsIndex].transform.position )
    //        {
    //            pointsIndex++;
    //        }
    //    }
    //}



    //private void LateUpdate()
    //{
    //    if (dialogManager.isOpened)
    //    {
    //        isActive = false;
    //        return;
    //    }

    //    float moveDirX = 0;
    //    float moveDirY = 0;

    //    if (Input.GetMouseButtonDown(0) && !isUIClick)
    //    {
    //        lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        isMoving = true;

    //        if (lastClickedPos.x > transform.position.x)
    //        {
    //            moveDirX = 1;
    //        }
    //        else if (lastClickedPos.x < transform.position.x)
    //        {
    //            moveDirX = -1;
    //        }

    //        if (Math.Abs(lastClickedPos.y - transform.position.y) <
    //            Math.Abs(lastClickedPos.x - transform.position.x))
    //        {
    //            moveDirY = 0;
    //        }
    //        else if (lastClickedPos.y > transform.position.y)
    //        {
    //            moveDirY = 1;
    //        }
    //        else if (lastClickedPos.y < transform.position.y)
    //        {
    //            moveDirY = -1;
    //        }

    //        moveDirection = new Vector2(moveDirX, moveDirY);
    //    }
    //    else
    //    {
    //        isUIClick = false;
    //    }

    //    if (isMoving && (Vector2)transform.position != lastClickedPos)
    //    {
    //        var step = speed * Time.deltaTime;
    //        Vector3 newPosition = new(lastClickedPos.x, lastClickedPos.y, transform.position.z);
    //        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    //        rb2d.MovePosition(transform.position);

    //        if (Vector2.Distance(transform.position, lastClickedPos) < 0.1f)
    //        {
    //            isMoving = false;

    //            // Kiểm tra nếu chạm đối tượng có tag "Red Point"
    //            Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f);
    //            if (hit != null && hit.CompareTag("RedPoint"))
    //            {
    //                dialogManager.ShowTextBox(); // Gọi sự kiện khi đến Red Point
    //            }
    //        }
    //    }
    //    else
    //    {
    //        isMoving = false;
    //    }

    //    Animate();
    //}


    //private void LateUpdate()
    //{
    //    if (!isActive || dialogManager.isOpened)
    //    {
    //        isMoving = false;
    //        return;
    //    }

    //    float moveDirX = 0;
    //    float moveDirY = 0;

    //    if (Input.GetMouseButtonDown(0) && !isUIClick)
    //    {
    //        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        mouseWorldPos.z = 0;

    //        // Kiểm tra nếu điểm nhấn thuộc một điểm hợp lệ
    //        if (IsOnRoad(mouseWorldPos))
    //        {
    //            lastClickedPos = mouseWorldPos; // Chỉ di chuyển đến điểm hợp lệ
    //            isMoving = true;

    //            // Cập nhật hướng di chuyển
    //            if (lastClickedPos.x > transform.position.x)
    //            {
    //                moveDirX = 1;
    //            }
    //            else if (lastClickedPos.x < transform.position.x)
    //            {
    //                moveDirX = -1;
    //            }

    //            if (Math.Abs(lastClickedPos.y - transform.position.y) <
    //                Math.Abs(lastClickedPos.x - transform.position.x))
    //            {
    //                moveDirY = 0;
    //            }
    //            else if (lastClickedPos.y > transform.position.y)
    //            {
    //                moveDirY = 1;
    //            }
    //            else if (lastClickedPos.y < transform.position.y)
    //            {
    //                moveDirY = -1;
    //            }
    //            Vector2 direction = lastClickedPos - (Vector2)transform.position;
    //            direction.Normalize();
    //            moveDirection = new Vector2(moveDirX, moveDirY);
    //        }

    //    }

    //    if (isMoving && (Vector2)transform.position != lastClickedPos)
    //    {
    //        var step = speed * Time.deltaTime;
    //        Vector3 newPosition = new(lastClickedPos.x, lastClickedPos.y, transform.position.z);
    //        transform.position = Vector2.MoveTowards(transform.position, newPosition, step);
    //        rb2d.MovePosition(transform.position);

    //        if (Vector2.Distance(transform.position, lastClickedPos) < 0.1f)
    //        {
    //            isMoving = false;
    //        }
    //    }
    //    else
    //    {
    //        isMoving = false;
    //    }

    //    Animate();
    //}

    private bool IsOnRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Road"))
        {
            return true; // Nếu asset có tag "Road", trả về true
        }

        return false; // Nếu không, trả về false
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

