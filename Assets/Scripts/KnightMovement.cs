using System;
using UnityEngine;


public class KnightMovement : MonoBehaviour
{
	public float speed = 10f;
	private Vector2 lastClickedPos;
	private Vector2 moveDirection;
	private bool moving;
	public static bool isShip;
	public static bool isUIClick = false;
	private Animator animator;

	private void Start()
	{
		//animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		float moveDirX = 0;
		float moveDirY = 0;

		if (Input.GetMouseButtonDown(0) && !isUIClick)
		{
			lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moving = true;

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

		if (moving && (Vector2)transform.position != lastClickedPos)
		{
			var step = speed * Time.deltaTime;
			Vector3 newPosition = new(lastClickedPos.x, lastClickedPos.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
		}
		else
		{
			moving = false;
		}

		//Animate();
	}

	private void Animate()
	{
		animator.SetBool("IsMoving", moving);
		animator.SetFloat("LastMoveX", moveDirection.x);
		animator.SetFloat("LastMoveY", moveDirection.y);
		animator.SetBool("IsShip", isShip);
	}
}

