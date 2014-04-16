using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;


public class User : MonoBehaviour {

	public AudioClip sound;
	
	private bool connected = false;
	private AudioListener userAudioListener;
	private AudioSource playBackAudio;
	private NetworkBehaviour networkClass;
	private SoundManager soundManagerClass;
	int sada;
	private int length=1374;
	private int difference = 1;
	private byte[] newData = new byte [1373];
	private int id;
	bool testBool = false;
	private Boolean check=false;
	private Boolean complete=false;

	// Use thisous for initialization
	void Start () {
		
		userAudioListener = GetComponent<AudioListener>();
		networkClass = GetComponent<NetworkBehaviour>();
		soundManagerClass = GetComponent<SoundManager>();

		micActivate();

		//InvokeRepeating("testFunc", 0f, 1f);

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
		
		for (int i = 0; i < byteArray.Length / 4; i++)
		{
			floatArray[i] = System.BitConverter.ToSingle(byteArray, i * 4);
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
		
		audio.clip = Microphone.Start(selectedDevice, true, 1, 10000);
		audio.loop = true; // so it does not cut off!!! :D :D 
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		//audio.Play(); // Play the audio source!
	}
	
	byte[] bytesToSend()
	{
		float[] audioData = new float[audio.clip.samples * audio.clip.channels];
		audio.clip.GetData(audioData, 1);

		return ToByteArray(audioData);
		
	}
	
	void connect()
	{
		//establish a connection to the server

		//wait on success message
		connected = true;
		renderer.enabled = true;
		userAudioListener.enabled = true;
		soundManagerClass.enabled = true;

		//testBool = true;
	}
	
	void disconnect()
	{
		//break connection from the server

		
		//wait on success message
		soundManagerClass.removeSoundSources();

		connected = false;
		renderer.enabled = false;
		userAudioListener.enabled = false;
		soundManagerClass.enabled = false;

		//testBool = false;
	}

	void testFunc()
	{
		if (testBool)
		{
			float[] testArr = ToFloatArray(bytesToSend());
			
			soundManagerClass.receiveFloats(testArr, 0);
		}

	}

	private void  checkData(byte[] data, int message_length)
	{   

		if("0".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			id=0;
			
			//print (newData.Length+"ll");
			//print (message_length-1);
			//print (length-1+"sdsd");
			verifyData (data,newData,message_length-1,1);
		}
		if("1".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			id=1;
			
			//print (newData.Length+"ll");
			//print (message_length-1);
			//print (length-1+"sdsd");
			verifyData (data,newData,message_length-1,1);
		}
		
		else if(difference>1&&complete==false)
		{  // print ("f");

			verifyData (data,newData,message_length,0);
		}
		else if(difference==1 && "N".Equals(Encoding.ASCII.GetString((data), 0, 1)) )
		{
		//	print ("no one there fuck off");
			
		}
		
		
		
	}
	
	private void  verifyData (byte[] data,byte[] newData, int message_length,int offset)
	{
		if((length-1) == message_length)
		{  
			
			//print (newData.Length+"SAda");
			
			Buffer.BlockCopy(data, offset, newData, 0, message_length-1);
			//print (newData.Length+"Recieved "+BitConverter.ToString(newData));
			//print (difference);
			difference=newData.Length+difference;
			//print (difference);
			check=true;
			complete=true;
		}
		
		else if(length-1 >= message_length)
		{  //  print ("arrive");
			//print (difference);
			Buffer.BlockCopy(data, offset, newData, difference-1, message_length);
			difference=message_length+difference;
			//print (newData.Length+"Recieved "+BitConverter.ToString(newData));
			check=false;
			complete=true;
		}
		
		
		
	}
	
	
	private byte[] getData(int message_length)
	{   //print (difference);
		//print (message_length+"KK");
		if(difference==message_length)
		{   //print("kk");
			byte[] finalData= new byte[newData.Length];
			difference=1;
			Buffer.BlockCopy(newData, 0,finalData, 0, newData.Length);
			Array.Clear(newData,0,newData.Length);
			//string dat = Encoding.ASCII.GetString((finalData), 0, newData.Length);
			print (finalData.Length+" " +id);
			check= false;
			complete=false;
			return newData;
			
		}
		return null;
	}
	IEnumerator get() {
		if(check==false)
		{
		//print (networkClass.recived_data.Count);
		}
		if(networkClass.recived_data.Count>0&&check==false)
		{
			byte[] byteData =networkClass.recived_data.Dequeue ();
//			print (byteData.Length);
			//string dat = Encoding.ASCII.GetString((byteData), 0, byteData.Length);
			//print (dat);
			checkData(byteData,byteData.Length);
//			print ("HI");
			complete=false;
			 getData(length);
			return null;
		}
		return null;
	}
	// Update is called once per frame
	void Update () {

		byte[] lel = new byte[] {0,1,0,1,0};
		
		//networkClass.Write(bytesToSend());
		networkClass.Write(lel);
		StartCoroutine ("get");
		//print (networkClass.recived_data.Count );

}


}