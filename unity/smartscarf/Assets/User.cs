using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour {
	
	public GameObject soundSource;
	public AudioClip sound;
	
	private bool connected = false;
	private AudioListener userAudioListener;
	private NetworkBehaviour networkClass;
	private List<GameObject> sourceList = new List<GameObject>();
	private byte[] sampleBytes;
	
	
	// Use thisous for initialization
	void Start () {

		userAudioListener = GetComponent<AudioListener>();
		networkClass = GetComponent<NetworkBehaviour>();

		micActivate();
	}

	public byte[] ToByteArray(float[] floatArray) {
		
		int len = floatArray.Length * 4;
		byte[] byteArray = new byte[len];
		int pos = 0;
		
		foreach (float f in floatArray)
		{
			byte[] data = System.BitConverter.GetBytes(f);
			System.Array.Copy(data, 0, byteArray, pos, 4);
			pos += 4;
		}
		
		return byteArray;
		
	}
	
	public float[] ToFloatArray(byte[] byteArray)
	{
		
		int len = byteArray.Length / 4;
		float[] floatArray = new float[len];
		
		for (int i = 0; i < byteArray.Length; i+=4)
		{
			floatArray[i/4] = System.BitConverter.ToSingle(byteArray, i);
		}
		
		return floatArray;
		
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
	
	void micActivate()
	{
		string selectedDevice = "Built-in Microphone";
		
		audio.clip = Microphone.Start(selectedDevice, true, 50, 44100);
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		//audio.Play(); // Play the audio source!
	}
	
	byte[] bytesToSend()
	{
		float[] audioData = new float[audio.clip.samples * audio.clip.channels];
		audio.clip.GetData(audioData, 0);
		
		return ToByteArray(audioData);
		
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

	IEnumerator sendData ()
	{
		yield return new WaitForEndOfFrame();

		networkClass.SendData(bytesToSend());
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
		
		
		/*
	 * 
	 * sending:
		sampleBytes = bytesToSend();
		//then send smaplebtess off

		receiving
		//receive sample bytes

		float[] audioData = ToFloatArray(sampleBytes);
		audio.clip.SetData(audioData, 0);

	*/
		
		StartCoroutine("sendData");
		
	}
}
