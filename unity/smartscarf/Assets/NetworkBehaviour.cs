using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;

public class NetworkBehaviour : MonoBehaviour {
	
	// Use this for initialization
	
	
	const int READ_BUFFER_SIZE = 255;
	
	public TcpClient client;
	private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
	public string strMessage;
	private IAsyncResult ar;

	private Socket m_Socket;
	void Start () {
		//client = new TcpClient ("54.246.121.80", 8214);
		client=new TcpClient("54.246.121.80", 8214);
		
		
	}
	private void ConnectCallback(IAsyncResult result)
	{
		try
		{
			client.EndConnect(result);
		}
		catch
		{
			//Increment the failed connection count in a thread safe way
			//Interlocked.Increment(ref failedConnectionCount);
			//if (failedConnectionCount >= addresses.Length)
			{
				//We have failed to connect to all the IP Addresses
				//connection has failed overall.
				return;
			}
		}
		
		//We are connected successfully.
		NetworkStream networkStream = client.GetStream();
		byte[] buffer = new byte[client.ReceiveBufferSize];
		//Now we are connected start asyn read operation.
		//networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
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

		}
	}
	
	
	
	
	public void SendData(byte[] Snoop)
	{

		Write(Snoop);
	}
	
	public void Write(byte[] bytes)
	{
		NetworkStream networkStream = client.GetStream();
		//Start async write operation
		networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, null);
	}
	
	/// <summary>
	/// Callback for Write operation
	/// </summary>
	/// <param name="result">The AsyncResult object</param>
	private void WriteCallback(IAsyncResult result)
	{
		NetworkStream networkStream = client.GetStream();
		networkStream.EndWrite(result);
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		// Start an asynchronous read invoking DoRead to avoid lagging the user
		// interface.
		//client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);
		
	}
	

}
