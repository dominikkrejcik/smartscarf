using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour {

	public GameObject soundSource;
	public AudioClip sound;

	private bool connected = false;
	private AudioListener userAudioListener;
	private List<GameObject> sourceList = new List<GameObject>();

	// Use thisous for initialization
	void Start () {
		userAudioListener = GetComponent<AudioListener>();
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
	
	void connect()
	{
		//establish a connection to the server
		
		addSoundSource(sound);
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
		Vector3 posistionVector = getNeePosistionVecotr();

		GameObject source = Instantiate(soundSource, posistionVector, Quaternion.identity) as GameObject;

		sourceList.Add(source);

		AudioSource sourceComponent = source.GetComponent<AudioSource>();
		sourceComponent.PlayOneShot(sound);
	}

	Vector3 getNeePosistionVecotr()
	{
		//place sources around user

		return (Quaternion.Euler(0, 0, ((360f)/(sourceList.Count+1)	)) * Vector3.up);

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
