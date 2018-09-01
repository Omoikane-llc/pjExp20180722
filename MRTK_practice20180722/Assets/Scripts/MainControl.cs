using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainControl : MonoBehaviour {
	public GameObject othelloPlayArea;

	[SerializeField]
	private float updateInterval;

	private string roomNumber;
	private OthelloPieces othelloPieces;

	void Start() {
		Debug.Log ("MainControl Start ");
		othelloPieces = othelloPlayArea.transform.gameObject.GetComponent<OthelloPieces> ();
		StartCoroutine( UpdateStatusCoroutine() );
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator UpdateStatusCoroutine() {
		while(true){
			// Do anything
			Debug.Log("UpdateStatusCoroutine");
			var JsonObj = othelloPieces.GetState ();
			JsonObj.roomNumber = this.roomNumber;
			string jsonString = JsonUtility.ToJson(JsonObj);
			//Debug.Log("jsonString " + jsonString);

			HttpControl obj = this.transform.gameObject.GetComponent<HttpControl> ();
			obj.PostJsonForUpdate (null, jsonString, othelloPieces);
			//yield return PostJsonCoroutine (null, jsonString);

			yield return new WaitForSeconds(updateInterval);
		}
	}

	public void SetRoomNumber(string roomNumber) {
		this.roomNumber = roomNumber;
	}

}
