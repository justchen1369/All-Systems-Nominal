using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

class HardwareBehavior : MonoBehaviour {
	[System.Serializable]
	public class structural {
		public int MaxHealth;
		private int health;
		public int Health
		{
			get { return health; }
			set { Mathf.Clamp (value, 0, MaxHealth); }
		}
		public float Weight;
	}

	[System.Serializable]
	public class thermodynamic {
		public float ThermalCapacity;
		public float Insulation;
		public float Temperature;
	}

	public structural StructuralSpecs;
	public thermodynamic ThermodynamicSpecs;

	void Start() {
		StructuralSpecs.Health = StructuralSpecs.MaxHealth;
	}
}