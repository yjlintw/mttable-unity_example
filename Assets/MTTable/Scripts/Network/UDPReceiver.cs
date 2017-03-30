using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using SimpleJSON;

public class UDPReceiver : MonoBehaviour {
	Thread receiveThread;
	UdpClient client;

	public int port = 11999;
	public string lastReceivedUDPPacket="";
	public string receivedUDPPacket="";
	public string allReceivedUDPPackets="";

	private static void Main() {
		UDPReceiver receiverObj = new UDPReceiver();
		receiverObj.init();
		string text = "";

		do {
			text = Console.ReadLine();
		} while (!text.Equals("exit"));
	}

	// Use this for initialization
	void Start () {
		init();
		 Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void init() {
		print("UDPSend.init()");
		port = 11999;
		print("Sending to n127.0.0.1:" + port);
		print("Test-Sending to this Port: nc -u n127.0.0.1 " + port + "");
		
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}
	

	private void ReceiveData() {
		client = new UdpClient(port);
		while (true) {
			try {
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				
				string text = Encoding.UTF8.GetString(data);
				//  print(">> " + text);
				lastReceivedUDPPacket = text;
				StringProcessing(lastReceivedUDPPacket);
				allReceivedUDPPackets = allReceivedUDPPackets + text;
			} catch (Exception err) {
				print(err.ToString());
			}
		}
	}

	public string getLatestUDPPacket() {
		if (allReceivedUDPPackets != string.Empty) { 
			allReceivedUDPPackets="";
			return lastReceivedUDPPacket;
		} else {
			return string.Empty;
		}
	}

	private void StringProcessing(string input) {
		// Debug.Log(input);
		// var N = JSON.Parse(input);
		// Debug.Log(N.ToString(""));

	}
}
