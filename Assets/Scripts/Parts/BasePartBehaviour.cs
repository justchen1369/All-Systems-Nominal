using UnityEngine;
using System.Collections;

public class BasePartBehaviour : MonoBehaviour {
	[System.Serializable]
	public class thermal {
		public float ThermalEnergy = 20f;
		public float ThermalHeatCapacity = 1f;
		public float Temperature {
			get { return ThermalEnergy / ThermalHeatCapacity; }
			set { ThermalEnergy = ThermalHeatCapacity * value; }
		}
		public float MaxTemperature = 500f;
		public float AmbientTemperature = 20f;
	}
	public thermal Thermal;
	public float MaxCrashVelocity = 7f;
	public string Name;
	public string Description;

	[System.Serializable]
	public class connectionPoint {
		public Vector2 LocalPosition;
		public Vector2 LocalUp;
		public connectionPoint Atatchment;
	}

	public connectionPoint[] ConnectionPoints;

	public float distance(Vector3 comparison) {
		return (transform.position - comparison).magnitude;
	}

	public GameObject Explosion;
	public float ExplosionEnergy = 0f;

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.relativeVelocity.magnitude > MaxCrashVelocity) {
			if (Explosion != null) {
				GameObject e = Instantiate (Explosion);
				Transform t = e.transform;
				ParticleSystem ps = e.GetComponent<ParticleSystem> ();
				t.position = transform.position;
				ps.Play ();
			}
			foreach (BasePartBehaviour tc in FindObjectsOfType<BasePartBehaviour>()) {
				tc.Thermal.ThermalEnergy += ExplosionEnergy / Mathf.Max (1, tc.distance (transform.position));
			}
			Destroy (gameObject);
			if (transform.parent.childCount < 2) {
				Destroy (transform.parent.gameObject);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Rigidbody2D rb = gameObject.GetComponentInParent<Rigidbody2D>();
		if (rb != null) {

			if (Thermal.Temperature > Thermal.MaxTemperature) {
				if (Explosion != null) {
					GameObject e = Instantiate (Explosion);
					Transform t = e.transform;
					ParticleSystem ps = e.GetComponent<ParticleSystem> ();
					t.position = transform.position;
					ps.Play ();
				}
				foreach (BasePartBehaviour tc in FindObjectsOfType<BasePartBehaviour>()) {
					tc.Thermal.ThermalEnergy += ExplosionEnergy / Mathf.Max (1, tc.distance (transform.position));
				}
				Destroy (gameObject);
				if (transform.parent.childCount < 2) {
					Destroy (transform.parent.gameObject);
				}
			}
			foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer> ()) {
				sr.color = new Color (
					1,
					Mathf.InverseLerp (Thermal.MaxTemperature, 0, Thermal.Temperature),
					Mathf.InverseLerp (Thermal.MaxTemperature, 0, Thermal.Temperature)
				);
			}
		}
	}
}
