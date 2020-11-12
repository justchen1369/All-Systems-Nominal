using UnityEngine;
using System.Collections;

public class BindToUIBehaviour : MonoBehaviour {
	[System.Serializable]
	public class KeyToUIBind {
		public KeyCode PrimaryKey;
		public KeyCode SecondaryKey;
		public GameObject gameObject;
	}

	public KeyToUIBind[] Binds;

	// Update is called once per frame
	void Update () {
		foreach (KeyToUIBind Bind in Binds) {
			if (Bind.SecondaryKey == KeyCode.None) {
				if (Input.GetKeyDown (Bind.PrimaryKey)) {
					Bind.gameObject.SetActive (!Bind.gameObject.activeSelf);
				}
			} else {
				if (Input.GetKey(Bind.PrimaryKey) && Input.GetKeyDown (Bind.SecondaryKey)) {
					Bind.gameObject.SetActive (!Bind.gameObject.activeSelf);
				}
			}
		}
	}
}
