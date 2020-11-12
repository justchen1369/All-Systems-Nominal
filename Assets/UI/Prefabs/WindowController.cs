using UnityEngine;
using System.Collections;

public class WindowController : MonoBehaviour {
	public GameObject Target;
	public GameObject BindingDoodad;
	public GameObject ContentParent;

	public bool IsBinding = false;

	void OnTargetLost() {}
	void OnTargetChange() {}
	
	// Update is called once per frame
	void Update () {
		if (BindingDoodad != null) {
			LineRenderer lr = BindingDoodad.GetComponent<LineRenderer> ();
			if (Input.GetMouseButtonDown (0) && (Input.mousePosition - BindingDoodad.transform.position).magnitude < 10) {
				IsBinding = true;
				Target = null;
				OnTargetLost ();
			}
			if (Input.GetMouseButtonUp (0) && IsBinding) {
				Collider2D co = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				if (co != null) {
					if (co.tag == "Part" && co.isActiveAndEnabled) {
						Target = co.gameObject;
						OnTargetChange ();
					}
				}
				IsBinding = false;
			}
			if (Target != null || IsBinding) {
				lr.enabled = true;
				lr.SetPosition (0, Camera.main.ScreenToWorldPoint (BindingDoodad.transform.position) + new Vector3 (0, 0, 8));
				if (IsBinding) {
					lr.SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0, 0, 8));
				} else {
					lr.SetPosition (1, Target.transform.position);
				}
			} else {
				lr.enabled = false;
			}
		}
	}
}
