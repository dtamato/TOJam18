using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ProfileCreationController : MonoBehaviour {
	
	public void LoadNextScene () {

		//SceneManager.LoadScene (0);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
