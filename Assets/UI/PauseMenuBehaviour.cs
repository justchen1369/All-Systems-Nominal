using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuBehaviour : MonoBehaviour {
	public Button ResumeButton;
	public Button QuitButton;

	// Use this for initialization
	void Start () {
		ResumeButton.onClick.AddListener (() => gameObject.SetActive (false));
		QuitButton.onClick.AddListener (() => Application.Quit());
	}
}
