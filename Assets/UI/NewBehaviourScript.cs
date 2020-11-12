using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {	
	public Slider slider;
	public Text text;

	// Update is called once per frame
	void Start () {
		slider.onValueChanged.AddListener ((value) => onValueChanged (value));
	}

	void onValueChanged(float value) {
		Time.timeScale = value;
		text.text = string.Format ("x {0}", Mathf.Floor(value * 10f) / 10f);
	}
}
