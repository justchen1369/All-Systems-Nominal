using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAMADEBehaviourScript : MonoBehaviour {
	// Update is called once per frame
	void FixedUpdate () {
		Peripheral Periph = GetComponent<Peripheral> ();
		BasePartBehaviour bph = GetComponent<BasePartBehaviour> ();
		Periph.Ports [0] = Mathf.RoundToInt(bph.Thermal.Temperature);
	}
}
