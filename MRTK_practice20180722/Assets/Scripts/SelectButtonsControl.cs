using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonsControl : MonoBehaviour {
	public GameObject selectButtons;
	private GameObject shield;
	private GameObject othelloPlayArea;

	// Use this for initialization
	void Start () {
		this.othelloPlayArea = GameObject.Find ("SelectButtons");
		this.shield = GameObject.Find ("Shield");
		this.othelloPlayArea = GameObject.Find ("OthelloPlayArea");
		selectButtons.SetActive (false);
		shield.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateButtons(bool isActive) {
		selectButtons.SetActive (isActive);
		shield.SetActive (isActive);
	}

	public void OnClickCancel() {
		Debug.Log ("SelectButtonsControl Start OnClickCancel");
		othelloPlayArea.GetComponent<RotateControl> ().RestoreRate ();
		OthelloPieces.isSelectLocked = false;
		Invoke ("RemoveButtons", 0.5f);
		Invoke ("RemoveShield", 5.0f);
		Debug.Log ("SelectButtonsControl End OnClickCancel");
	}

	public void OnClickDone() {
		Debug.Log ("SelectButtonsControl Start OnClickDone");

		var currentSelected = OthelloPieces.currentSelected;
		othelloPlayArea.GetComponent<OthelloPieces> ().PutNewPostion (currentSelected);
		othelloPlayArea.GetComponent<RotateControl> ().RestoreRate ();
		OthelloPieces.isSelectLocked = false;
		Invoke ("RemoveButtons", 0.5f);
		Invoke ("RemoveShield", 5.0f);
		Debug.Log ("SelectButtonsControl End OnClickDone " + currentSelected);
	}

	void RemoveButtons() {
		selectButtons.SetActive (false);
	}

	void RemoveShield() {
		shield.SetActive (false);
	}
}
