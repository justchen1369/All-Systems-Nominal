using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ThrusterControllerWindowBehaviour : MonoBehaviour {
	public ThrusterPartScript[] Thrusters;
	public Text StatusText;
	public Slider Throttle;
	public Slider Gimbal;
	public RectTransform JoyStick;

	public GameObject BindingDoodad;
	public GameObject ContentParent;

	public bool IsBinding = false;

	// Update is called once per frame
	void Update () {
		if (BindingDoodad != null) {
			LineRenderer lr = BindingDoodad.GetComponent<LineRenderer> ();
			if (Input.GetMouseButtonDown (0) && (Input.mousePosition - BindingDoodad.transform.position).magnitude < 10) {
				IsBinding = true;
			}
			if (Input.GetMouseButtonUp (0) && IsBinding) {
				foreach (Collider2D co in Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition))) {
					if (co != null) {
						if (co.tag == "Part") {
							ThrusterPartScript Thruster = co.GetComponent<ThrusterPartScript> ();
							if (Thruster != null) {
								if (Thrusters.Any((tps) => tps == Thruster)) {
									Thrusters = Thrusters.SkipWhile ((tps) => tps == Thruster || tps == null).ToArray ();
								} else {
									Thrusters = Thrusters.Concat (new ThrusterPartScript[] { Thruster }).ToArray ();
								}
							}
						}
					}
				}
				IsBinding = false;
			}
			ThrusterPartScript[] LineThrustersRendered = Thrusters.SkipWhile ((tps) => tps == null).ToArray ();
			if (LineThrustersRendered.Length > 0 || IsBinding) {
				lr.enabled = true;
				int acc = 0;
				if (IsBinding) {
					lr.positionCount = (LineThrustersRendered.Length * 2 + 2);
				} else {
					lr.positionCount = (LineThrustersRendered.Length * 2);
				}
				foreach (ThrusterPartScript tps in LineThrustersRendered) {
					lr.SetPosition (acc, Camera.main.ScreenToWorldPoint (BindingDoodad.transform.position) + new Vector3 (0, 0, 8));
					lr.SetPosition (acc + 1, tps.transform.position);
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

	void FixedUpdate() {
		StatusText.text = "";
		int acc = 0;
		foreach (ThrusterPartScript tps in Thrusters) {
			if (tps != null) {
				Peripheral Periph = tps.GetComponent<Peripheral>();
				Periph.Ports [0] = Mathf.RoundToInt (Input.GetAxis ("Horizontal") * Gimbal.value);
				Periph.Ports [2] = Mathf.RoundToInt (Mathf.Clamp (Input.GetAxis ("Vertical"), 0f, 1f) / 64f * Throttle.value);
				StatusText.text += string.Format ("Thruster {0} online: Temp {3} C, H: {1} V: {2} \n", new object[] {
					acc,
					Periph.Ports [0],
					Periph.Ports [2],
					Periph.Ports [3]
				});
				if (Periph.Ports [3] < tps.thruster.ignitionTemperatureThreshold + 10 && Input.GetAxis ("Vertical") > 0) {
					Periph.Ports [1] = 1;
				} else {
					Periph.Ports [1] = 0;
				}
			} else {
				StatusText.text += string.Format ("Thruster {0} offline \n", new object[] { acc });
			}
			acc++;
		}
		RectTransform parent = JoyStick.parent.GetComponent<RectTransform> ();
		JoyStick.anchoredPosition = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * parent.rect.width * 0.4f;
	}
}
