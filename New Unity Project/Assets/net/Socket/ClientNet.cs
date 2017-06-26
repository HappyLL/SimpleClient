using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System;

public class ClientNet{
	private const int BUFF_SIZE = 1024;

	private Socket m_socket;
	private string m_socketStatus;
	private byte[] m_sendBytes;
	private byte[] m_recvBytes;
	private int m_buffcnt;

	public ClientNet(string ip_str="127.0.0.1", int port=8181){
		m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_socketStatus = "close";
		this.Connect (ip_str, port);

		m_sendBytes = new byte[BUFF_SIZE];
		m_recvBytes = new byte[BUFF_SIZE];
		m_buffcnt = 0;
	}

	public void Connect(string ip_str, int port){
		if (m_socketStatus.Equals ("connected")) {
			return;
		}
		IPAddress ipAddr = IPAddress.Parse (ip_str);
		IPEndPoint pt = new IPEndPoint(ipAddr, port);
		m_socket.Connect (pt);
		m_socketStatus = "connected";
	}
			
	public void unConnect(){
		if (m_socketStatus.Equals ("unconnected")) {
			return;
		}
		m_socket.Close ();
		m_socketStatus = "unconnected";
	}

	private void _sendMsg(){
		int cnt = m_socket.Send (m_sendBytes);
		Array.Copy (m_sendBytes, 0, m_sendBytes, cnt, m_sendBytes.Length);
	}
		
	public void SendMsg(byte[] bytes){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		m_sendBytes = m_sendBytes.Concat (bytes).ToArray();
		_sendMsg ();
	}

	public void RecvMsg(){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		byte[] tmpBytes = new byte[BUFF_SIZE];
		int cnt = m_socket.Receive (tmpBytes);
		m_recvBytes = m_recvBytes.Concat (tmpBytes).ToArray ();
	}
}
