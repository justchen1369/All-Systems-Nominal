using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class RecourseConnectionPanel : MonoBehaviour {
	public RecourseConnection Target;
	public GameObject BindingDoodad;
	public Text text;

	public bool IsBinding;

	void Update () {
		if (Target != null) {
			Image image = BindingDoodad.GetComponent<Image> ();
			image.color = Target.Recourse.Color;
			LineRenderer line = BindingDoodad.GetComponent<LineRenderer> ();
			line.startColor = Target.Recourse.Color;
			line.endColor = Target.Recourse.Color;
			text.text = Target.Recourse.name;
			if (BindingDoodad != null && Target != null) {
				LineRenderer lr = BindingDoodad.GetComponent<LineRenderer> ();
				if (Input.GetMouseButtonDown (0) && (Input.mousePosition - BindingDoodad.transform.position).magnitude < 10) {
					IsBinding = true;
				}
				if (Input.GetMouseButtonUp (0) && IsBinding) {
					foreach (Collider2D co in Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition))) {
						if (co != null) {
							if (co.tag == "Part") {
								if (co.GetComponentsInChildren<RecourseStorage> ().Any((r) => r.Contents == Target.Recourse)) {
									RecourseStorage rs = co.GetComponentsInChildren<RecourseStorage> ().First ((r) => r.Contents == Target.Recourse);
									if (rs != null) {
										if (Target.TargetStorages.Any ((trs) => trs == rs)) {
											Target.TargetStorages = Target.TargetStorages.SkipWhile ((trs) => trs == rs || trs == null).ToArray ();
										} else {
											Target.TargetStorages = Target.TargetStorages.Concat (new RecourseStorage[] { rs }).ToArray ();
										}
									}
								}
							}
						}
					}
					IsBinding = false;
				}
				if (Target.TargetStorages.Length > 0 || IsBinding) {
					lr.enabled = true;
					int acc = 0;
					if (IsBinding) {
						lr.positionCount = (Target.TargetStorages.Length * 2 + 2);
					} else {
						lr.positionCount = (Target.TargetStorages.Length * 2);
					}
					foreach (RecourseStorage rs in Target.TargetStorages) {
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
}
