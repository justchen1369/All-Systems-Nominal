using UnityEngine;
using System.Collections;

public class RecourseStorage : MonoBehaviour {
	public RecourseType Contents;
	public float Capacity;
	public float MaxValue  {
		get {return Capacity * MaxPressure;}
	}
	public float Value;
	public float MaxPressure;
	public string PressureDamageMode;
	public float Pressure {
		get { return Value / Capacity; }
		set { Value = value * Capacity; }
	}
	public string Description {
		get { return string.Format (Contents.Description.Replace("\\n", "\n"), new object[] { Mathf.Round(Value * 100) / 100, Mathf.Round(MaxValue * 100) / 100, Mathf.Round(Pressure * 100) / 100, Mathf.Round(MaxPressure * 100) / 100, Mathf.Round(Capacity * 100) / 100 }); }
	}
}
