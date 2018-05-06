using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class TitleSceneController : MonoBehaviour {

	[SerializeField] Image titleImage;
	[SerializeField] float fadeInSpeed;
	[SerializeField] GameObject[] pawArray;
	[SerializeField] GameObject registerButton;

	bool canShowTitle = false;

	void Start () {

		titleImage.color = Color.clear;
		StartCoroutine (ShowPaws ());
	}

	void Update () {

		if (canShowTitle) {

			ShowTitle ();
		}
	}

	void ShowTitle () {

		if (titleImage.color.a <= 1) {

			float newAlpha = titleImage.color.a + fadeInSpeed * Time.deltaTime;

			titleImage.color = new Color (1, 1, 1, newAlpha);
		}
		else {

			registerButton.SetActive (true);
		}
	}

	IEnumerator ShowPaws () {

		yield return new WaitForSeconds (1f);

		float pawRevealDelay = 0.35f;

		for (int i = 0; i < pawArray.Length; i++) {

			pawArray [i].SetActive (true);
			yield return new WaitForSeconds (pawRevealDelay);

			if (pawArray [i].GetComponent<FadeOutPaw> ()) {
				
				pawArray [i].GetComponent<FadeOutPaw> ().StartFading ();
			}
			else {

				canShowTitle = true;
			}
		}
	}

	public void RegisterButton () {

		registerButton.GetComponent<AudioSource> ().Play ();
		StartCoroutine (LoadNextScene ());
	}

	IEnumerator LoadNextScene () {

		yield return new WaitForSeconds (registerButton.GetComponent<AudioSource> ().clip.length);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
		yield return null;
	}
}
