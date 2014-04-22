using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class NetworkBehaviour : MonoBehaviour {
	
	private string  IpAddress = "54.246.121.80";
	private int port =8214;
	private TcpClient client;
	private int retry;
	private User userClass;
	private int index;
	private int count;
	private int length=8001;
	private int difference = 1;
	Boolean isWriting = false;
	private byte[] newData = new byte [8000];
	private Queue<byte[]> recived_data = new Queue<byte[]>();

	void Start()
	{     

		Connect();
		
	}
	

	
	private void Connect()
	{
		client = new TcpClient ();
		client.BeginConnect (IpAddress, port, ConnectCallBack, null);
	}
	
	private void OnApplicationQuit()
	{
//		print ("close");
		client.Close ();
	}
	
	
	
	private void ConnectCallBack(IAsyncResult asyncResult)
	{
		try
		{ 

			client.EndConnect(asyncResult);
			
		}
		catch (Exception ex)
		{
			retry++;
			if (retry==3)
			{  
				client.BeginConnect (IpAddress, port, ConnectCallBack, null);;
			}
			else
			{
				return; 
			}
			return;
		}
		
		try
		{   
			NetworkStream networkStream = client.GetStream();
			client.ReceiveBufferSize =length;
			byte[] buffer = new byte[client.ReceiveBufferSize];
			print ("Connected");
			networkStream.BeginRead(buffer, 0, buffer.Length, ClientReadCallback, buffer);
		}
		catch (Exception ex)
		{   
			return; 
		}
		
	}
	
	private void  ClientReadCallback(IAsyncResult asyncResult)
	{
		try
		{   
			NetworkStream networkStream = client.GetStream();
			int read = networkStream.EndRead(asyncResult);
		   
			byte[] buffer = asyncResult.AsyncState as byte[];
			if (buffer != null&&read>0)
			{
				byte[] data = new byte[read];
				string dat = Encoding.ASCII.GetString((buffer), 0, read);
		        Buffer.BlockCopy(buffer, 0, data, 0, read);
				recived_data.Enqueue(data);
			    Array.Clear(buffer,0,buffer.Length);
				networkStream.BeginRead(buffer, 0, buffer.Length, ClientReadCallback, buffer);
				
			}
			
		}
		catch (Exception ex)
		{
			
		}
	}


	
	public void Write(byte[] bytes, string iden)
	{   
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
	//	String iden = userClass.Identity;
//		print (iden);
	    byte[] id = encoding.GetBytes (iden);
		byte[] buffer = new byte[ id.Length + bytes.Length];
		System.Buffer.BlockCopy( id, 0, buffer, 0, id.Length );
		System.Buffer.BlockCopy( bytes, 0, buffer, id.Length, bytes.Length );
		NetworkStream networkStream = client.GetStream();

		if(false == isWriting)
		{
			networkStream.BeginWrite(buffer, 0, buffer.Length, WriteCallback, null);
			isWriting = true;
		}
		
	}
	

	private void WriteCallback(IAsyncResult result)
	{
		NetworkStream networkStream = client.GetStream();
		networkStream.EndWrite(result);
		isWriting = false;
	}
	
	public void setQueue(Queue<byte[]> queue)
	{
		recived_data = queue;
	}

	public Queue<byte[]> getQueue()
	{
		return recived_data;
	}

}
