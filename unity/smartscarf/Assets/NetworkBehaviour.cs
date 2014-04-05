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

			BytesRead = client.GetStream().EndRead(ar);
		}
		catch
		{
			//An error has occured when reading
			return;
		}
		
		if (BytesRead == 0)
		{
			//The connection has been closed.
			return;
		}
		
		readBuffer = ar.AsyncState as byte[];

		//string data = Encoding.ASCII.GetString(readBuffer, 0, BytesRead);
		//print(data);
		//Do something with the data object here.
		//Then start reading from the network again.
		client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);

	}

	public byte[] asdf ()
	{
		return readBuffer;
		//client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);
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
		if(readBuffer != null)
		{
			client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(read), null);
		}
	}
	

}
