using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

[System.Serializable]
public class FingerStruct {
	public int id;
	public Vector2 pos;
	public int action;

	public JSONNode toJSON() {
		JSONClass jc = new JSONClass();
		jc["id"] = new JSONNode();
		jc["id"].AsInt = this.id;
		jc["x"] = new JSONNode();
		jc["x"].AsFloat = this.pos.x;
		jc["y"] = new JSONNode();
		jc["y"].AsFloat = this.pos.y;
		jc["action"] = new JSONNode();
		jc["action"].AsInt = this.action; 

		return jc;
	}


}
