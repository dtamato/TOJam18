using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PetNameCreator : MonoBehaviour {

	[SerializeField] string[] petNameArray;
	[SerializeField] Text petNameField;
	[SerializeField] GameObject submitButtonObject;

	void Awake () {

		submitButtonObject.SetActive (false);
	}

	public void NewPetName () {

		petNameField.text = petNameArray [Random.Range (0, petNameArray.Length)];
	}

	public void ShowSubmitButton () {

		submitButtonObject.SetActive (true);
	}
}
