using UnityEngine;
using System.Collections;
using System.Linq;

public class RecourseConnection : MonoBehaviour {
	public RecourseStorage[] TargetStorages;
	public RecourseType Recourse;
	public float TakeRecourse(float value) {
		float x = value;
		foreach (RecourseStorage rs in TargetStorages) {
			x = Mathf.Min (rs.Pressure, x);
			rs.Value -= Mathf.Min(rs.Pressure, value) / TargetStorages.Length;
		}
		if (TargetStorages.Length < 1) {
			x = 0;
		}
		return x;
	}
	public float RecourseAvailability(float value) {
		float x = value;
		foreach (RecourseStorage rs in TargetStorages) {
			x = Mathf.Min (rs.Pressure, x);
		}
		if (TargetStorages.Length < 1) {
			x = 0;
		}
		return x;
	}
	public bool AllowTransfer = false;
	public int MaxConnections;

	void FixedUpdate() {
		if (AllowTransfer == true && TargetStorages.Length > 1) {
			float x = 0;
			foreach (RecourseStorage rs in TargetStorages) {
				if (rs != null) {
					x += rs.Pressure;
				}
			}
			foreach (RecourseStorage rs in TargetStorages) {
				if (rs != null) {
					rs.Pressure = x / TargetStorages.Length;
				}
			}
		}
		TargetStorages = TargetStorages.SkipWhile ((trs) => trs == null).ToArray ();
	}
}
