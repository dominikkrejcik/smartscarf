using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour {

	public GameObject soundSource;
	public AudioClip sound;

	private bool connected = false;
	private AudioListener userAudioListener;
	private List<GameObject> sourceList = new List<GameObject>();
	private float[] samples;

	// Use thisous for initialization
	void Start () {
		userAudioListener = GetComponent<AudioListener>();

		//micStart();

		bool yey = false;

		float[] samples = new float[audio.clip.samples * audio.clip.channels];
		audio.clip.GetData(samples, 0);

		for (int i = 0; i < samples.Length; ++i)
		{
			if (samples[i] != 0f)
			{
				yey = true;
			}
		}
		byte[] ba = new byte[samples.Length * 4];

		print (yey);
	}
	
	void OnGUI() {
		
		if (!connected)
		{
			if (GUI.Button(new Rect(10, Screen.height-50, Screen.width-20, 30), "Enter room"))
			{
				Debug.Log("User wants to join");
				connect();
			}
		}
		else
		{
			if (GUI.Button(new Rect(10, Screen.height-50, Screen.width-20, 30), "Leave room"))
			{
				Debug.Log("User wants to leave");
				disconnect();
			}
		}
	}

	void micStart()
	{
		string selectedDevice = "Built-in Microphone";

		audio.clip = Microphone.Start(selectedDevice, true, 10, 44100);
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		audio.Play(); // Play the audio source!
	}
	
	void connect()
	{
		//establish a connection to the server
	
		addSoundSource(sound);
		addSoundSource(sound);
		addSoundSource(sound);

		//wait on success message
			connected = true;
			renderer.enabled = true;
			userAudioListener.enabled = true;
	}

	void disconnect()
	{
		//break connection from the server

		removeSoundSources();

		//wait on success message
			connected = false;
			renderer.enabled = false;
			userAudioListener.enabled = false;

	}

	void addSoundSource(AudioClip sound)
	{
		Vector3 posistionVector = getNewPosistionVecotr();

		GameObject source = Instantiate(soundSource, posistionVector, Quaternion.identity) as GameObject;

		sourceList.Add(source);

		AudioSource sourceComponent = source.GetComponent<AudioSource>();
		sourceComponent.PlayOneShot(sound);
	}

	Vector3 getNewPosistionVecotr()
	{
		//place sources around user
		
		return (Quaternion.Euler(0, 0, ((360f)/(sourceList.Count+1)	)) * Vector3.right * 4);

	}
	
	void removeSoundSources()
	{

		for (int i=0; i < sourceList.Count; i++)
		{
			Destroy(sourceList[i]);
		}

		sourceList.Clear();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
