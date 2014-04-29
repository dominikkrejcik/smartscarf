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
	public Texture2D[] profiles;
	public GUISkin Login, Enter, Exit;
	
	private bool connected = false;
	private AudioListener userAudioListener;
	private AudioSource playBackAudio;
	private NetworkBehaviour networkClass;
	private SoundManager soundManagerClass;
	int sada;
	int x=0;
	private int length=32001;
	private int difference = 1;
	private byte[] newData = new byte [32000];
	private byte[] backup ;//= new byte [8000];
	private int id;
	bool testBool = false;
	bool backUpData=false;
	private Boolean check=false;
	private Boolean logIn=false;
	private Boolean complete=false;
	private Queue<byte[]> put0data = new Queue<byte[]>();
	private Queue<byte[]> put1data = new Queue<byte[]>();
	private Queue<byte[]> put2data = new Queue<byte[]>();
	private Queue<byte[]> send_data = new Queue<byte[]>();
	string inputString="";
	string saveString="";
	public  string Identity="";
	bool start=false;
	public int index=0;
	private Boolean room =false;

	
	float[] audioData;

	// Use thisous for initialization
	void Start () {
			
		userAudioListener = GetComponent<AudioListener>();
		networkClass = GetComponent<NetworkBehaviour>();
		soundManagerClass = GetComponent<SoundManager>();

		micActivate();

		InvokeRepeating("bytesToSend", 0f, 1f);
	    InvokeRepeating ("testFunc", 0f,    0.2f);


	

		audioData  = new float[audio.clip.samples * audio.clip.channels];
		print (audioData.Length);
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

		float width = (Screen.width*(26f/32f));
		float height = (Screen.height/6)*5;

		Rect rect = new Rect(Screen.width/2-(width/2), height, width, Screen.height/10);

		if(logIn==false)
		{
			GUI.skin = Login;
			inputString = GUI.TextField(new Rect(Screen.width/2-(width/2), Screen.height/3, width, Screen.height/10), inputString, 200);

			if (Event.current.Equals(Event.KeyboardEvent("None"))&& inputString != "")
			{
				doneTyping(inputString);
			}

			if (GUI.Button(rect,"") && inputString != "")
			{   
				doneTyping(inputString);
			}
			
		}
		else if(logIn==true)
		{
			joinRoom(rect);
		}
	}

	void doneTyping(string input)
	{
		if(inputString.Equals("Nej"))
		{
			Identity="0";
		}
		else if(inputString.Equals("Dom"))
		{
			Identity="1";
		}
		
		else if(inputString.Equals("Ed"))
		{
			Identity="2";
		}

		renderer.material.mainTexture = profiles[int.Parse(Identity)];
		logIn = true;
	}
	
	void joinRoom(Rect rect)
	{
		if (!connected)
		{
			GUI.skin = Enter;
			if (GUI.Button(rect,""))
			{
				//			Debug.Log("User wants to join");
				networkClass.Connect();
				connect();
				room=true;
			}
		}
		else
		{
			GUI.skin = Exit;
			if (GUI.Button(rect,""))
			{
				//	Debug.Log("User wants to leave");
				networkClass.Quit();
				disconnect();
				room=false;
			}
		}
	}
	

	
	void micActivate()
	{
		string selectedDevice = "Built-in Microphone";
		
		audio.clip = Microphone.Start(selectedDevice, true, 1, 8000);
		audio.loop = true; // so it does not cut off!!! :D :D 
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		//audio.Play(); // Play the audio source!
	}

	void bytesToSend()
	{   if (room)
		{
		audio.clip.GetData(audioData, 0);

//		print ((ToByteArray(audioData)).Length);

		send_data.Enqueue(ToByteArray(audioData));
		}
	}
	
	void connect()
	{
		//establish a connection to the server

		//wait on success message
		connected = true;
		renderer.enabled = true;
		userAudioListener.enabled = true;
		soundManagerClass.enabled = true;

		testBool = true;
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

  void  testFunc()
	{    
	      if (put0data.Count > 0&&put1data.Count>0) 
		{
			//print (put0data.Count);
			print ("01");
			byte[] send0 = put0data.Dequeue();
			float[] testArr0 = ToFloatArray (send0);
			soundManagerClass.receiveFloats (testArr0, 0);
			byte[] send1 = put1data.Dequeue();
			float[] testArr1 = ToFloatArray (send1);
			soundManagerClass.receiveFloats (testArr1, 1);
		    testBool = false;
		
		}

		else if (put2data.Count > 0&&put1data.Count>0) 
		{

			print ("21");
			byte[] send1 = put1data.Dequeue();
			float[] testArr1 = ToFloatArray (send1);
			soundManagerClass.receiveFloats (testArr1, 1);
			byte[] send2 = put2data.Dequeue();
			float[] testArr2 = ToFloatArray (send2);
			soundManagerClass.receiveFloats (testArr2, 2);
			testBool = false;
		}

		else if (put0data.Count > 0&&put2data.Count>0) 
		{

			print ("02");
			byte[] send0 = put0data.Dequeue();
			float[] testArr0 = ToFloatArray (send0);
			soundManagerClass.receiveFloats (testArr0, 0);
			byte[] send2 = put2data.Dequeue();
			float[] testArr2 = ToFloatArray (send2);
			soundManagerClass.receiveFloats (testArr2, 2);
			testBool = false;
		}

		else if (put0data.Count > 0) 
		{
			print ("0");
			byte[] send0 = put0data.Dequeue();
			float[] testArr0 = ToFloatArray (send0);
			soundManagerClass.receiveFloats (testArr0, 0);
		
			testBool = false;
		}

		else if (put1data.Count > 0) 
		{
			print ("1");
			byte[] send1 = put1data.Dequeue();
			float[] testArr1 = ToFloatArray (send1);
			soundManagerClass.receiveFloats (testArr1, 1);
			
			testBool = false;
		}
		else if (put2data.Count > 0) 
		{
			print ("2");
			byte[] send2 = put2data.Dequeue();
			float[] testArr2 = ToFloatArray (send2);
			soundManagerClass.receiveFloats (testArr2, 2);
			
			testBool = false;
		}
			


	}

	void send()
	{     
		if (send_data.Count > 0)
		{


			networkClass.Write(send_data.Dequeue(),Identity);
		}
	}

	private void  checkData(byte[] data, int message_length)
	{   //  print ("in");

		if("0".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			id=0;
		
			verifyData (data,newData,message_length-1,1);
		}
		if("1".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			id=1;

			verifyData (data,newData,message_length-1,1);
		}

		if("2".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			id=2;
			
			verifyData (data,newData,message_length-1,1);
		}

		
		else if(difference>1&&complete==false)
		{  

			verifyData (data,newData,message_length,0);
		}
		else if(difference==1 && "N".Equals(Encoding.ASCII.GetString((data), 0, 1)) )
		{
		//	print ("no one there fuck off");
			
		}
		
		
		
	}
	
	private void  verifyData (byte[] data,byte[] newData, int message_length,int offset)
	{   
		check=false;
		if((length-1) == message_length)
		{  

			Buffer.BlockCopy(data, offset, newData, 0, message_length-1);
			difference=newData.Length+difference;
			check=true;
			complete=true;
		}
		
		else if(length-1 >= message_length)
		{   

			if(difference+message_length<=32001)
			{
			Buffer.BlockCopy(data, offset, newData, difference-1, message_length);
			difference=message_length+difference;
	
			check=false;
			complete=true;
			}
			else if(difference+message_length>32001)
			{   
			
				int delta=(32001-difference);
				int copy= data.Length-delta;
	
				backup =new byte[copy];
				Buffer.BlockCopy(data,offset,newData,difference-1,delta);
				Buffer.BlockCopy(data,delta,backup,0,copy);
				difference=difference+delta;
			
				backUpData=true;
				check=false;
				complete=true;
			}
		}
		
		
		
	}
	
	
	private byte[] getData(int message_length)
	{     
		if(difference==message_length)
		{  
			byte[] finalData= new byte[newData.Length];
			difference=1;
			Buffer.BlockCopy(newData, 0,finalData, 0, newData.Length);
			Array.Clear(newData,0,newData.Length);
	

		    if(backUpData==true)
			{  
			
				checkData(backup,backup.Length);
				backUpData=false;
			}

			check= false;
			complete=false;
			return finalData;
			
		}
		return null;
	}



	IEnumerator get() {
		if(check==false)
		{
			
		}
		Queue<byte[]> recived_data = networkClass.getQueue();
		
		if(recived_data.Count>0&&check==false)
		{
			byte[] byteData =recived_data.Dequeue ();
			
			checkData(byteData,byteData.Length);
			
			complete=false;
			byte [] lastData= getData (length);
			if (lastData !=null){
				
				if(id==0)
				{
					put0data.Enqueue(lastData);
				}
				else if(id==1)
				{
					put1data.Enqueue(lastData);
				}
				else if(id==2)
				{
					put2data.Enqueue(lastData);
				}

				
				
				
				return null;
			}
		}
		return null;
	}
	// Update is called once per frame
	void Update () {

		byte[] lel = new byte[] {0,1,0,1,0};

		if(connected)
		{   

			StartCoroutine ("get");
			send ();

		
		}


	

}


}