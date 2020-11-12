using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WindowMovementScript : MonoBehaviour {
	public GameObject MovementDoodad;
	public GameObject MinimiseDoodad;
	public GameObject ContentWindow;

	public bool isMoving = false;
	private Vector3 mouseOffset;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (MinimiseDoodad != null) {
				if ((Input.mousePosition - MinimiseDoodad.transform.position).magnitude < 10) {
					ContentWindow.SetActive (!ContentWindow.activeSelf);
					UnityEngine.UI.Image image = transform.parent.gameObject.GetComponent<UnityEngine.UI.Image> ();
					image.enabled = ContentWindow.activeSelf;
				}
			}
			if (MovementDoodad != null) {
				if ((Input.mousePosition - MovementDoodad.transform.position).magnitude < 10) {
					isMoving = true;
					mouseOffset = transform.parent.position - Input.mousePosition;
				}
			}
		}
		if (isMoving) {
			transform.parent.position = Input.mousePosition + mouseOffset;
			if (Input.GetMouseButtonUp (0)) {
				isMoving = false;
			}
		}
	}
}
