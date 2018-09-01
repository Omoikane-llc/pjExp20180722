using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class BindGazeAction : MonoBehaviour, IFocusable {
	public GameObject othelloPlayArea;
	public GameObject selectButtons;
	public Material onFocusColor;

	private Color colorBuffer;

	// Use this for initialization
	void Start () {
		colorBuffer = gameObject.GetComponent<Renderer> ().material.color;
		this.othelloPlayArea = GameObject.Find ("OthelloPlayArea");
		this.selectButtons = GameObject.Find ("SelectButtons");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnFocusEnter() {
		if (OthelloPieces.isSelectLocked) {
			Debug.Log ("Not Active OnFocusEnter");
			return;
		}
		var isMyTurn = othelloPlayArea.GetComponent<OthelloPieces> ().GetIsMyTurn ();

		colorBuffer = gameObject.GetComponent<Renderer> ().material.color;
		gameObject.GetComponent<Renderer> ().material.color = onFocusColor.color;

		var currentSelected = OthelloPieces.currentSelected;
		Debug.Log ("currentSelected " + currentSelected);
		if (currentSelected == null || currentSelected.Length == 0 || isMyTurn) {
			OthelloPieces.currentSelected = GetCurrentName ();
		}

		if (isMyTurn) {
			OthelloPieces.isSelectLocked = true;
			othelloPlayArea.GetComponent<RotateControl>().StopRotate ();
			othelloPlayArea.GetComponent<SelectButtonsControl> ().ActivateButtons (true);
			var tempPosition = gameObject.transform.position;
			selectButtons.transform.position = new Vector3 (tempPosition.x, tempPosition.y, 2.0f);
		}

		Debug.Log ("End OnFocusEnter");
	}

	public void OnFocusExit() {
		if (OthelloPieces.isSelectLocked) {
			Debug.Log ("Not Active OnFocusExit");
			return;
		}
		Debug.Log ("Start OnFocusExit");
		gameObject.GetComponent<Renderer> ().material.color = colorBuffer;

		//Invoke ("RestoreRate", 2.0f);
		Debug.Log ("End OnFocusExit");
	}

	private string GetCurrentName() {
		var zColName = gameObject.name;
		var xRowName = gameObject.transform.parent.name;
		var layerName = gameObject.transform.parent.transform.parent.name;

		Debug.Log (layerName + "-" + xRowName + "-" + zColName);
		return layerName + "-" + xRowName + "-" + zColName;
	}
}
