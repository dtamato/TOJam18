using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed = 1;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {

		rb2d = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {

		PlayerMovement ();
	}

	void PlayerMovement()
	{
		Vector2 moveVector = Vector2.zero;

		if (Input.GetAxis("Horizontal") < -0.1f)
		{
			moveVector += Vector2.left;

			if (this.transform.localScale.x < 0)
			{
				this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			}
		}
		else if (Input.GetAxis("Horizontal") > 0.1f)
		{
			moveVector += Vector2.right;

			if (this.transform.localScale.x > 0)
			{
				this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			}
		}

		if (Input.GetAxis("Vertical") > 0.1f)
		{
			moveVector += Vector2.up;
		}
		else if(Input.GetAxis("Vertical") < -0.1f)
		{
			moveVector += Vector2.down;
		}

//		if (moveVector != Vector2.zero)
//		{
//			foreach (Animator animator in playerAnimators)
//			{
//				animator.SetBool("isWalking", true);
//			}
			rb2d.MovePosition(rb2d.position + moveSpeed * moveVector * Time.deltaTime);
//		}
//		else
//		{
//			foreach (Animator animator in playerAnimators)
//			{
//				animator.SetBool("isWalking", false);
//			}
//		}
	}
}
