﻿using System.Collections;
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
        //dialogueCanvas.gameObject.SetActive(false);

		rend = this.GetComponent<SpriteRenderer> ();

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
	
		yield return new WaitForSeconds (Random.Range (restTimeMin, restTimeMax));				

        resting = false;

        waypointIndex++;
        if (waypointIndex >= waypoints.Length) { waypointIndex = 0; }

    }
		
	void OnMouseDown()
	{	
		talking = !talking;

		if (talking) {
			if (interact) {
				dialogueCanvas.gameObject.SetActive (true);
			}

		}

		if (!talking) {	
			dialogueCanvas.gameObject.SetActive (false);
			resting = false;
		}



		//set the plaer waypoint to this doggo
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "Player") 
		{
			interact = true;
			rend.material.color = Color.cyan;

			//Debug.Log ("Collision Detected: " + other.gameObject.name);

		}
			
	}

	void OnTriggerExit2D(Collider2D other)
	{

		if (other.tag == "Player") { //Player leaves, dog goes back to normal, dialogue disappears

			interact = false;
			rend.material.color = Color.white;
			dialogueCanvas.gameObject.SetActive (false);
			resting = false;

		}

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
