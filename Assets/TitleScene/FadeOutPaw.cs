using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FadeOutPaw : MonoBehaviour {

	[SerializeField] float fadeSpeed;
	bool isFading = false;
	Image pawImage;

	void Awake () {

		pawImage = this.GetComponent<Image> ();
	}

	public void StartFading () {

		isFading = true;
	}

	void Update () {

		if (isFading) {

			if (pawImage.color.a > 0) {

				float newAlpha = pawImage.color.a - fadeSpeed * Time.deltaTime;
				pawImage.color = new Color (1, 1, 1, newAlpha);
			}
			else {

				isFading = false;
			}
		}
	}
}
