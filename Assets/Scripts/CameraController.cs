using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraController: MonoBehaviour {
	public GameObject Target;
	public float ZoomSensitivity;
	public float RotationSpeed;
	public int RotateZoomThreshold;

	public GameObject UI;

	private int mode = 0;
	public int Mode {
		get { return mode; }
		set { mode = value; }
	}

	private bool auto = true;
	public bool Auto {
		get { return auto; }
		set { auto = value; }
	}

	public Vector3 Offset = new Vector3(0, 0, 0);
	private Vector3 MouseLastFrame = new Vector3(0, 0, 0);
	
	// Update is called once per frame
	void Update () {
		Camera c = GetComponent<Camera> ();
		if (Target != null) {
			if (Input.GetMouseButton (1)) {
				Offset += Camera.main.ScreenToWorldPoint(MouseLastFrame) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			transform.position = Target.transform.position + Offset;
		} else {
			if (Input.GetMouseButton (1)) {
				transform.position += Camera.main.ScreenToWorldPoint (MouseLastFrame) - Camera.main.ScreenToWorldPoint (Input.mousePosition);
			}
		}
		transform.position = new Vector3 (transform.position.x, transform.position.y, -8);
		GravityBehaviour Planet = null;
		foreach(GravityBehaviour p in FindObjectsOfType<GravityBehaviour> ()) {
			if (Planet == null) {
				Planet = p;
				continue;
			}
			if ((p.transform.position - transform.position).magnitude < (Planet.transform.position - transform.position).magnitude) {
				Planet = p;
			}
		}
		if (Auto) {
			Mode = 0;
			if ((Planet.transform.position - transform.position).magnitude < Planet.transform.localScale.magnitude && c.orthographicSize < RotateZoomThreshold) {
				Mode = 1;
			}
		}
		switch (Mode) {
		case 0:
			transform.up = Vector3.MoveTowards (transform.up, new Vector3 (0, 1, 0), RotationSpeed);
			break;

		case 1:
			Vector3 v = transform.position;
			v.z = 0;
			transform.up = Vector3.MoveTowards (transform.up, (v - Planet.transform.position).normalized, RotationSpeed);
			break;

		case 2:
			transform.up = Vector3.MoveTowards (transform.up, Target.transform.up, RotationSpeed);
			break;
		}

		c.orthographicSize -= c.orthographicSize * Input.mouseScrollDelta.y * ZoomSensitivity;
		transform.localScale = new Vector3 (c.orthographicSize, c.orthographicSize, c.orthographicSize);
		MouseLastFrame = Input.mousePosition;
	}
}
