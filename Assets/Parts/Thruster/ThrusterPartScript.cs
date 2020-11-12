using UnityEngine;
using System.Collections;
using System.Linq;


public class ThrusterPartScript : MonoBehaviour {
	public bool sparkerOn;
	public bool SetSparker {
		get { return sparkerOn; }
		set { sparkerOn = value; }
	}

	[System.Serializable]
	public class Thruster {
		public float forcePerUnit;
		public float gimbalAngle;
		public float ignitionTemperatureThreshold;
		public float inginitionThermalEnergy;

		public RecourseConnection[] requiredRecourses;

	}
	public Thruster thruster;

	[System.Serializable]
	public class Sparker {
		public float sparkerConsumption;

		public RecourseConnection[] requiredRecourses;
	}
	public Sparker sparker;

	public float particleViolence;

	public float gimbal {
		get { 
			Vector3 v = GimbalTransform.localEulerAngles;
			return v.z;
		}
		set { GimbalTransform.localEulerAngles = new Vector3 (0f, 0f, Mathf.Clamp(value, -thruster.gimbalAngle, thruster.gimbalAngle));}
	}

	public ParticleSystem IgnitionEffects;
	public ParticleSystem FuelEffects;
	public Transform GimbalTransform;

	void FixedUpdate() {
		Rigidbody2D rb = gameObject.GetComponentInParent<Rigidbody2D> ();
		if (rb != null) {
			BasePartBehaviour tc = gameObject.GetComponent<BasePartBehaviour> ();
			Peripheral Peripheral = gameObject.GetComponent<Peripheral> ();


			gimbal = Peripheral.Ports [0];
			SetSparker = (Peripheral.Ports [1] != 0);
			Peripheral.Ports [3] = Mathf.RoundToInt (tc.Thermal.Temperature);

			float f = Peripheral.Ports [2] / 64f;
			foreach (RecourseConnection rc in thruster.requiredRecourses) {
				f = Mathf.Min (f, rc.TakeRecourse (Peripheral.Ports [2]) / 64f);
			}
			
			if (sparkerOn) {
				float t = sparker.sparkerConsumption;
				foreach (RecourseConnection rc in sparker.requiredRecourses) {
					t = Mathf.Min (t, rc.TakeRecourse (sparker.sparkerConsumption));
				}
				tc.Thermal.ThermalEnergy += t;
			}

			ParticleSystem.MainModule main = FuelEffects.main;

			if (tc.Thermal.Temperature> thruster.ignitionTemperatureThreshold) {
				rb.AddForceAtPosition (GimbalTransform.up * f * thruster.forcePerUnit, transform.position);
				tc.Thermal.ThermalEnergy += thruster.inginitionThermalEnergy * f;
				if (IgnitionEffects != null) {
					if (IgnitionEffects.isStopped) {
						IgnitionEffects.Play ();
					}
					ParticleSystem.EmissionModule em = IgnitionEffects.emission;
					main.startSpeedMultiplier = f * particleViolence;
					em.rateOverTimeMultiplier = f * particleViolence;
				}
				if (FuelEffects != null) {
					if (FuelEffects.isPlaying) {
						FuelEffects.Stop ();
					}
				}
			} else {
				rb.AddForceAtPosition (GimbalTransform.up * f, GimbalTransform.position);
				if (IgnitionEffects != null) {
					if (IgnitionEffects.isPlaying) {
						IgnitionEffects.Stop ();
					}
				}
				if (FuelEffects != null) {
					if (FuelEffects.isStopped) {
						FuelEffects.Play ();
					}
					ParticleSystem.EmissionModule em = FuelEffects.emission;
					main.startSpeedMultiplier = f * particleViolence;
					em.rateOverTimeMultiplier = f * particleViolence;
				}
			}
		}
	}
}
