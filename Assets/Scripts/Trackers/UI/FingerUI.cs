using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerUI : MonoBehaviour {
	public Text textUI;
	private Finger finger;
	public void SetFinger(Finger f) {
		finger = f;
		textUI.text = "Finger ID: " + finger.id.ToString();

	}
	
}
