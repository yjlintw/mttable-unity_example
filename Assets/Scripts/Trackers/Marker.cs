using UnityEngine;
using System.Collections;

public class Marker {
	public Vector2 center;
	public Marker(int id, float x, float y, float angle) {
		this.id = id;
		this.center.x = x;
		this.center.y = y;
		this.angle = angle;
	}

	public int id {
		private set; get;
	}
	public float angle {
		set; get;
	}
	public float x {
		set {
			center.x = value;
		}

		get {
			return center.x;
		}
	}

	public float y {
		set {
			center.y = value;
		}

		get {
			return center.y;
		}
	}
	

}
