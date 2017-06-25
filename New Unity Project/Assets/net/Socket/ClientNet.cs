using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;

public class ClientNet{
	private const int BUFF_SIZE = 1024;

	private Socket m_socket;
	private string m_socketStatus;
	private byte[] m_sendBytes;
	private int m_buffcnt;

	public ClientNet(string ip_str="127.0.0.1", string port=8181){
		m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream);
		m_socketStatus = "close";
		this.Connect (ip_str, port);

		m_sendBytes = new byte[BUFF_SIZE];
		m_buffcnt = 0;
	}

	public void Connect(string ip_str, string port){
		if (m_socketStatus.Equals ("connected")) {
			return;
		}
		IPAddress ipAddr = IPAddress.Parse (ip_str);
		IPEndPoint pt = new IPEndPoint(ipAddr, port);
		m_socket.Connect (ipAddr);
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

	}
		
	public void SendMsg(byte[] bytes){
		m_sendBytes = m_sendBytes.Concat (bytes);
		_sendMsg ();
	}

	public void RecvMsg(){
		
	}
}
