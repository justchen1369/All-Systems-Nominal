using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceSupply : MonoBehaviour {
	public enum ResourceVarient {
		Power,
		LiquidFuel,
		LiquidOxygen,
		SolidFuel
	}

	public struct ResourceVarientMeta {
		public float MassPerUnit;
		public float EnergyPerUnit;
		public string Name;
	}

	public abstract float Request (float Maximum);
	public abstract float Query (float Maximum);

	public ResourceVarient ResourceType;
}
