using UnityEngine;
using System.Collections;

public class PartEditorWindow : MonoBehaviour {
	public GameObject Target;
	public GameObject BindingDoodad;
	public GameObject ContentParent;

	public GameObject RecourseConnectionPanel;
	public GameObject RecourseStoragePanel;
	public GameObject ComputerCodeInputPanel;
	public GameObject ComputerConsoleOutputPanel;
	public GameObject BasePartPanel;

	public bool IsBinding = false;

	void OnTargetLost() {
		foreach (RectTransform rt in ContentParent.GetComponentsInChildren<RectTransform>()) {
			if (rt.gameObject != ContentParent.gameObject) {
				Destroy (rt.gameObject);
			}
		}
	}
	void OnTargetChange() {
		foreach (BasePartBehaviour bps in Target.GetComponentsInChildren<BasePartBehaviour>()) {
			PartStatsPanel NewPanel = Instantiate (BasePartPanel).GetComponent<PartStatsPanel> ();
			NewPanel.Target = bps;
			NewPanel.transform.SetParent (ContentParent.transform);
		}
		foreach (RecourseConnection rc in Target.GetComponentsInChildren<RecourseConnection>()) {
			RecourseConnectionPanel NewPanel = Instantiate (RecourseConnectionPanel).GetComponent<RecourseConnectionPanel> ();
			NewPanel.Target = rc;
			NewPanel.transform.SetParent (ContentParent.transform);
		}
		foreach (RecourseStorage rs in Target.GetComponentsInChildren<RecourseStorage>()) {
			RecourseStoragePanel NewPanel = Instantiate (RecourseStoragePanel).GetComponent<RecourseStoragePanel> ();
			NewPanel.Target = rs;
			NewPanel.transform.SetParent (ContentParent.transform);
		}
		foreach (ComputerPartScript cps in Target.GetComponentsInChildren<ComputerPartScript>()) {
			ComputerCodeInputPanel NewPanel = Instantiate (ComputerCodeInputPanel).GetComponent<ComputerCodeInputPanel> ();
			NewPanel.Target = cps;
			NewPanel.transform.SetParent (ContentParent.transform);
		}
		foreach (ComputerPartScript cps in Target.GetComponentsInChildren<ComputerPartScript>()) {
			ComputerConsoleOutputPanel NewPanel = Instantiate (ComputerConsoleOutputPanel).GetComponent<ComputerConsoleOutputPanel> ();
			NewPanel.Target = cps;
			NewPanel.transform.SetParent (ContentParent.transform);
		}

	}

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
