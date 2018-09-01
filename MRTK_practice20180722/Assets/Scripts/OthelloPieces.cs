using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloPieces : MonoBehaviour {

	public Material baseColor;
	public Material player1Color;
	public Material player2Color;
	public GameObject turnOverEffect1;
	public GameObject turnOverEffect2;

	[HideInInspector]
	public static string currentSelected;

	[HideInInspector]
	public static bool isSelectLocked;


	private GameObject gameControl;
	private string isMyTurn;
	private Dictionary<string,PieceState> pieces;
	private static string[] colorNames;

	// Use this for initialization
	void Start () {
		Debug.Log ("OthelloPieces Start Start");
		this.gameControl = GameObject.Find ("GameControl");
		colorNames = new string[]{this.baseColor.name,this.player1Color.name,this.player2Color.name };
		InitPlayArea ();
		SetFirstPieces ();

		Debug.Log ("OthelloPieces End Start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitPlayArea() {
		Debug.Log ("OthelloPieces Start InitPlayArea ");

		isMyTurn = "false";
		isSelectLocked = false;
		pieces = new Dictionary<string,PieceState> ();
		var layers = GameObject.FindGameObjectsWithTag ("Layer");
		for (var k = 0; k < layers.Length; k++)
		{
			for (var i = 0; i < 8; i++)
			{
				for (var j = 0; j < 8; j++)
				{
					//Debug.Log (layers[k].name + "-" + layers[k].transform.GetChild (i).name + "-" + layers[k].transform.GetChild (i).GetChild(j).name);
					var key = layers [k].name + "-" + layers [k].transform.GetChild (i).name + "-" + layers [k].transform.GetChild (i).GetChild (j).name;

					layers [k].transform.GetChild (i).GetChild (j).GetComponent<Renderer> ().material.color = baseColor.color;
					layers [k].transform.GetChild (i).GetChild (j).GetComponent<BoxCollider> ().enabled = false;

					var pieceState = new PieceState (layers [k].transform.GetChild (i).GetChild (j).gameObject, baseColor, false);
					pieces.Add (key, pieceState);
				}
			}
		}

		Debug.Log ("OthelloPieces End InitPlayArea ");
	}

	public void SetFirstPieces() {
		Debug.Log ("OthelloPieces Start SetFirstPieces ");
		string[] initPos = {"Layer04-XRow04-ZCol04","Layer04-XRow04-ZCol05","Layer04-XRow05-ZCol05","Layer04-XRow05-ZCol04",
			"Layer05-XRow04-ZCol05","Layer05-XRow05-ZCol05","Layer05-XRow05-ZCol04","Layer05-XRow04-ZCol04"  };

		for (var i = 0; i < initPos.Length; i++) {
			if (i % 2 == 0) {
				pieces [initPos[i]].SetColor(player1Color);
			} else {
				pieces [initPos[i]].SetColor(player2Color);
			}
		}

		Debug.Log ("OthelloPieces End SetFirstPieces ");		
	}

	public bool GetIsMyTurn() {
		return bool.Parse (isMyTurn);
	}

	public void SetIsMyTurn(bool isMyTurn) {
		this.isMyTurn = isMyTurn.ToString ();
	}

	public JsonCarrier GetState() {
		Debug.Log ("OthelloPieces Start GetState ");
		var result = new JsonCarrier ();
		result.actionType = "status_update";
		result.isMyTurn = this.isMyTurn;
		result.lastPosition = currentSelected;
		result.pieacesState = this.GetPiecesState ();

		Debug.Log ("OthelloPieces End GetState ");
		return result;
	}

	public void SetState(JsonCarrier data){
		Debug.Log ("OthelloPieces Start SetState ");

		if (data == null || data.pieacesState == null) {
			Debug.LogWarning ("state data is null");
			return;
		}
		this.isMyTurn = data.isMyTurn;
		gameControl.GetComponent<MainControl> ().SetRoomNumber (data.roomNumber);
		foreach (var state in data.pieacesState) {
			var items = state.Split (new string[]{"\t"}, System.StringSplitOptions.None);
			if (pieces.ContainsKey (items [0])) {
				var initMaterial = pieces [items [0]].material;
				//Debug.LogWarning ("initMaterial.color " + initMaterial.color + " GetMaterial (items [1]) " + GetMaterial (items [1]).color );
				if (initMaterial.Equals (GetMaterial (items [1]))) {
					
				} else {
					var target = pieces [items [0]].piece.transform.position;
					if (initMaterial.Equals (player1Color)) {
						//Debug.LogWarning ("player1 effect");
						this.turnOverEffect1.transform.position = new Vector3 (target.x, target.y, 8.0f);
						this.turnOverEffect1.GetComponent<ParticleSystem> ().Play ();
					} else if (initMaterial.Equals (player2Color)) {
						//Debug.LogWarning ("player2 effect");
						this.turnOverEffect2.transform.position = new Vector3 (target.x, target.y, 8.0f);
						this.turnOverEffect2.GetComponent<ParticleSystem> ().Play ();
					}
				}
				pieces [items [0]].SetColor(GetMaterial (items [1]));
				pieces [items [0]].SetColliderEnabled (System.Boolean.Parse(items [2]));
			} else {
				Debug.LogWarning ("invalid data " + state);
			}
		}

		Debug.Log ("OthelloPieces End SetState ");
	}

	public void PutNewPostion(string posKey){
		var piece = pieces [posKey].piece;
		var pieceState = new PieceState (piece, player1Color, false);
		pieces [posKey] = pieceState;
		isMyTurn = false.ToString();
	}

	private Material GetMaterial(string code){
		var result = this.baseColor;

		if (code.Equals ("1")) {
			result = this.player1Color;
		} else if (code.Equals ("2")) {
			result = this.player2Color;
		} else if (!code.Equals ("0")) {
			Debug.LogWarning ("invalid status " + code);
		}

		return result;
	}

	private List<string> GetPiecesState(){
		Debug.Log ("OthelloPieces Start GetPiecesState ");
		var result = new List<string> ();

		foreach (var piece in pieces) {
			var temp = "";
			temp += (piece.Key + "\t");
			temp += piece.Value.GetState();
			result.Add (temp);
		}

		Debug.Log ("OthelloPieces End GetPiecesState " + result[0]);
		return result;
	}

	private class PieceState {
		public GameObject piece;
		public Material material;
		public bool isColliderEnabled;

		public PieceState(GameObject piece,Material material,bool isColliderEnabled){
			this.piece = piece;
			this.material = material;
			this.isColliderEnabled = isColliderEnabled;
		}

		public void SetColor(Material material){
			this.piece.GetComponent<Renderer> ().material.color = material.color;
			this.material = material;
		}

		public void SetColliderEnabled(bool isColliderEnabled){
			this.piece.GetComponent<BoxCollider> ().enabled = isColliderEnabled;
			this.isColliderEnabled = isColliderEnabled;
		}

		public string GetState() {
			var result = "";
			result += GetColorState(this.material.name);
			result += "\t";
			result += this.isColliderEnabled.ToString ();
			return result;
		}

		private string GetColorState(string materialName) {
			var result = "0";

			for (var i = 0; i < OthelloPieces.colorNames.Length; i++) {
				if(materialName.StartsWith(OthelloPieces.colorNames[i])) {
					result = i.ToString();
				}
			}

			return result;
		}
	}
}
