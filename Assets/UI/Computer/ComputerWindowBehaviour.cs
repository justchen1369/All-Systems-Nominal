using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class ComputerWindowBehaviour : MonoBehaviour {
	public ComputerPartScript Computer;
	public GameObject Target;
	public GameObject BindingDoodad;
	public GameObject ContentParent;

	public InputField CodeInput;
	public Text CodeOutput;

	public bool IsBinding = false;

	void OnTargetLost() {
		CodeInput.text = "";
		CodeInput.interactable = false;
	}
	void OnTargetChange() {
		Computer = Target.GetComponent<ComputerPartScript> ();
		if (Computer != null) {
			CodeInput.interactable = true;
			CodeInput.text = Computer.Program;
		} else {
			CodeInput.interactable = false;
		}
	}
	void UpdateCode(string s) {
		if (Target != null && Computer != null) {
			Computer.Program = s;
		} else {
			CodeInput.text = "";
		}
	}

	void Start() {
		CodeInput.onEndEdit.AddListener ((s) => UpdateCode (s));
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
		if (Computer != null) {
			CodeOutput.text = Computer.Output.TrimEnd (new char[3] { ' ', '\n', '	' });
		} else {
			CodeOutput.text = "> NO SIGNAL";
		}
		if (Time.frameCount % 30 < 15) {
			CodeOutput.text += "_";
		}
	}
}
