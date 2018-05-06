using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfilePicSelector : MonoBehaviour {

	[SerializeField] GameObject profilePicSelector;
	[SerializeField] Image profilePictureRenderer;

	public void OpenProfilePicSelector () {

		profilePicSelector.SetActive (true);
	}

	public void ChangeProfilePicture () {

		Sprite newPicture = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ().sprite;
		profilePictureRenderer.sprite = newPicture;
		profilePicSelector.SetActive (false);
	}
}
