using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ComputerCodeInputPanel : MonoBehaviour {
	public ComputerPartScript Target;
	public GameObject BindingDoodad;
	public InputField CodeInput;
	public Button ExecuteButton;
	private Text ExecuteButtonText;

	public bool IsBinding = false;

	void UpdateCode() {
		if (Target != null) {
			if (Target.enabled) {
				CodeInput.interactable = true;
				CodeInput.text = Target.Program;
				Target.enabled = false;
				ExecuteButtonText.text = "Execute";
			} else {
				CodeInput.interactable = false;
				Target.Program = CodeInput.text;
				Target.enabled = true;
				ExecuteButtonText.text = "Stop";
			}
		}
	}

	void Start() {
		ExecuteButtonText = ExecuteButton.GetComponentInChildren<Text> ();
		ExecuteButton.onClick.AddListener (() => UpdateCode ());
		CodeInput.text = Target.Program;
		if (Target.enabled) {
			ExecuteButtonText.text = "Stop";
			CodeInput.interactable = false;
		} else {
			ExecuteButtonText.text = "Execute";
			CodeInput.interactable = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Target != null) {
			LineRenderer lr = BindingDoodad.GetComponent<LineRenderer> ();
			if (Input.GetMouseButtonDown (0) && (Input.mousePosition - BindingDoodad.transform.position).magnitude < 10) {
				IsBinding = true;
			}
			if (Input.GetMouseButtonUp (0) && IsBinding) {
				foreach (Collider2D co in Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition))) {
					if (co != null) {
						if (co.tag == "Part") {
							Peripheral Periph = co.GetComponentInChildren<Peripheral> ();
							if (Periph != null) {
								if (Target.connectedPeripherals.Any ((cp) => cp == Periph)) {
									Target.connectedPeripherals = Target.connectedPeripherals.SkipWhile ((cp) => cp == Periph || Periph == null).ToArray ();
								} else {
									Target.connectedPeripherals = Target.connectedPeripherals.Concat (new Peripheral[] { Periph }).ToArray ();
								}
							}
						}
					}
				}
				IsBinding = false;
			}
			if (Target.connectedPeripherals.Length > 0 || IsBinding) {
				lr.enabled = true;
				int acc = 0;
				if (IsBinding) {
					lr.positionCount = (Target.connectedPeripherals.Length * 2 + 2);
				} else {
					lr.positionCount = (Target.connectedPeripherals.Length * 2);
				}
				foreach (Peripheral rs in Target.connectedPeripherals) {
					lr.SetPosition (acc, Camera.main.ScreenToWorldPoint (BindingDoodad.transform.position) + new Vector3 (0, 0, 8));
					lr.SetPosition (acc + 1, rs.transform.position);
					acc += 2;
				}
				if (IsBinding) {
					lr.SetPosition (acc, Camera.main.ScreenToWorldPoint (BindingDoodad.transform.position) + new Vector3 (0, 0, 8));
					lr.SetPosition (acc + 1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0, 0, 8));
				}
			} else {
				lr.enabled = false;
			}
		}
	}
}
