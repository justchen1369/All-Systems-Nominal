using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : ResourceSupply {
	public override float Request (float Maximum) {
		Units -= Mathf.Min(Units / PressurePerUnit, Maximum);
		return Mathf.Min(Units / PressurePerUnit, Maximum);
	}

	public override float Query (float Maximum) {
		return Mathf.Min(Units / PressurePerUnit, Maximum);
	}

	public float PressurePerUnit;
	public float MaximumPressure;
	public float Units;
}
