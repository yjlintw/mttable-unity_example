using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using MATT_JSON;
using SimpleJSON;





[RequireComponent(typeof(TrackerHub))]
public class FakePointers : MonoBehaviour {
	public MarkerStruct[] fakeMarkerList;
	public FingerStruct[] fakeFingerList;
	public bool useMouse = false;
	[SerializeField]
	private FingerStruct mouseFinger;
	// Use this for initialization
	private TrackerHub th;
	void Start() {
		th = gameObject.GetComponent<TrackerHub>();
		mouseFinger = new FingerStruct();
		mouseFinger.id = 0;
		mouseFinger.pos = new Vector2(0, 0);
		mouseFinger.action = 3;
	}

	void Update() {
		if (useMouse) {
			if (Input.GetMouseButton(0)) {
				mouseFinger.action = 1;
				mouseFinger.pos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			} else {
				mouseFinger.action = 3;
			}
			updatePointers();
		}
	}

	private string formulateJSONMessage() {
		JSONClass alljo = new JSONClass();
		// Marker
		// JSONObject mjos = new JSONObject(JSONObject.Type.OBJECT);
		alljo["markers"] = new JSONClass();
		foreach(MarkerStruct ms in fakeMarkerList) {
			string id_str = ms.id.ToString();
			alljo["markers"][id_str] = ms.toJSON();
		}


		// Finger Point
		alljo["fingers"] = new JSONClass();
		// JSONObject fjos = new JSONObject(JSONObject.Type.OBJECT);
		foreach(FingerStruct fs in fakeFingerList) {
			string id_str = fs.id.ToString();
			alljo["fingers"][id_str] = fs.toJSON();
		}

		if (useMouse) {
			string id_str = mouseFinger.id.ToString();
			alljo["fingers"][id_str] = mouseFinger.toJSON();
		}

		// alljo.AddField("fingers", fjos);	

		return alljo.ToString();
	}

	public void updatePointers() {
		th.fakeMsg = formulateJSONMessage();
		// Debug.Log(formulateJSONMessage());
	}
}
