using UnityEngine;
using System.Collections;

public class HeadScript : MonoBehaviour {
	//Inspector vars.
	public enum EyebrowState {
		normal, mean, sad
	}
	public EyebrowState eyebrowState;
	public enum LidState {
		normal, crazy, cool
	}
	public LidState eyelidState;
	[Range(0.0f, 1.0f)]
	public float mouthOpenPercent = 0.5f;
	public string eyeTargets;
	public bool isSmiling = false;
	public bool isShowingBroccoli = false;

	//Private vars.
	private GameObject normalMouth, smilingMouth, lids, leftBrow, rightBrow, eyes, broccoli;
	private string eyeTargetQueue = "";

	// Use this for initialization
	void Start () {
		this.normalMouth = GameObject.Find("mouth");
		this.smilingMouth = GameObject.Find("smileMouth");
		this.lids = GameObject.Find("lids");
		this.leftBrow = GameObject.Find("leftBrow");
		this.rightBrow = GameObject.Find("rightBrow");
		this.eyes = GameObject.Find("eyes");
		this.broccoli = GameObject.Find("broccoli");
	}
	
	// Update is called once per frame
	void Update () {
		updateMouth();
		updateLids();
		updateBrows();
		updateEyes();
		updateBroccoli();
	}

	private void updateMouth() {
		const float MIN_MOUTH_OPEN = 0.1f;
		const float MAX_MOUTH_OPEN = 1.0f;
		GameObject mouth;
		if (this.isSmiling) {
			this.normalMouth.GetComponent<Renderer>().enabled = false;
			this.smilingMouth.GetComponent<Renderer>().enabled = true;
			mouth = this.smilingMouth;
		} else {
			this.normalMouth.GetComponent<Renderer>().enabled = true;
			this.smilingMouth.GetComponent<Renderer>().enabled = false;
			mouth = this.normalMouth;
		}
		Vector3 v = mouth.transform.localScale;
		v.y = this.mouthOpenPercent;
		if (v.y < MIN_MOUTH_OPEN) {
			v.y = MIN_MOUTH_OPEN;
		} else if (v.y > MAX_MOUTH_OPEN) {
			v.y = MAX_MOUTH_OPEN;
		}
		mouth.transform.localScale = v;
	}

	private void updateLids() {
		Vector3 destPosition;
		if (this.eyelidState == LidState.cool) {
			destPosition = new Vector3(-0.025f, 0.473f, 0.5f);
		} else if (this.eyelidState == LidState.crazy) {
			destPosition = new Vector3(-0.025f, 0.524f, 0.5f);
		} else { //normal
			destPosition = new Vector3(-0.025f, 0.5f, 0.5f);
		}
		const float LID_CHANGE_SPEED = .5f;
		float step = LID_CHANGE_SPEED * Time.deltaTime;
		this.lids.transform.localPosition = Vector3.MoveTowards(
			this.lids.transform.localPosition, destPosition, step);
	}

	private void updateBrows() {
		updateLeftBrow();
		updateRightBrow();
	}

	private void updateLeftBrow() {
		Vector3 destPosition;
		if (this.eyebrowState == EyebrowState.mean) {
			destPosition = new Vector3(0f, 0f, 350f);
		} else if (this.eyebrowState == EyebrowState.sad) {
			destPosition = new Vector3(0f, 0f, 10f);
		} else { //normal
			destPosition = new Vector3(0f, 0f, 0f);
		}
		this.leftBrow.transform.localEulerAngles = destPosition;
	}

	private void updateRightBrow() {
		Vector3 destPosition;
		if (this.eyebrowState == EyebrowState.mean) {
			destPosition = new Vector3(0f, 0f, 10f);
		} else if (this.eyebrowState == EyebrowState.sad) {
			destPosition = new Vector3(0f, 0f, 350f);
		} else { //normal
			destPosition = new Vector3(0f, 0f, 0f);
		}
		this.rightBrow.transform.localEulerAngles = destPosition;
	}

	private void updateEyes() {
		if (this.eyeTargets != null && this.eyeTargets != "") {
			this.eyeTargetQueue = this.eyeTargets;
			this.eyeTargets = "";
		}
		if (this.eyeTargetQueue == "")
			return;

		Vector3 destPosition;
		string nextTarget = this.eyeTargetQueue.Substring(0,1);
		if (nextTarget=="7") {
			destPosition = new Vector3(-0.057f, 0.428f, 0.75f);
		} else if (nextTarget=="8") {
			destPosition = new Vector3(-0.022f, 0.428f, 0.75f);
		} else if (nextTarget=="9") {
			destPosition = new Vector3(-0.003f, 0.428f, 0.75f);
		} else if (nextTarget=="4") {
			destPosition = new Vector3(-0.057f, 0.405f, 0.75f);
		} else if (nextTarget=="5") {
			destPosition = new Vector3(-0.022f, 0.405f, 0.75f);
		} else if (nextTarget=="6") {
			destPosition = new Vector3(-0.003f, 0.405f, 0.75f);
		} else if (nextTarget=="1") {
			destPosition = new Vector3(-0.057f, 0.367f, 0.75f);
		} else if (nextTarget=="2") {
			destPosition = new Vector3(-0.022f, 0.367f, 0.75f);
		} else if (nextTarget=="3") {
			destPosition = new Vector3(-0.003f, 0.367f, 0.75f);
		} else {
			Debug.LogError("unexpected target - " + nextTarget);
			return;
		}
			
		const float CHANGE_SPEED = .2f;
		float step = CHANGE_SPEED * Time.deltaTime;
		this.eyes.transform.localPosition = Vector3.MoveTowards(
			this.eyes.transform.localPosition, destPosition, step);

		if (this.eyes.transform.localPosition == destPosition) {
			this.eyeTargetQueue = this.eyeTargetQueue.Substring(1);
		}
	}

	private void updateBroccoli() {
		Vector3 destPosition;
		float changeSpeed;
		if (this.isShowingBroccoli) {
			destPosition = new Vector3(.762f, .355f, 11);
			changeSpeed = .15f;
		} else {
			destPosition = new Vector3(-0.125f, .173f, 11);
			changeSpeed = .5f;
		}
		float step = changeSpeed * Time.deltaTime;
		this.broccoli.transform.localPosition = Vector3.MoveTowards(
			this.broccoli.transform.localPosition, destPosition, step);
	}
		
}
