using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecourseStoragePanel : MonoBehaviour {
	public RecourseStorage Target;
	public GameObject BindingDoodad;
	public Text name;
	public Text desc;

	void Update () {
		if (Target != null) {
			Image image = BindingDoodad.GetComponent<Image> ();
			image.color = Target.Contents.Color;
			name.text = Target.Contents.name;
			desc.text = Target.Description;
		}
	}
}
