using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonControl : MonoBehaviour {

	private GameObject gameControl;
	private GameObject othelloPlayArea;
	private GameObject startButton;

	// Use this for initialization
	void Start () {
		this.gameControl = GameObject.Find ("GameControl");
		this.othelloPlayArea = GameObject.Find ("OthelloPlayArea");
		this.startButton = GameObject.Find ("StartButton");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickStart() {
		Debug.Log ("StartButtonControl Start OnClickStart");
		othelloPlayArea.GetComponent<OthelloPieces> ().InitPlayArea ();
		othelloPlayArea.GetComponent<OthelloPieces> ().SetFirstPieces ();
		othelloPlayArea.GetComponent<OthelloPieces> ().SetIsMyTurn (true);
		gameControl.GetComponent<MainControl> ().SetRoomNumber ("tempNumber");

		Invoke ("RemoveButton", 2.0f);
		Debug.Log ("StartButtonControl End OnClickStart");
	}

	public void RemoveButton() {
		// 対戦相手待ちの表示を一定時間表示
		startButton.SetActive (false);
	}
}
