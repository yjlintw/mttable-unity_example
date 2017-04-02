using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHotWords : MonoBehaviour {

	void OnEnable() {
		Messenger.AddListener<bool>( TrackerHubEvent.HOT_WORDS, OnHotwordChanged );
	}

	void OnDisable() {
		Messenger.RemoveListener<bool>( TrackerHubEvent.HOT_WORDS, OnHotwordChanged );
	}

	void OnHotwordChanged(bool value) {
		if (value) {
			Camera.main.backgroundColor = new Color32(255, 100, 100, 255);
		} else {
			Camera.main.backgroundColor = new Color32(0, 0, 0, 255);
		}
	}

}
