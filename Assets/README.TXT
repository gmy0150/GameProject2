CARTOON MUMBLER VOICE PACK - VOLUME 1
BY SEESPACE LABS

The main thing here is the audio files. There are 135 WAV files covering a full range of character moods. You can find tons of documentation online for playing audio clips in Unity since it’s a straightforward and typical thing to do.

We’ve also included an example project that you can fiddle with. The source code here is not planned as a reusable component, and it isn’t a necessary part of the voice pack. But you’re welcome to use it, of course!

* * *
USING VOICE PACK IN YOUR UNITY PROJECT

An easy way to do it: import the Voice Pack into your project from the Unity Asset Store, and then delete everything except for the Resources/audio directory so you just have the WAV files.

Another method would be to import into an empty project so that you have the WAV files on your hard drive. Then open your Unity Project and copy the WAV files into the “Resources” directory using normal file management, e.g. drag from Finder to Unity on a Mac.

* * *
MOOD DESCRIPTIONS

Some of these “moods” are not so much moods as speech patterns. Each is described below so you can pick which ones to use better. We’ve standardized on these moods across multiple mumble voice packs so that other mumble voice packs can be used interchangeably with the same game code.

* OKAY - a short positive acknowledgment, e.g. “okay”, “sure”
* DOUBT - a short negative noise indicating doubt or minor disagreement, e.g. “naw”, “unh-uh”
* PROMPT - a short noise prompting someone else to continue conversation, e.g. “go on”, “yes?”
* AGREE - substantial agreement, positivity, or confidence
* DISAGREE - strong disagreement, protest, or disgust
* CONFUSED - uncertainty, wonder, dawning thought - neutral in tone
* NORMAL - an ordinary statement, a plain fact
* INTERESTED - recounting a story, saying something important
* MUSE - pondering, wistful, fanciful - positive in tone
* CAUTIOUS - calmly delivered warning, e.g. “heed my words”
* ALERT - a quick exclamation of danger, e.g. “look out!”
* TOUGH - determined, solemn, serious, a call to arms
* ASK - asking a simple question
* DEMAND - interrogating, demanding something, e.g. “Get off my lawn!”
* SUSPICIOUS - calmly distrustful
* CONTENT - pleased, peaceful, giving a happy ending speech
* AMUSED - enjoying the moment, laughing
* JOY - a short burst coinciding with sudden happiness or insight
* BORED - bored, depressed, sullen
* SAD - heartbroke, remorseful, regretful
* COOL - slick, sexy, scheming, hushed tones
* NERVOUS - uncomfortable, afraid, tense but still keeping control
* TERRIFIED - lost control, freaked out
* SMUG - authoritative, pompous, sure of oneself
* IRRITATED - uncomfortable, frustrated, dealing with it
* ANGRY - rage, hate, attacking
* SNOTTY - insult, sarcasm

Filename convention is:

  CHARACTERNAME_MOOD-VARIATION.wav

For example:

  imp_angry-3.wav

* * *
A FEW TIPS AND IDEAS FOR USING THE VOICE PACK

The code in the sample is inefficient in how it loads and plays audio clips each time the character talks. A better way to do it is to instance and load the audio clips once when the game starts and reuse these instances to play the clips again. Similarly, it’s a better use of resources to instance one or more AudioSource objects that correspond with game characters/GameObjects, instead of using the AudioSource.PlayClipAtPoint() as in the sample code.

If you want to have more than one character voice, e.g. two mumbling characters conversing with each other, a simple way to do that is set up AudioSource instances with pitch-shifting so that one voice is lower or higher than the other. If you’ve got a need for more distinct voices than this technique affords, you could look for our other voice packs, or contact us about recording some more for your project. (See Voice Acting Services section below)

Each mood has five variations to it. This is to help the mumbling sound more realistic over repeated use. Your game code can randomize to pick different variations. If you are using subtitle text, there is a nice trick to ensure that one subtitle text is always mumbled with the same audio clip. (giving the illusion of some true language behind the mumbling) Take the subtitle text and calculate a sum of all the characters in the string, then do a modulo operation ( % operator ) on it to get a number between 1 and 5. Use that number to concatenate a filename.

* * *
SUPPORT

Email us at support@seespacelabs.com, and let us know the name of the product and the issue you have. Don’t be too angry too quick. We’re gonna handle it, my friend!

* * *
YOUR PRODUCT IDEAS

What kind of product would you like to see us release next? More Mumble voice packs? Reusable components for voice animation? Send an email to info@seespacelabs.com and let us know. We might have the thing you are looking for already. Or you might inspire us to release a new product with your idea.

* * *
VOICE ACTING SERVICES

We’ve got a sweet recording space, a talented group of professional actors, and consistent quality standards. Seespace Labs does all kinds of voice and audio work for games and animation. Would you like more mumbling voices? Would you like some exclusive content that is unique to your project? Want to use real dialogue instead of mumbling? By all means, let’s talk! Have a look at our website ( http://seespacelabs.com ) for more info about our unique services.

* * *
FINALLY

If you make a game with our voice pack, let us know! You don’t have to, of course, but it would be cool to know about your project and be a little proud about contributing to it in a small way. We might even be able to promote your game on our website.

Good luck to you in your work. Keep coding, writing, drawing, composing, testing… whatever it takes! Don’t stop until the thing is built.

-Erik Hermansen
Producer, Seespace Labs
