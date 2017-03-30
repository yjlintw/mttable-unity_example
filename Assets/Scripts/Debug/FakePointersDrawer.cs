using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FakePointers))]
public class FakePointersDrawer : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		FakePointers fpScript = (FakePointers)target;
		if (GUILayout.Button("Update")) {
			fpScript.updatePointers();
		}
	}
}
