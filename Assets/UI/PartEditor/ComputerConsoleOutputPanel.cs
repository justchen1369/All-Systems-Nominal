using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerConsoleOutputPanel : MonoBehaviour {
	public ComputerPartScript Target;
	public Text CodeOutput;

	// Update is called once per frame
	void Update () {
		if (Target != null) {
			CodeOutput.text = Target.Output.TrimEnd (new char[3] { ' ', '\n', '	' });
			if (Time.frameCount % 30 < 15) {
				CodeOutput.text += "_";
			} else {
				CodeOutput.text += " ";
			}
		}
	}
}
