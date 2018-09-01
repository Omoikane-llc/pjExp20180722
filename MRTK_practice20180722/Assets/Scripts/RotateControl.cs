using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour {
	public GameObject othelloPlayArea;
	public static float xRotationRate = -0.1f;
	public static float yRotationRate = 0.1f;

	private List<float> rotateRateBuffer;

	// Use this for initialization
	void Start () {
		rotateRateBuffer = new List<float> ();
	}
	
	// Update is called once per frame
	void Update () {
		othelloPlayArea.transform.Rotate(new Vector3 (xRotationRate, yRotationRate, 0));
	}

	public void StopRotate() {
		rotateRateBuffer.Add (xRotationRate);
		rotateRateBuffer.Add (yRotationRate);
		xRotationRate = 0.0f;
		yRotationRate = 0.0f;
	}

	public void RestoreRate() {
		if (rotateRateBuffer.Count > 1) {
			xRotationRate = rotateRateBuffer [0];
			yRotationRate = rotateRateBuffer [1];
			rotateRateBuffer.Clear ();
		}
	}

}
