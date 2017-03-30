using UnityEngine;
using System.Collections;

public class Finger {

	
	public Finger(int id, float x, float y) {
		this.id = id;
		this.x = x;
		this.y = y;
	}
	public int id {
		private set; get;
	}
	public float x {
		set; get;
	}
	public float y {
		set; get;
	}

}
