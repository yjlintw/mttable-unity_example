using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEditor;


[CustomPropertyDrawer(typeof(MarkerDict))]

public class MarkerDictDrawer : DictionaryDrawer<int, MarkerGO> { }

[System.Serializable] public class MarkerDict : SerializableDictionary<int, MarkerGO> { }

[System.Serializable]
public class IDPrefabMap<T> {
	int id;
	T prefab;
}

public class TrackerHub : MonoBehaviour {
	public Funnel.Funnel funnel;
	public UDPReceiver input;
	public Canvas canvas;

	// Prefabs for fingers and markers
	public FingerUI fingerPointPrefab;
	public MarkerGO defaultMarkerPrefab;


	// Containers for active fingerpoints and markers
	private Dictionary<int, FingerUI> fingerPoints;
	private Dictionary<int, MarkerGO> markerGOs;
	public MarkerDict markerPrefabDict;
	public bool simulation = false;
	public string fakeMsg;
	
	public MarkerGO activeMarker;

	// float lastUpdateTime;
	// const float CLEARTIME = 0.5f;

	// For Debug
	private bool hotwordflag = false;
	// Use this for initialization
	void Start () {
		fingerPoints = new Dictionary<int, FingerUI>();
		markerGOs = new Dictionary<int, MarkerGO>();
	}
	
	// Update is called once per frame
	void Update () {
		string newMsg = "";
		if (simulation) {
			newMsg = fakeMsg;
		} else {
			newMsg = input.getLatestUDPPacket();
		}
		if (newMsg != string.Empty) {
			JSONNode jo = JSON.Parse(newMsg);
			if (jo["fingers"] != null) {
				updateFingers((JSONClass)jo["fingers"]);
			} else {
			}

			if (jo["markers"] != null) {
				updateMarkers((JSONClass)jo["markers"]);
			} else {
			}
		}

		// Debug
		if (Input.GetKeyDown(KeyCode.H)) {
			hotwordflag = !hotwordflag;
			setHotword(hotwordflag);
		}
	}
	void updateFingers(JSONClass fingers) {
		foreach (string k in fingers.keys) {
			int id = fingers[k]["id"].AsInt;
			float x = fingers[k]["x"].AsFloat;
			float y = funnel.screenHeight - fingers[k]["y"].AsFloat;
			int action = fingers[k]["action"].AsInt;
			Finger finger = new Finger(id, x, y);
			FingerUI fingerUI;
			RectTransform t;
			switch (action) {
				case 1:
				case 2:
					if (fingerPoints.ContainsKey(id)) {
						fingerUI = fingerPoints[id];
					} else {
						fingerUI = GameObject.Instantiate<FingerUI>(fingerPointPrefab);
						fingerUI.transform.SetParent(canvas.transform, false);
						fingerUI.SetFinger(finger);
						fingerPoints[id] = fingerUI;
					}
					t = fingerUI.GetComponent<RectTransform>();
					t.anchoredPosition = new Vector2(x, y);
					break;
				case 3:
					if (fingerPoints.ContainsKey(id)) {
						fingerUI = fingerPoints[id];
						fingerUI.gameObject.SetActive(false);
						GameObject.DestroyImmediate(fingerUI.gameObject);
						fingerPoints.Remove(id);
					}
					break;
			}
		}
		List<int> keys = new List<int>(fingerPoints.Keys);	
		foreach(var key in keys) {
			if (fingers[key.ToString()] == null) {
				FingerUI fingerUI = fingerPoints[key];
				fingerUI.gameObject.SetActive(false);
				GameObject.DestroyImmediate(fingerUI.gameObject);
				fingerPoints.Remove(key);
			}
		}
	}

	void updateMarkers(JSONClass markers) {
		foreach (string k in markers.keys) {
			int id = markers[k]["id"].AsInt;
			float x = markers[k]["center"]["x"].AsFloat;
			float y = funnel.screenHeight - markers[k]["center"]["y"].AsFloat;
			float angle = markers[k]["angle"].AsFloat;
			int action = markers[k]["action"].AsInt;
			Marker marker = new Marker(id, x, y, angle);
			MarkerGO markerGO;
			// RectTransform t;
			Transform trans;
			switch (action) {
				case 1:
				case 2:
					if (markerGOs.ContainsKey(id)) {
						markerGO = markerGOs[id];
						markerGO.gameObject.SetActive(true);
					} else {
						MarkerGO markerPrefab = defaultMarkerPrefab;
						if (markerPrefabDict.ContainsKey(id) && markerPrefabDict[id]) {
							markerPrefab = markerPrefabDict[id];
						}
						markerGO = GameObject.Instantiate<MarkerGO>(markerPrefab);
						markerGOs[id] = markerGO;
						markerGO.marker = marker;
					}
					trans = markerGO.transform;
					Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10));
					trans.position = pos;
					trans.eulerAngles = new Vector3(0, 0, angle);
					break;
				case 3:
					if (markerGOs.ContainsKey(id)) {
						markerGO = markerGOs[id];
						markerGO.gameObject.SetActive(false);
					}
					break;
				
			}
		}
	}

	void clearAllMarkers() {
		List<int> keys = new List<int>(markerGOs.Keys);	
		foreach(var key in keys) {
			MarkerGO markerGO = markerGOs[key];
			GameObject.DestroyImmediate(markerGO.gameObject);
		}
		markerGOs.Clear();
	}

	void clearAllFingers() {
		List<int> keys = new List<int>(fingerPoints.Keys);	
		foreach(var key in keys) {
			FingerUI fingerUI = fingerPoints[key];
			GameObject.DestroyImmediate(fingerUI.gameObject);
		}

		fingerPoints.Clear();
	}

	public void setActiveMarker(int id) {
		MarkerGO go = markerGOs[id];
		if (go != null) {
			activeMarker = go;
			activeMarker.setHighlight();
		}
	}

	public void removeActiveMarker() {
		if (activeMarker) {
			activeMarker.unsetHighlight();
			activeMarker = null;
		}
	}

	public void setHotword(bool value) {
		Messenger.Broadcast<bool>(TrackerHubEvent.HOT_WORDS, value);
	}
}


