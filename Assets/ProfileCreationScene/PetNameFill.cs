using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PetNameFill : MonoBehaviour {

	[SerializeField] float fillSpeed;
	[SerializeField] Text titleText;

	Image fillImage;
	bool isFilling;

	void Awake () {

		fillImage = this.GetComponent<Image> ();

	}

	void Start () {

		fillImage.fillAmount = 0;
	}

	void Update () {

		if (isFilling) {

			fillImage.fillAmount += fillSpeed * Time.deltaTime;
			titleText.text = "Your owner calls you...";
			this.GetComponentInParent<PetNameCreator> ().NewPetName ();
		}

		if (fillImage.fillAmount >= 1) {

			this.GetComponentInParent<PetNameCreator> ().NewPetName ();
			this.GetComponentInParent<PetNameCreator> ().ShowSubmitButton ();
			titleText.text = "Pet Name";
			fillImage.fillAmount = 0;
			isFilling = false;
		}
	}

	public void GeneratePetName () {

		isFilling = true;
	}
}
