using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using CI.HttpClient;
using System;

public class HttpControl : MonoBehaviour {
	[SerializeField]
	private string serverURL = "";

	// Use this for initialization
	void Start () {
		Debug.Log ("HttpControl Start ");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string PostJsonForUpdate(string url, string jsonString, OthelloPieces othelloPieces) {
		//Debug.Log ("HttpControl Start PostJson ");
		string result = "";
		if (String.IsNullOrEmpty (url)) {
			url = this.serverURL;
		}

		try {
			HttpClient client = new HttpClient();
			Uri reqUri = new Uri(url);

			StringContent stringContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

			client.Post(reqUri, stringContent, HttpCompletionOption.AllResponseContent, (r) =>
				{
					try{
						//Debug.Log("Start callback");
						//Debug.Log("r.StatusCode " + r.StatusCode);
						result = System.Text.Encoding.UTF8.GetString(r.Data);

						var data = JsonUtility.FromJson<JsonCarrier>(result);
						//Debug.Log("data.pieacesState[0] " + data.pieacesState[0]);
						othelloPieces.SetState(data);
						//Debug.Log("End callback");
					}catch(Exception ex){
						Debug.LogWarning ("Fail callback \n" + StackTraceUtility.ExtractStringFromException(ex));
					}

				});
		}catch (Exception ex) {
			Debug.LogWarning ("Fail PostJson \n" + StackTraceUtility.ExtractStringFromException(ex));
		}

		//Debug.Log ("HttpControl End PostJson " + result);
		return result;
	}
}
