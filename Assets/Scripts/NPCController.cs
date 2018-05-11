using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class NPCController : MonoBehaviour {

    [Header("Navigation")]
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float restTimeMin = 3;
    [SerializeField] float restTimeMax = 10;
    [SerializeField] float distanceRequired = 0.5f;
    [SerializeField] GameObject[] waypoints;
    int waypointIndex = 0;
    bool resting = false;
	bool talking = false;
	bool interact = false;

	public GameObject playerGO;
	public PlayerController playerScript;

	public float dogType;
	bool fright = false;
	bool bamboozled = false;
	float statusTimer = 0.0f;

	[SerializeField] float numBork = 1.5f; //charge of bork
	bool fren = false;

	ParticleSystem borkPFX;

    [Header("Dialogue")]
    [SerializeField] Canvas dialogueCanvas;
   // [SerializeField] GameObject NPCEmote;
   // [SerializeField] string[] fartReactionArray;
   // [SerializeField] string[] playerCalloutArray;

    Rigidbody2D rb2d;
	SpriteRenderer rend;


	// Use this for initialization
	void Start () {

        rb2d = this.transform.GetComponentInChildren<Rigidbody2D>();
		rend = this.GetComponent<SpriteRenderer> ();
		borkPFX = this.GetComponentInChildren<ParticleSystem> ();
		playerScript = playerGO.GetComponent<PlayerController> ();

        if (waypoints.Length == 0) { resting = true; }
        else { this.transform.position = waypoints[waypointIndex].transform.position; }
	}
	
	// Update is called once per frame
	void Update () {

		if (talking) {resting = true;}


        if(!resting)
        {
            // Check how far the NPC is to their destination waypoint
            float distance = Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position);
            Vector2 moveVector = waypoints[waypointIndex].transform.position - this.transform.position;

            // Flip sprite if necessary
            if((waypoints[waypointIndex].transform.position.x > this.transform.position.x && this.transform.localScale.x > 0) ||
                (waypoints[waypointIndex].transform.position.x < this.transform.position.x && this.transform.localScale.x < 0))
           {
              this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

				dialogueCanvas.GetComponentInChildren<RectTransform>().transform.localScale = new Vector3(-dialogueCanvas.GetComponentInChildren<RectTransform>().transform.localScale.x, dialogueCanvas.GetComponentInChildren<RectTransform>().transform.localScale.y, dialogueCanvas.GetComponentInChildren<RectTransform>().transform.localScale.z);
           }
        
            if(distance > distanceRequired)
            {
                rb2d.MovePosition(rb2d.position + moveSpeed * moveVector.normalized * Time.deltaTime);
				this.GetComponent<Animator> ().SetBool ("isWalking", true);
            }

            else if(distance < distanceRequired)
            {
				
                StartCoroutine(WaitAtWaypoint());
				this.GetComponent<Animator> ().SetBool ("isWalking", false);
            }
				
        }

		//*DOG BORK INTERACTION*//

		if (interact && !fren) { //If Player is interacting with dog with enterTrigger2D

			if (playerScript.bork) {

				if (dogType == 0) { //Dog is Submissive, need to bork less than a certain amount

					if (playerScript.borkLvl < numBork) {

						Debug.Log ("FREN");
						fren = true;

					} else if (playerScript.borkLvl > numBork) { // Doin the dog a frighten

						Debug.Log ("FRIGHTEN");
						fright = true;

						//Set a timer for the dog to calm down.

					}
						

				} else if (dogType == 1) { //Dog is Neutral, need to match bork to be fren, otherwise bambooze/frighten

					if (Mathf.Round(playerScript.borkLvl) == Mathf.Round(numBork)) { //Rounding bork nums

						Debug.Log ("FREN");
						fren = true;

					} //These dogs can be frightened and/or bamboozled... but iunno how to implement.

				} else if (dogType == 2) { //Dog is Dominant, need to bork more than numBork

					if (playerScript.borkLvl > numBork) {

						Debug.Log ("FREN");
						fren = true;


					} else if (playerScript.borkLvl < numBork) { // Doin the dog a frighten

						Debug.Log ("BAMBOOZLE");
						bamboozled = true;

						//Set a timer for the dog to calm down.

					}

				}
					
			}

		}

		//FREN, PROFILE SHOWING/DIALOGUE

		if (fren) { // Player befriends dog through Bork. Placeholder turn dog yellow to show progress

			rend.material.color = Color.yellow;

		}

		//BAMBOOZLE, Player confuses domninant and neutral dogs

		if (bamboozled) {

			rend.material.color = Color.grey;
			statusTimer += Time.deltaTime;
			if (statusTimer > 5f) {

				rend.material.color = Color.white;
				bamboozled = false;
				statusTimer = 0.0f;

			}

		}

		//FRIGHTEN, Player scares submissive and neutral dogs

		if (fright) {

			rend.material.color = Color.red;
			statusTimer += Time.deltaTime;
			if (statusTimer > 5f) {

				rend.material.color = Color.white;
				fright = false;
				statusTimer = 0.0f;

			}

		}

	}

   //void GetAngry()
   //{
   //    NPCEmote.GetComponent<Animator>().SetBool("IsAngry", true);
   //    StartCoroutine(CalmDown());
   //}
   //
   //IEnumerator CalmDown()
   //{
   //    yield return new WaitForSeconds(5);
   //    NPCEmote.GetComponent<Animator>().SetBool("IsAngry", false);
   //}

	IEnumerator WaitAtWaypoint ()
    {
        resting = true;
	
		if (!interact) {

			yield return new WaitForSeconds (Random.Range (restTimeMin, restTimeMax));				

			resting = false;

		}

        waypointIndex++;
        if (waypointIndex >= waypoints.Length) { waypointIndex = 0; }

    }
		
	// Instead of clicking dogs for dialogue, the player needs to befren them first
	// Frening happens when the player borks to the dog, and matches certain requirements
	// When the player approaches a dog, the dog will BORK. The player either has to submit, dominate, or match the bork.
	//


	void OnMouseDown() //Clicking on Dog to communicate
	{	
		if (fren) {

			talking = !talking;

			if (talking) {
				if (interact) { // When Player is nearby
					dialogueCanvas.gameObject.SetActive (true);
				}

			}

			if (!talking) {	
				dialogueCanvas.gameObject.SetActive (false);
				resting = false;
			}

		}
			
	}

	void OnTriggerEnter2D(Collider2D other) //When Player is within the zone
	{

		if (other.tag == "Player") 
		{

			if (!fren && (!bamboozled || !fright)) {

				interact = true;
				rend.material.color = Color.cyan;
				Bork ();

				//Debug.Log ("Collision Detected: " + other.gameObject.name);

			}

		}
			
	}

	void OnTriggerExit2D(Collider2D other)
	{

		if (other.tag == "Player") { //Player leaves, dog goes back to normal, dialogue disappears

			if (!fren && (!bamboozled || !fright)) {

				interact = false;
				rend.material.color = Color.white;
				dialogueCanvas.gameObject.SetActive (false);
				resting = false;

			}

		}

	}	
		

	void Bork() { //NPC Bork, defined by numBork of NPC and numBork of player. 

		borkPFX.Play ();
		var borkSize = borkPFX.sizeOverLifetime;
		borkSize.sizeMultiplier = numBork;

		Color blue = new Color (0f, 1f, 1f, 1f);
		Color red = new Color (1f, 0f, 0f, 1f);

		var pfxMain = borkPFX.main;
		pfxMain.startColor = Color.Lerp(blue, red, (numBork/3));

	}

  //  public void SmellFart ()
  //  {
  //      GetAngry();
  //      dialogueCanvas.gameObject.SetActive(true);
  //      dialogueCanvas.GetComponentInChildren<Text>().text = fartReactionArray[Random.Range(0, fartReactionArray.Length)];
  //      StartCoroutine(HideDialogueBox());
  //  }
  //
  //  public void SayHi()
  //  {
  //      dialogueCanvas.gameObject.SetActive(true);
  //      dialogueCanvas.GetComponentInChildren<Text>().text = playerCalloutArray[Random.Range(0, playerCalloutArray.Length)];
  //      StartCoroutine(HideDialogueBox());
  //  }
  //
  //  IEnumerator HideDialogueBox ()
  //  {
  //      yield return new WaitForSeconds(3);
  //      dialogueCanvas.gameObject.SetActive(false);
  //  }
}
