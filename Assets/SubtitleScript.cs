using UnityEngine;
using System.Collections;

public class SubtitleScript : MonoBehaviour {
	public string newText = "";
	public float showTextSeconds = 3f;

	private float lastNewTextTime;
	private TextMesh subtitleTextMesh;

	// Use this for initialization
	void Start () {
		this.subtitleTextMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		float now = Time.time, sinceNewText;

		if (this.newText != "") {
			this.subtitleTextMesh.text = this.newText;
			this.newText = "";
			this.lastNewTextTime = now;
		}

		sinceNewText = now - this.lastNewTextTime;
		if (sinceNewText < .1) {
			this.subtitleTextMesh.fontSize = 18;
		} else if (sinceNewText < .12) {
			this.subtitleTextMesh.fontSize = 17;
		} else if (sinceNewText < .15) {
			this.subtitleTextMesh.fontSize = 16;
		} else if (sinceNewText < .17) {
			this.subtitleTextMesh.fontSize = 15;
		} else if (sinceNewText < this.showTextSeconds) {
			this.subtitleTextMesh.fontSize = 14;
		} else {
			this.subtitleTextMesh.text = "";
		}
	}
}
