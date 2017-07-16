using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ClientNet{
	private const int BUFF_SIZE = 1024;

	private Socket m_socket;
	private string m_socketStatus;
	private byte[] m_sendBytes;
	private byte[] m_recvBytes;
	private int m_recvcount;
	private int m_sendcount;

	public ClientNet(string ip_str="127.0.0.1", int port=8181){
		m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_socketStatus = "close";
		this.Connect (ip_str, port);

		m_sendBytes = new byte[BUFF_SIZE];
		m_recvBytes = new byte[BUFF_SIZE];
		m_recvcount = 0;
		m_sendcount = 0;
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
		m_sendcount -= cnt;
		Array.Copy (m_sendBytes, cnt, m_sendBytes, 0, m_sendcount);
		if(m_sendcount > 0)
			_sendMsg ();
	}
		
	public void SendMsg(byte[] bytes){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		m_sendcount += bytes.Length;
		m_sendBytes = m_sendBytes.Concat (bytes).ToArray();
		_sendMsg ();
	}

	public void RecvMsg(){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		byte[] tmpBytes = new byte[BUFF_SIZE];
		int cnt = m_socket.Receive (tmpBytes);
		byte[] copyBytes = new byte[cnt];
		Array.Copy (tmpBytes, 0, copyBytes, 0, cnt);
		m_recvcount += cnt;
		m_recvBytes = m_recvBytes.Concat (copyBytes).ToArray ();
	}

	public void HandleMsg(){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		int check_ret = this._CheckPackageLen ();
		if (check_ret == -1) {
			Debug.Log ("[ClientNet][HandleMsg] check ret is -1");
			return;
		}
		int content_len = check_ret;
		this.ParseHeader (content_len);
	}

	private int _CheckPackageLen(){
		if (m_recvcount == 0)
			return -1;
		int ind = 0;
		ArrayList arrL = Proto.unpack ("i", m_recvBytes, out ind);
		if (arrL == null || arrL.Count != 1) {
			return -1;
		}
		int content_len = (int)arrL [0];
		if (content_len > m_recvcount - ind) {
			Debug.Log ("[ClientNet][_CheckPackageLen] the content len > m_recvcnt");
			return -1;
		}
		return content_len;
	}

	//通过hid发送对应的bytes给对应的监听者 
	private void ParseHeader(int hLen){
		int ind;
		ArrayList arrL = Proto.unpack ("iH", m_recvBytes, out ind);
		if (arrL == null || arrL.Count != 2) {
			return;
		}
		int hid = (ushort)arrL [1];
		byte[] tmpBytes = new byte[hLen];
		ind = ind - (int)Proto.calsize ("H");
		Array.Copy (m_recvBytes, ind, tmpBytes, 0, hLen);
		m_recvcount -= hLen;
		Array.Copy (m_recvBytes, hLen, m_recvBytes, 0, m_recvcount);
		//发送数据
		NetDelegate.GetIns().DispatchEvent((ushort)hid, tmpBytes);
	}
}
