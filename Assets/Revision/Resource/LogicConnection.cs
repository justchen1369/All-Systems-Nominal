using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LogicConnection : MonoBehaviour {
	public List<LogicConnection> Inputs;

	abstract public int Compute ();

	public bool RenderLines;

	public void Update()
	{
		if (RenderLines) {
			foreach(LogicConnection LC in Inputs) {
				Instantiate<LineRenderer> (new LineRenderer ());
			}
		}
	}
}
