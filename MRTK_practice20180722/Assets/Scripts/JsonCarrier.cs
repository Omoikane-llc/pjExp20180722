using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonCarrier {
	public string actionType;
	public string roomNumber;
	public string isMyTurn;
	public string lastPosition;
	public List<string> pieacesState; // Layer-xRow-zCol:color(0 or 1 or 2) 
	//public List<string> pieacesColliderState; // Layer-xRow-zCol:collider(0 or 1)

}
