using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BackgroundScroller : MonoBehaviour {

	[SerializeField] float startX;
	[SerializeField] float endX;
	[SerializeField] float moveSpeed = 1;

	RectTransform rectTransform;

	void Awake () {

		rectTransform = this.GetComponent<RectTransform> ();
	}

	void Start () {

		rectTransform.localPosition = new Vector3 (startX, rectTransform.localPosition.y, rectTransform.localPosition.z);
	}

	void Update () {
		
		rectTransform.localPosition = moveSpeed * Time.deltaTime * Vector3.right + rectTransform.localPosition;

		if (rectTransform.localPosition.x > endX) {

			rectTransform.localPosition = new Vector3 (startX, rectTransform.localPosition.y, rectTransform.localPosition.z);
		}
	}
}
