using UnityEngine;
public class MarkerGO : MonoBehaviour {

	[SerializeField]
	private bool _enabled;
	public bool isEnabled {
		private set {
			_enabled = value;
		} 
		get {
			return _enabled;
		}
	}

	[SerializeField]
	private bool _highlight;
	public bool highlight {
		private set {
			_highlight = value;
		}
		get {
			return _highlight;
		}
	}
	public Marker marker {
		set; get;
	}

	public virtual void Update() {
		if (!isEnabled) {
			return;
		}
	}

	public void setHighlight() {
		highlight = true;
	}

	public void unsetHighlight() {
		highlight = false;
	}

	public void setActive(bool value) {
		isEnabled = value;
	}
}
