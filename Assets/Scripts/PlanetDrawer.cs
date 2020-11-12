using UnityEngine;
using System.Collections;
using System.Linq;

public class PlanetDrawer : MonoBehaviour {
	public Vector2 RotateVector(Vector2 v, float angle)
	{
		float radian = (angle%360) *Mathf.Deg2Rad;
		float _x = v.x*Mathf.Cos(radian) - v.y*Mathf.Sin(radian);
		float _y = v.x*Mathf.Sin(radian) + v.y*Mathf.Cos(radian);
		return new Vector2(_x,_y);
	} 


	[System.Serializable]
	public class ChildNoiseSettings {
		public float NoiseIntensity;
		public float NoiseRange;
		public float NoiseSeed;
		public float Offset = 1;

		public float Calculate(float x) {
			return (Mathf.PerlinNoise (x * NoiseIntensity, NoiseSeed) * NoiseRange + Offset);
		}
	}


	[System.Serializable]
	public class NoiseSettings {
		public float NoiseIntensity;
		public float NoiseRange;
		public float NoiseSeed;
		public float Offset;

		public ChildNoiseSettings Amplification;
		public ChildNoiseSettings Intensity;

		public float Calculate(float x) {
			float i = 1;
			float a = 1;
			if (Intensity != null) {
				i = Intensity.Calculate (x);
			}
			if (Amplification != null) {
				a = Amplification.Calculate (x);
			}
			return (Mathf.PerlinNoise (x * NoiseIntensity * i, NoiseSeed) * NoiseRange * a + Offset);
		}
	}

	public NoiseSettings[] Noises;
	public float Height;

	public int PlanetVertexCount;
	public int Overdraw;

	// Use this for initialization
	void OnEnable () {
		EdgeCollider2D collider = GetComponent<EdgeCollider2D> ();
		LineRenderer lr = GetComponent<LineRenderer> ();
		Vector2[] Points = new Vector2[PlanetVertexCount + Overdraw];
		lr.positionCount = (PlanetVertexCount + Overdraw);
		for (int i = 0; i < PlanetVertexCount + Overdraw; i++) {
			Points [i] = RotateVector (new Vector2 (0, Height + Noises.Sum((Noise) => Noise.Calculate(i))), (360f * i) / PlanetVertexCount + 180f) * 0.5f;
			lr.SetPosition (i, Points [i]);
		}
		collider.points = Points;
	}
}
