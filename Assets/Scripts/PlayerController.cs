using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed = 1;
	Vector3 destinationPosition;
	Animator moveAnimator;

	Rigidbody2D rb2d;

	SpriteRenderer rend;

	ParticleSystem borkPFX;

	public float numBork = 0.0f;
	float numBorkMax = 3.0f;
	bool borkCharge = true;

	float borkTimer = 0;
	float timeToBork = 0.5f;
	bool isChargeBork = false;

	void Start () {

		rb2d = this.GetComponent<Rigidbody2D> ();
		moveAnimator = this.GetComponent<Animator> ();
		destinationPosition = this.transform.position;
		rend = this.GetComponent<SpriteRenderer> ();
		borkPFX = this.GetComponentInChildren<ParticleSystem> ();

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

		//BORK
		//Main communication between dogs.
		//The player can still interact with items, but you need bork to other dogs
		// Your borks need to match, whether it's a smol bork or a big bork.
		// For now I just have this, but I hope to have the borks charge, and it mandatory to talk to dogs.

		if (Input.GetKey (KeyCode.Space)) { //Charging a Bork if space is held down for a period of time

			borkTimer += Time.deltaTime; 

			if (borkTimer > timeToBork) { //If the player holds the button for 1 second or so

				isChargeBork = true;
				ChargeBork ();

			}

		}

		if (Input.GetKeyUp (KeyCode.Space)) { //When the player lets go of Space

			Bork ();
			Debug.Log ("BORK");

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

	void MovePlayer() { //Player moves

		Vector3 directionVector = destinationPosition - this.transform.position;
		Vector2 velocity = moveSpeed * new Vector2 (directionVector.x, directionVector.y);

		rb2d.MovePosition (velocity * Time.fixedDeltaTime + rb2d.position);

		moveAnimator.SetBool ("isWalking", true);

		if (transform.position.x > destinationPosition.x) { //Flipping Sprite

			rend.flipX = true;

		} else {
			
			rend.flipX = false;
		}

	}

	void ChargeBork() { //Player is charging a bork
		// The player has to charge their bork to use it. This requires the player to not only hold space
		// But to also time their bork for maximum borkness. Otherwise a quick tap will let out a smol Bork.

		//The charge UI needs to enable here, and run off the numBork float.

		//Play a growl sound here that loops

		if (borkCharge) {

			numBork += Time.deltaTime;
			if (numBork > numBorkMax) {

				borkCharge = false; //Charge downwards here

			}

		} else if (!borkCharge) {

			numBork -= Time.deltaTime;
			if (numBork < 0) {

				borkCharge = true; //Charge upwards here

			}

		}

		Debug.Log (numBork);

	}

	void Bork() { //Player borks

		if (isChargeBork) {

			borkPFX.Play ();
			var borkSize = borkPFX.sizeOverLifetime;
			borkSize.sizeMultiplier = numBork;

			Color blue = new Color (0f, 1f, 1f, 1f);
			Color orange = new Color (1f, 0.7f, 0f, 1f);

			var pfxMain = borkPFX.main;
			pfxMain.startColor = Color.Lerp(blue, orange, numBork);

			isChargeBork = false;

		} else if (!isChargeBork) {

			borkPFX.Play ();
			var pfxMain = borkPFX.main;
			pfxMain.startColor = Color.white;

		}
			
		borkCharge = false;
		numBork = 0.0f;

		borkTimer = 0;

	}
}
