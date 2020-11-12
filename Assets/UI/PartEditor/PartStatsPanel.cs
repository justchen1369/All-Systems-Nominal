using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartStatsPanel : MonoBehaviour {
	public Text name;
	public Text desc;
	public BasePartBehaviour Target;

	// Update is called once per frame
	void Update () {
		if (Target != null) {
			name.text = Target.Name;
			desc.text = Target.Description.Replace ("\\n", "\n");
		}
	}
}
