using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

[System.Serializable]
public class MarkerStruct {
	public int id;
	public Vector2 pos;
	public float angle;
	public int action;

	public JSONClass toJSON() {
		JSONClass jc = new JSONClass();
		jc["id"] = new JSONNode();
		jc["id"].AsInt = this.id;
		jc["center"] = new JSONClass();
		jc["center"]["x"] = new JSONNode();
		jc["center"]["x"].AsFloat = this.pos.x;
		jc["center"]["y"] = new JSONNode();
		jc["center"]["y"].AsFloat = this.pos.y;
		jc["angle"] = new JSONNode();
		jc["angle"].AsFloat = this.angle;
		jc["action"] = new JSONNode();
		jc["action"].AsInt = this.action; 

		return jc;
	}
	
}
