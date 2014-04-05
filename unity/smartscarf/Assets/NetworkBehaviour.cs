using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class NetworkBehaviour: MonoBehaviour {
	
	// Use this for initialization
	
	
	const int READ_BUFFER_SIZE = 255;
	
	private TcpClient client;
	private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
	public string strMessage;
	private IAsyncResult ar;
	private Socket m_Socket;
	void Start () {
		client = new TcpClient ("54.246.121.80", 8214);
		
	}
	
	
	
	
	private void read(IAsyncResult ar)
	{ 
		int BytesRead;
		try
		{
			// Finish asynchronous read into readBuffer and return number of bytes read.
			BytesRead = client.GetStream().EndRead(ar);
			if (BytesRead < 1) 
			{
				// if no bytes were read server has close.  
				
				return;
			}
			// Convert the byte array the message was saved into, minus two for the
			
			strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead);
			
			print(strMessage);//+"uud");
			
			// Start a new asynchronous read into readBuffer.
			client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);
			
		} 
		catch
		{
			;
		}
	}
	
	
	
	
	public IEnumerator SendData(Byte[] data)
	{
		BinaryWriter writer = new BinaryWriter(client.GetStream());
		writer.Write(data);
		writer.Flush();
		
		yield return null;
	}
	
	
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		// Start an asynchronous read invoking DoRead to avoid lagging the user
		// interface.
		//client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);
		
	}

}

