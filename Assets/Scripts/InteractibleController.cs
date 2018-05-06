using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractibleController : MonoBehaviour {

	[SerializeField] Canvas popupCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown (){
		popupCanvas.gameObject.SetActive (!popupCanvas.gameObject.activeSelf);
	}
}
