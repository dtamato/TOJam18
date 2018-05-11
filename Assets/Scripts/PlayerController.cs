using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed = 1;
	Vector3 destinationPosition;
	Animator moveAnimator;

	[SerializeField] Canvas playerCanvas;
	[SerializeField] GameObject borkBar;
	[SerializeField] Image borkBarFill; 

	Rigidbody2D rb2d;

	SpriteRenderer rend;

	ParticleSystem borkPFX;

	public bool bork = false; //When the dog has borked
	public float sinceBork = 0.0f; // Timer after dog bork
	public float numBork = 0.0f; //charge of bork
	public float borkLvl; //storing of numBork
	float numBorkMax = 3.0f; //max charge of bork
	bool borkCharge = true; //when bork is charging up or down

	float borkTimer = 0; //Timer before dog bork
	float timeToBork = 0.5f; //max time before borkCharge
	bool isChargeBork = false; //when bork is charging

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

		}

		if (bork) { //Timer for Player Bork, helps reset bork boolean for NPC Interaction

			sinceBork += Time.deltaTime;

			if (sinceBork > 1.0f) {

				bork = false;
				sinceBork = 0.0f;

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

		playerCanvas.gameObject.SetActive (true);
		// The player has to charge their bork to use it. This requires the player to not only hold space
		// But to also time their bork for maximum borkness. Otherwise a quick tap will let out a smol Bork.

		//The charge UI needs to enable here, and run off the numBork float.

		//Play a growl sound here that loops

		if (borkCharge) {

			numBork += Time.deltaTime;
			borkBarFill.fillAmount = numBork / numBorkMax;

			if (numBork > numBorkMax) {

				borkCharge = false; //Charge downwards here

			}

		} else if (!borkCharge) {
			
			numBork -= Time.deltaTime;
			borkBarFill.fillAmount = numBork / numBorkMax;
			if (numBork < 0) {

				borkCharge = true; //Charge upwards here

			}

		}

		//Debug.Log ("BORK = " + numBork);

	}

	void Bork() { //Player borks

		if (isChargeBork) {

			bork = true;
			borkLvl = numBork;

			playerCanvas.gameObject.SetActive (false);
			borkPFX.Play ();
			var borkSize = borkPFX.sizeOverLifetime;
			borkSize.sizeMultiplier = numBork;

			Color blue = new Color (0f, 1f, 1f, 1f);
			Color red = new Color (1f, 0f, 0f, 1f);

			var pfxMain = borkPFX.main;
			pfxMain.startColor = Color.Lerp(blue, red, (numBork/3));

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
