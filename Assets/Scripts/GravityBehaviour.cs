using UnityEngine;
using System.Collections;

public class GravityBehaviour : MonoBehaviour {
	public float Mass;
	float Falloff = 0.003f;

	// Update is called once per frame
	void FixedUpdate () {
		foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D> ()) {
			if (rb.tag == "Craft") {
				Vector3 offset = transform.position - new Vector3(rb.worldCenterOfMass.x, rb.worldCenterOfMass.y, 0);
				rb.AddForce (offset.normalized * ((rb.mass * Mass) / (offset.magnitude * Falloff)));
			}
		}
	}
}
