using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanel : MonoBehaviour {

	[SerializeField] GameObject creditsPanel;


	public void CloseCreditsPanel () {

		creditsPanel.SetActive (false);
	}
}
