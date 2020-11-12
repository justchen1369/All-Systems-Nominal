using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConstructionBehavior : MonoBehaviour {
	GameObject TargetedHardware;
	public GameObject BaseVessel; 

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && TargetedHardware == null) {
			foreach (Collider2D collider in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
				HardwareBehavior hb = collider.GetComponentInParent<HardwareBehavior> ();
				if (hb != null) {
					Transform parent = hb.transform.parent;
					TargetedHardware = hb.gameObject;
					TargetedHardware.transform.SetParent (null);
					if (parent.childCount == 0) {
						Destroy (parent.gameObject);
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && TargetedHardware != null) {
			PolygonCollider2D collider = TargetedHardware.GetComponentInChildren<PolygonCollider2D> ();
			Bounds bounds = collider.bounds;
			bounds.Expand (0.1f);
			foreach (Collider2D other in Physics2D.OverlapAreaAll (bounds.min, bounds.max).OrderBy((c) => (c.transform.position - TargetedHardware.transform.position).magnitude)) {
				HardwareBehavior hb = other.GetComponentInParent<HardwareBehavior> ();
				if (hb != null && hb.gameObject != TargetedHardware) {
					TargetedHardware.transform.SetParent (hb.transform.parent);
					TargetedHardware.transform.localPosition = new Vector2 (Mathf.Round (TargetedHardware.transform.localPosition.x), Mathf.Round(TargetedHardware.transform.localPosition.y));
					TargetedHardware.transform.localEulerAngles = new Vector3 (0, 0, Mathf.Round (TargetedHardware.transform.localEulerAngles.z / 90f) * 90f);
					break;
				}
			}
			if (TargetedHardware.transform.parent == null) {
				GameObject Vessel = Instantiate (BaseVessel);
				Vessel.transform.position = TargetedHardware.transform.position;
				Vessel.transform.rotation = TargetedHardware.transform.rotation;
				TargetedHardware.transform.SetParent (Vessel.transform);
			}
			TargetedHardware = null;
		}
		if (TargetedHardware != null) {
			TargetedHardware.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3(0, 0, 10);
			if (Input.GetKeyDown (KeyCode.R)) {
				TargetedHardware.transform.Rotate (new Vector3 (0, 0, 1), 45f);
			}
		}
	}
}
