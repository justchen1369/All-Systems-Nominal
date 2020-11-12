using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroBehaviour : MonoBehaviour {
	public GameObject GyroWheel;

	void Update () {
		GyroWheel.transform.up = new Vector3 (0, 1, 0);
	}

	// Update is called once per frame
	void FixedUpdate () {
		Peripheral Periph = GetComponent<Peripheral> ();
		Periph.Ports [0] = Mathf.RoundToInt(transform.eulerAngles.z);
	}
}
