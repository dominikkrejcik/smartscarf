using UnityEngine;
using System.Collections;

public class User : MonoBehaviour {

	private bool connected = false;
	private AudioListener userAudioListener;

	// Use this for initialization
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

		//wait on success message
			connected = true;
			renderer.enabled = true;
			userAudioListener.enabled = true;
	}

	void disconnect()
	{
		//break connection from the server

		
		//wait on success message
			connected = false;
			renderer.enabled = false;
			userAudioListener.enabled = false;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
