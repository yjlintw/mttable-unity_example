using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;





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
		JSONObject alljo = new JSONObject(JSONObject.Type.OBJECT);
		// Marker
		JSONObject mjos = new JSONObject(JSONObject.Type.OBJECT);
		foreach(MarkerStruct ms in fakeMarkerList) {
			JSONObject mjo = new JSONObject(JSONObject.Type.OBJECT); 
			mjo.AddField("id", ms.id);
			JSONObject po = new JSONObject(JSONObject.Type.OBJECT);
			po.AddField("x", ms.pos.x);
			po.AddField("y", ms.pos.y);
			mjo.AddField("center", po);
			mjo.AddField("angle", ms.angle);
			mjo.AddField("action", ms.action);
			mjos.AddField(ms.id.ToString(), mjo);
		}
		alljo.AddField("markers", mjos);


		// Finger Point
		
		JSONObject fjos = new JSONObject(JSONObject.Type.OBJECT);
		foreach(FingerStruct fs in fakeFingerList) {
			JSONObject fjo = new JSONObject(JSONObject.Type.OBJECT);
			fjo.AddField("id", fs.id);
			fjo.AddField("x", fs.pos.x);
			fjo.AddField("y", fs.pos.y);
			fjo.AddField("action", fs.action);
			fjos.AddField(fs.id.ToString(), fjo);
		}

		if (useMouse) {
			JSONObject fjo = new JSONObject(JSONObject.Type.OBJECT);
			fjo.AddField("id", mouseFinger.id);
			fjo.AddField("x", mouseFinger.pos.x);
			fjo.AddField("y", mouseFinger.pos.y);
			fjo.AddField("action", mouseFinger.action);
			fjos.AddField(mouseFinger.id.ToString(), fjo);	
		}

		alljo.AddField("fingers", fjos);	

		return alljo.ToString();
	}

	public void updatePointers() {
		th.fakeMsg = formulateJSONMessage();
		// Debug.Log(formulateJSONMessage());
	}
}
