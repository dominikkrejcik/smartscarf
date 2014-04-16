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
	private bool wait=true;
	private int index;
	private int count;
	private int length=1374;
	private int difference = 1;
	Boolean isWriting = false;
	private byte[] newData = new byte [1373];
	private Queue<byte[]> recived_data = new Queue<byte[]>();
	
	void Start()
	{     
		Connect();
		
	}
	
	
	
	private void Connect()
	{
		//print ("About to Connect");
		client = new TcpClient ();
		client.BeginConnect (IpAddress, port, ConnectCallBack, null);
	}
	
	public void OnApplicationQuit()
	{
		
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
				return; //add exceptions
			}
			return;
		}
		
		try
		{   print ("Connected");
			NetworkStream networkStream = client.GetStream();
			client.ReceiveBufferSize =length;
			byte[] buffer = new byte[client.ReceiveBufferSize];
			//print(BitConverter.ToString(buffer));
			networkStream.BeginRead(buffer, 0, buffer.Length, ClientReadCallback, buffer);
		}
		catch (Exception ex)
		{   
			return; //add exceptions 
		}
		
	}
	
	private void  ClientReadCallback(IAsyncResult asyncResult)
	{
		try
		{   // print ("Reading Rekt");
			NetworkStream networkStream = client.GetStream();
			int read = networkStream.EndRead(asyncResult);
			//print (read+"#1");
			//print ("length: "+read);
			if (read == 0)
			{
				//print ("Data Rekt");
			}
			//if(read!=5274){
			//read = networkStream.EndRead(asyncResult);
			//	print ("tryiing");
			//	}
			byte[] buffer = asyncResult.AsyncState as byte[];
			if (buffer != null&&read>0)
			{
				byte[] data = new byte[read];
				//checkData(buffer);
				string dat = Encoding.ASCII.GetString((buffer), 0, read);
				//count=count+data.Length;
				//	print(buffer.Length+"Recieved");
				//print (buffer.Length+"Recieved"+"  "+dat+"  "+BitConverter.ToString(buffer));
				Buffer.BlockCopy(buffer, 0, data, 0, read);
				//string dat = Encoding.ASCII.GetString((data), 0, read);
				//print (buffer.Length+"Recieved"+"  "+dat+"  "+BitConverter.ToString(buffer));
				//checkData(buffer,read);
				print ("hhhsad");
				//getData(length);
				recived_data.Enqueue(data);
				//byte[] byteData =recived_data.Dequeue ();
			//	string datas = Encoding.ASCII.GetString((byteData), 0, byteData.Length);
				Array.Clear(buffer,0,buffer.Length);
				networkStream.BeginRead(buffer, 0, buffer.Length, ClientReadCallback, buffer);
				
			}
			
		}
		catch (Exception ex)
		{
			
		}
	}
	
	private void  checkData(byte[] data, int message_length)
	{   
		if("0".Equals(Encoding.ASCII.GetString((data), 0, 1)) && difference==1)
		{   
			int id=0;
			
			print (newData.Length+"ll");
			//print (message_length-1);
			//print (length-1+"sdsd");
			verifyData (data,newData,message_length-1,1);
		}
		
		
		else if(difference>1)
		{  //print ("f");
			verifyData (data,newData,message_length,0);
		}
		else if(difference==1 && "N".Equals(Encoding.ASCII.GetString((data), 0, 1)) )
		{
			print ("no one there fuck off");
			
		}
		
		
		
	}
	
	private void  verifyData (byte[] data,byte[] newData, int message_length,int offset)
	{
		if((length-1) == message_length)
		{  
			
			//print (newData.Length+"SAda");
			
			Buffer.BlockCopy(data, offset, newData, 0, message_length-1);
			//print (newData.Length+"Recieved "+BitConverter.ToString(newData));
			
			difference=newData.Length;
			
		}
		
		else if(length-1 >= message_length)
		{   print ("arrive");
			//print (difference);
			Buffer.BlockCopy(data, offset, newData, difference-1, message_length);
			difference=message_length+difference;
			//print (newData.Length+"Recieved "+BitConverter.ToString(newData));
			
		}
		
		
		
	}
	
	
	private byte[] getData(int message_length)
	{   print (difference);
		print (message_length+"KK");
		if(difference==message_length)
		{   //print("kk");
			byte[] finalData= new byte[newData.Length];
			difference=1;
			Buffer.BlockCopy(newData, 0,finalData, 0, newData.Length);
			Array.Clear(newData,0,newData.Length);
			string dat = Encoding.ASCII.GetString((finalData), 0, newData.Length);
			print (dat);
			return newData;
			
		}
		return null;
	}
	

	
	
	public void Write(byte[] bytes)
	{   
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
		byte[] buf = encoding.GetBytes ("This block of text goes on and on and does not have any visual separation between what would normally make up a new paragraph. Instead, it will continue to have more text added to it and look like it has not been formatted. It just keeps going and going and going and will start new topics without separating them with paragraphs. You see, even though this is nice, let's talk about Cascading Style Sheets (CSS), oftentimes called style sheets, for a minute.This block of text goes on and on and does not have any visual separation between what would normally make up a new paragraph. Instead, it will continue to have more text added to it and look like it has not been formatted. It just keeps going and going and going and will start new topics without separating them with paragraphs. You see, even though this is nice, let's talk about Cascading Style Sheets (CSS), oftentimes called style sheets, for a minute.This block of text goes on and on and does not have any visual separation between what would normally make up a new paragraph. Instead, it will continue to have more text added to it and look like it has not been formatted. It just keeps going and going and going and will start new topics without separating them with paragraphs. You see, even though this is nice, let's talk about Cascading Style Sheets (CSS), oftentimes called style sheets, for a minute");
		
		byte[] id = encoding.GetBytes ("1");
		byte[] buffer = new byte[ id.Length + buf.Length];
		System.Buffer.BlockCopy( id, 0, buffer, 0, id.Length );
		System.Buffer.BlockCopy( buf, 0, buffer, id.Length, buf.Length );
		//print (buffer.Length);
		NetworkStream networkStream = client.GetStream();
		//Start async write operation
		if(false == isWriting)
		{
			//print ("WRITE START " + bytes.Length);
			//networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, null);
			networkStream.BeginWrite(buffer, 0, buffer.Length, WriteCallback, null);
			isWriting = true;
		}
		
	}
	

	private void WriteCallback(IAsyncResult result)
	{
		//print ("WRITE DONE");
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
