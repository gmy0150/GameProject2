using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menuScript : MonoBehaviour {
	private Dictionary<string,int> moodCurrentVariations = new Dictionary<string,int>();
	private Vector3 cameraVector;
	private const int SAMPLE_COUNT = 4096;
	private const float MOUTH_ANIMATE_INTERVAL = .05f;
	private float timeSinceLastMouthAnimate;
	private float[] samples = new float[SAMPLE_COUNT];
	private HeadScript head;
	private SubtitleScript subtitle;
	private const float DEMO_NOT_STARTED = -1f;
	private int demoStepNo = -1;
	private float demoStartTime = DEMO_NOT_STARTED;
	private float lastPlayingVolume = 0f;
	private GameObject background;
	private Vector3 backgroundInitialScale;

	void Start () {
		this.timeSinceLastMouthAnimate = Time.time;
		this.cameraVector = new Vector3(0,0,-10);
		this.head = GameObject.Find("headObject").GetComponent<HeadScript>();
		this.subtitle = GameObject.Find("subtitleObject").GetComponent<SubtitleScript>();
		this.background = GameObject.Find("background");
		this.backgroundInitialScale = this.background.transform.localScale;
	}

	void Update () {
		float now = Time.time;
		if (now - this.timeSinceLastMouthAnimate > MOUTH_ANIMATE_INTERVAL) {
			this.timeSinceLastMouthAnimate = now;
			this.lastPlayingVolume = getCurrentPlayingVolume();
			this.head.mouthOpenPercent = this.lastPlayingVolume * 2;
		}

		if (this.demoStartTime != DEMO_NOT_STARTED) {
			updateForDemo();
		}

		updateBackground();
	}
		
	void OnGUI() {
		if (this.demoStartTime != DEMO_NOT_STARTED) return;

		// Make a background box
		GUI.Box(new Rect(10,10,295,350), "LET'S GET READY TO MUMBLE!");

		if(GUI.Button(new Rect(20,40,85,20), "Okay")) {
			
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "65";
			this.head.isSmiling = false;
			this.subtitle.newText = "Okay.";
			mumble("okay");
		}
		if(GUI.Button(new Rect(115,40,85,20), "Doubt")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "75";
			this.head.isSmiling = false;
			this.subtitle.newText = "Not so sure.";
			mumble("doubt");
		}
		if(GUI.Button(new Rect(210,40,85,20), "Prompt")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "45";
			this.head.isSmiling = false;
			this.subtitle.newText = "Yeah?";
			mumble("prompt");
		}
			
		if(GUI.Button(new Rect(20,70,85,20), "Agree")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "65";
			this.head.isSmiling = true;
			this.subtitle.newText = "Of course.";
			mumble("agree");
		}
		if(GUI.Button(new Rect(115,70,85,20), "Disagree")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "35";
			this.head.isSmiling = false;
			this.subtitle.newText = "I don't think so!";
			mumble("disagree");
		}
		if(GUI.Button(new Rect(210,70,85,20), "Confused")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			this.head.eyeTargets = "7915";
			this.head.isSmiling = false;
			this.subtitle.newText = "What is this place?";
			mumble("confused");
		}

		if(GUI.Button(new Rect(20,100,85,20), "Normal")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "45";
			this.head.isSmiling = false;
			this.subtitle.newText = "I live in a town.";
			mumble("normal");
		}
		if(GUI.Button(new Rect(115,100,85,20), "Interested")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "65";
			this.head.isSmiling = false;
			this.subtitle.newText = "Let me tell you about broccoli.";
			mumble("interested");
		}
		if(GUI.Button(new Rect(210,100,85,20), "Muse")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "9785";
			this.head.isSmiling = false;
			this.subtitle.newText = "Let me rethink this...";
			mumble("muse");
		}

		if(GUI.Button(new Rect(20,130,85,20), "Cautious")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "465";
			this.head.isSmiling = false;
			this.subtitle.newText = "Don't put your finger in the hole.";
			mumble("caution");
		}
		if(GUI.Button(new Rect(115,130,85,20), "Alert")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "5";
			this.head.isSmiling = false;
			this.subtitle.newText = "Look out! Spiders!";
			mumble("alert");
		}
		if(GUI.Button(new Rect(210,130,85,20), "Tough")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "65";
			this.head.isSmiling = false;
			this.subtitle.newText = "We will climb the mountain.";
			mumble("tough");
		}

		if(GUI.Button(new Rect(20,160,85,20), "Ask")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "45";
			this.head.isSmiling = false;
			this.subtitle.newText = "Can I have a napkin?";
			mumble("ask");
		}
		if(GUI.Button(new Rect(115,160,85,20), "Demand")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "5";
			this.head.isSmiling = false;
			this.subtitle.newText = "Give me your napkin!";
			mumble("demand");
		}
		if(GUI.Button(new Rect(210,160,85,20), "Suspicious")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "1345";
			this.head.isSmiling = false;
			this.subtitle.newText = "You said you'd come alone.";
			mumble("suspicious");
		}

		if(GUI.Button(new Rect(20,190,85,20), "Content")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "85";
			this.head.isSmiling = true;
			this.subtitle.newText = "Mmm, hammock nap.";
			mumble("content");
		}
		if(GUI.Button(new Rect(115,190,85,20), "Amused")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.isSmiling = true;
			this.subtitle.newText = "Two ears? Ridiculous!";
			mumble("amused");
		}
		if(GUI.Button(new Rect(210,190,85,20), "Joy")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "95";
			this.head.isSmiling = true;
			this.subtitle.newText = "I have an idea!";
			mumble("joy");
		}

		if(GUI.Button(new Rect(20,220,85,20), "Bored")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "235";
			this.head.isSmiling = false;
			this.subtitle.newText = "Nothing to do, nowhere to go-ho.";
			mumble("bored");
		}
		if(GUI.Button(new Rect(115,220,85,20), "Sad")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			this.head.eyeTargets = "215";
			this.head.isSmiling = false;
			this.subtitle.newText = "They killed my broccoli!";
			mumble("sad");
		}
		if(GUI.Button(new Rect(210,220,85,20), "Cool")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "745";
			this.head.isSmiling = false;
			this.subtitle.newText = "Time for you and me to get sexy.";
			mumble("cool");
		}

		if(GUI.Button(new Rect(20,250,85,20), "Nervous")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			this.head.eyeTargets = "4315";
			this.head.isSmiling = false;
			this.subtitle.newText = "Spiders? I don't like spiders.";
			mumble("nervous");
		}
		if(GUI.Button(new Rect(115,250,85,20), "Terrified")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			this.head.eyeTargets = "7265";
			this.head.isSmiling = false;
			this.subtitle.newText = "Oh no! It's the spiders!";
			mumble("terrified");
		}
		if(GUI.Button(new Rect(210,250,85,20), "Smug")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "65";
			this.head.isSmiling = false;
			this.subtitle.newText = "Lucky I'm here to protect us.";
			mumble("smug");
		}

		if(GUI.Button(new Rect(20,280,85,20), "Irritated")) {
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "135";
			this.head.isSmiling = false;
			this.subtitle.newText = "No more secret smells.";
			mumble("irritated");
		}
		if(GUI.Button(new Rect(115,280,85,20), "Angry")) {
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "5";
			this.head.isSmiling = false;
			this.subtitle.newText = "You! It was you the whole time!";
			mumble("angry");
		}
		if(GUI.Button(new Rect(210,280,85,20), "Snotty")) {
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "465";
			this.head.isSmiling = false;
			this.subtitle.newText = "Ooh! Mr. Big Man Hooty Tooty!";
			mumble("snotty");
		}

		GUI.Label (new Rect (20, 305, 275, 20), "Click buttons multiple times to hear variations.");

		if (GUI.Button(new Rect(210, 330, 85, 20), "Demo")) {
			this.demoStepNo = -1;
			this.demoStartTime = Time.time;
		}
	}

	private void mumble(string mood) {
		const int MAX_VARIATIONS = 5; //For this voice pack, each mood has 5 variations.
		
		//Get next variation# for the specified mood. This increments the variation# each time user clicks
		//on a mumble button.
		int variationNo = 0;
		if (this.moodCurrentVariations.TryGetValue(mood, out variationNo)) {
			if (++variationNo > MAX_VARIATIONS) {
				variationNo = 1;
			}
		} else {
			variationNo = 1;
		}
		this.moodCurrentVariations[mood] = variationNo;

		//Concat filename for clip.
		string filename = "audio/imp_" + mood + "-" + variationNo;

		//This is an inefficient, but simple way to load an audioclip and play it. 
		AudioClip clip = Resources.Load(filename, typeof(AudioClip)) as AudioClip;
		AudioSource.PlayClipAtPoint(clip, this.cameraVector);

	}
		
	private float getCurrentPlayingVolume() {
		AudioListener.GetOutputData(this.samples, 0);
		float sum = 0;
		int readSampleCount = this.samples.Length;
		if (readSampleCount > SAMPLE_COUNT) readSampleCount = SAMPLE_COUNT;
		for (int i = 0; i < readSampleCount; ++i) {
			sum += (this.samples[i] * this.samples[i]);
		}
		return Mathf.Sqrt(sum / readSampleCount);
	}

	private void updateForDemo() {
		//If in the middle of mumble, don't change demo step.
		if (this.lastPlayingVolume > 0f)
			return;

		int wasStepNo = this.demoStepNo;
		float elapsed = Time.time - this.demoStartTime;
		
		if (elapsed < 2f) {
			this.demoStepNo = 0;
		} else if (elapsed < 3f) {
			this.demoStepNo = 1;
		}else if (elapsed < 5f) {
			this.demoStepNo = 2;
		}else if (elapsed < 6f) {
			this.demoStepNo = 3;
		}else if (elapsed < 10f) {
			this.demoStepNo = 4;
		}else if (elapsed < 12f) {
			this.demoStepNo = 5;
		}else if (elapsed < 15f) {
			this.demoStepNo = 6;
		}else if (elapsed < 17f) {
			this.demoStepNo = 7;
		}else if (elapsed < 19f) {
			this.demoStepNo = 8;
		}else if (elapsed < 21f) {
			this.demoStepNo = 9;
		}else {
			this.demoStartTime = DEMO_NOT_STARTED;
			return;
		}

		if (wasStepNo == this.demoStepNo)
			return;

		if (this.demoStepNo == 0) {
			this.subtitle.newText = "It's true.";
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "45";
			this.head.isSmiling = false;
			this.head.isShowingBroccoli = false;
			mumble("normal");
		} else if (this.demoStepNo == 1) {
			this.subtitle.newText = "I'm a mumbler.";
			this.head.eyeTargets = "8";
			mumble("normal");
		} else if (this.demoStepNo == 2) {
			this.subtitle.newText = "You think that's a problem?";
			this.head.eyeTargets = "5";
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			mumble("ask");
		} else if (this.demoStepNo == 3) {
			this.subtitle.newText = "Well, deal with it.";
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyeTargets = "465";
			mumble("snotty");
		} else if (this.demoStepNo == 4) {
			this.subtitle.newText = "Cause I'm not changing for you!";
			this.head.eyelidState = HeadScript.LidState.crazy;
			mumble("tough");
		} else if (this.demoStepNo == 5) {
			this.subtitle.newText = "Hey, one thing though...";
			this.head.eyelidState = HeadScript.LidState.normal;
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyebrowState = HeadScript.EyebrowState.normal;
			this.head.eyeTargets = "46465";
			mumble("interested");
		} else if (this.demoStepNo == 6) {
			this.subtitle.newText = "Did I ever show you my magic broccoli?";
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyeTargets = "26";
			this.head.isShowingBroccoli = true;
			mumble("cool");
		} else if (this.demoStepNo == 7) {
			this.subtitle.newText = "It's so amazing!";
			this.head.eyelidState = HeadScript.LidState.crazy;
			this.head.eyeTargets = "98";
			this.head.isSmiling = true;
			mumble("joy");
		} else if (this.demoStepNo == 8) {
			this.subtitle.newText = "You don't want to see it?";
			this.head.eyebrowState = HeadScript.EyebrowState.sad;
			this.head.eyeTargets = "5";
			this.head.isShowingBroccoli = false;
			this.head.isSmiling = false;
			mumble("demand");
		} else if (this.demoStepNo == 9) {
			this.subtitle.newText = "Fine! Your loss.";
			this.head.eyebrowState = HeadScript.EyebrowState.mean;
			this.head.eyelidState = HeadScript.LidState.cool;
			this.head.eyeTargets = "1";
			mumble("angry");
		} 
	}

	private void updateBackground() {
		float now = Time.time;
		float framePos = now % 1;
		float adjustScale;
		if (framePos > .5f) {
			adjustScale = (1 - framePos) * .05f;
		} else {
			adjustScale = framePos * .05f;
		}
		Vector3 scale = this.backgroundInitialScale;
		scale.x += adjustScale;
		scale.y += adjustScale;
		this.background.transform.localScale = scale;
	}
}
