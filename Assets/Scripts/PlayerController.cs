using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed = 1;
	Vector3 destinationPosition;
	Animator moveAnimator;

	Rigidbody2D rb2d;


	void Start () {

		rb2d = this.GetComponent<Rigidbody2D> ();
		moveAnimator = this.GetComponent<Animator> ();
		destinationPosition = this.transform.position;
	}

	void Update () {

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, 100)) {

				destinationPosition = hit.point;
				destinationPosition = new Vector3 (destinationPosition.x, destinationPosition.y, 0);
			}
				
		}
		else if (Input.GetMouseButtonDown (0)) {

			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

				destinationPosition = hit.point;
				destinationPosition = new Vector3 (destinationPosition.x, destinationPosition.y, 0);
			}
		}
	}

	void FixedUpdate () {

		float distance = Vector3.Distance (this.transform.position, destinationPosition);

		if (distance > 0.1f) {

			MovePlayer ();
		}
		else {

			moveAnimator.SetBool ("isWalking", false);
		}
	}

	void MovePlayer() {

		Vector3 directionVector = destinationPosition - this.transform.position;
		Vector2 velocity = moveSpeed * new Vector2 (directionVector.x, directionVector.y);

		rb2d.MovePosition (velocity * Time.fixedDeltaTime + rb2d.position);

		moveAnimator.SetBool ("isWalking", true);
	}
}
