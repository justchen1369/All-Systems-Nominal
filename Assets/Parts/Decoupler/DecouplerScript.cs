using UnityEngine;
using System.Collections;

public class DecouplerScript : MonoBehaviour {
	public GameObject BaseShip;
	public FixedJoint2D joint;
	public Peripheral Peripheral;

	void FixedUpdate() {
		if (enabled) {
			if (transform.parent != null) {
				if (transform.parent.tag == "Craft") {
					if (joint == null) {
						GameObject oldParent = transform.parent.gameObject;
						if (oldParent.transform.childCount > 1) {
							GameObject newShip = Instantiate (BaseShip);
							newShip.transform.position = transform.position;
							newShip.transform.rotation = transform.rotation;
							Rigidbody2D newRigidbody = newShip.GetComponent<Rigidbody2D> ();
							transform.SetParent (newShip.transform);
							joint = oldParent.AddComponent<FixedJoint2D> ();
							joint.connectedBody = newRigidbody;
						}
					} else {
						if (Peripheral.Ports [0] > 0) {
							Destroy (joint);
							enabled = false;
						}
					}
				} else {
					if (joint != null) {
						Destroy (joint);
					}
				}
			}
		}
	}
}
