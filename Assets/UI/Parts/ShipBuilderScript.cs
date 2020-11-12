using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class ShipBuilderScript : MonoBehaviour {

	public GameObject[] Parts;
	public Sprite[] Icons;
	public GameObject PartPanel;
	public GameObject PartPanelWindow;
	public GameObject CurrentPart;
	public GameObject BaseShip;
	

	// Use this for initialization
	void Start () {
		for(int i = 0; i < Parts.Length; i++) {
			if (Parts [i] != null) {
				GameObject newGameObject = Instantiate (PartPanel);
				Image image = newGameObject.transform.GetChild (0).GetComponent<Image> ();
				newGameObject.transform.SetParent (PartPanelWindow.transform);
				if (Icons [i] == null) {
					image.sprite = Parts[i].GetComponent<SpriteRenderer> ().sprite;
				} else {
					image.sprite = Icons [i];
				}
			}
		}
	}

	Vector3 Round(Vector3 v, float Base) {
		return new Vector3 (Mathf.Round (v.x / Base) * Base, Mathf.Round (v.y / Base) * Base, Mathf.Round (v.z / Base) * Base);
	}

	Vector3 Round(Vector3 v, int Base) {
		return new Vector3 (Mathf.Round (v.x / Base) * Base, Mathf.Round (v.y / Base) * Base, Mathf.Round (v.z / Base) * Base);
	}

	Vector3 Round(Vector3 v) {
		return new Vector3 (Mathf.Round (v.x), Mathf.Round (v.y), Mathf.Round (v.z));
	}

	// Update is called once per frame
	void Update () {
		if (CurrentPart != null) {
			CurrentPart.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0, 0, 8);
			Collider2D relativeCollider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint (Input.mousePosition), 2, 1);
			if (relativeCollider != null) {
				Transform relativeTransform = relativeCollider.transform;
				CurrentPart.transform.position = relativeTransform.TransformPoint (Round (relativeTransform.InverseTransformPoint (CurrentPart.transform.position), 0.5f));
				CurrentPart.transform.eulerAngles = relativeTransform.eulerAngles + Round (CurrentPart.transform.eulerAngles - relativeTransform.eulerAngles, 90f);
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				CurrentPart.transform.Rotate (new Vector3 (0, 0, 1), -90f);
			}
			if (Input.GetMouseButtonUp (0)) {
				if ((transform.position - Input.mousePosition).magnitude < 300) {
					Destroy (CurrentPart);
				} else {
					BasePartBehaviour bph = CurrentPart.GetComponent<BasePartBehaviour> ();
					foreach (BasePartBehaviour.connectionPoint v in bph.ConnectionPoints) {
						foreach (Collider2D c in Physics2D.OverlapPointAll(CurrentPart.transform.TransformPoint(v.LocalPosition))) {
							if (c.tag == "Part" && !Physics2D.IsTouching(c, bph.GetComponent<Collider2D>())) {
								CurrentPart.transform.SetParent (c.attachedRigidbody.transform);
							}
						}
					}
					if (CurrentPart.transform.parent == null) {
						GameObject newShip = Instantiate (BaseShip);
						newShip.transform.position = CurrentPart.transform.position;
						CurrentPart.transform.SetParent (newShip.transform);
					}
					CameraController cc = Camera.main.GetComponent<CameraController> ();
					cc.Target = CurrentPart.transform.parent.gameObject;
					cc.Offset = (cc.transform.position - CurrentPart.transform.parent.position);
					foreach (Collider2D collider in CurrentPart.GetComponents<Collider2D> ()) {
						collider.enabled = true;
					}
					CurrentPart = null;
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				Collider2D co = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				foreach (Collider2D c in Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition))) {
					if (co == null) {
						co = c;
					} else {
						if ((c.transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition)).magnitude < (co.transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition)).magnitude) {
							co = c;
						}
					}
				}
				if (co != null) {
					if (co.tag == "Part" && co.isActiveAndEnabled) {
						CurrentPart = co.gameObject;
						Transform oldTransform = CurrentPart.transform.parent;
						CurrentPart.transform.SetParent (null);
						foreach (Collider2D collider in CurrentPart.GetComponents<Collider2D> ()) {
							collider.enabled = false;
						}
						co.enabled = false;
						if (oldTransform.childCount < 1) {
							Destroy (oldTransform.gameObject);
						}
					}
				} else {
					foreach (Button b in PartPanelWindow.GetComponentsInChildren<Button>()) {
						if ((Input.mousePosition - b.transform.position).magnitude < 25) {
							CurrentPart = Instantiate (Parts [b.transform.GetSiblingIndex()]);
							CurrentPart.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
							foreach (Collider2D c in CurrentPart.GetComponentsInChildren<Collider2D>()) {
								c.enabled = false;
							}
						}
					}
				}
			}
		}
	}
}
