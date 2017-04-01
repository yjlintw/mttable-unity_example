using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMarker : MarkerGO {

	Renderer m_Renderer;
	// Use this for initialization
	void Start () {
		m_Renderer = gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (highlight) {
			m_Renderer.material.color = new Color32(200, 0, 0, 255);
		} else {
			m_Renderer.material.color = new Color32(255, 255, 255, 255);
		}
	}
}
