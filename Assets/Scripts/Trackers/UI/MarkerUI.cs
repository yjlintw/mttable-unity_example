using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarkerUI : MonoBehaviour {
	public Text textUI;
	private Marker marker;

	public void SetMarker(Marker m) {
		marker = m;
		textUI.text = "Marker ID: " + marker.id.ToString();
	}
}
