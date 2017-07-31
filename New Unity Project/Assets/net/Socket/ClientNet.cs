using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ClientNet{
	private const int BUFF_SIZE = 1024;
    static object locker = new object();
	private Socket m_socket;
	private string m_socketStatus;
	private byte[] m_sendBytes = { };
	private byte[] m_recvBytes = { };
    private byte[] m_tmpBytes;
	private int m_recvcount;
	private int m_sendcount;

	public ClientNet(string ip_str="127.0.0.1", int port=8888){
		m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_socketStatus = "close";
        m_recvcount = 0;
        m_sendcount = 0;
        m_tmpBytes = new byte[BUFF_SIZE];
        this.Connect (ip_str, port);
	}

	public void Connect(string ip_str, int port){
		if (m_socketStatus.Equals ("connected")) {
			return;
		}
		IPAddress ipAddr = IPAddress.Parse (ip_str);
		IPEndPoint pt = new IPEndPoint(ipAddr, port);
		m_socket.Connect (pt);
		m_socketStatus = "connected";
        this.RecvMsg();
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
        Debug.Log(cnt);
		m_sendcount -= cnt;
		Array.Copy (m_sendBytes, cnt, m_sendBytes, 0, m_sendcount);
        if (m_sendcount > 0)
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
        m_socket.BeginReceive(m_tmpBytes, 0, m_tmpBytes.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallBack), m_socket);
	}

    public void ReceiveCallBack(IAsyncResult ret) {
        lock (locker){
            int cnt = m_socket.EndReceive(ret);
           // Debug.Log("recvcnt is " + cnt);
            byte[] copyBytes = new byte[cnt];
            for (int index = 0; index < cnt; ++index)
            {
                Debug.Log(m_tmpBytes[index]);
            }
            Array.Copy(m_tmpBytes, 0, copyBytes, 0, cnt);
            m_recvcount += cnt;
            Debug.Log("recvcnt is " + m_recvcount);
            m_recvBytes = m_recvBytes.Concat(copyBytes).ToArray();
            //Debug.Log("rec");
            for (int index = 0; index < m_recvBytes.Length; ++index)
            {
               Debug.Log(m_recvBytes[index]);
            }
            m_tmpBytes = new byte[BUFF_SIZE];
            m_socket.BeginReceive(m_tmpBytes, 0, m_tmpBytes.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallBack), m_socket);
        }
    }

	public void HandleMsg(){
		if (this.m_socketStatus == "unconnected") {
			return;
		}
		int check_ret = this._CheckPackageLen ();
        //Debug.Log("handle_msg is " + check_ret);
        if (check_ret == -1) {
			//Debug.Log ("[ClientNet][HandleMsg] check ret is -1");
			return;
		}
		int content_len = check_ret;
        //Debug.Log("hehehe");
		this.ParseHeader (content_len);
	}

	private int _CheckPackageLen(){
        lock (locker){
            if (m_recvcount == 0)
                return -1;
            //Debug.Log("check m_recvcount is "+m_recvcount);
            int ind = 0;
            ArrayList arrL = Proto.unpack("i", m_recvBytes, out ind);
            if (arrL == null || arrL.Count != 1)
            {
                return -1;
            }
            int content_len = (int)arrL[0];
            //Debug.Log("content_len is " + content_len);
            //Debug.Log("recvcount is " + m_recvcount);
            //Debug.Log("ind is " + ind);
            if (content_len > m_recvcount - ind)
            {
                //Debug.Log("[ClientNet][_CheckPackageLen] the content len > m_recvcnt");
                return -1;
            }
            return content_len;
        }
	}

	//通过hid发送对应的bytes给对应的监听者 
	private void ParseHeader(int hLen){
        lock (locker){
            int ind;
            ArrayList arrL = Proto.unpack("iH", m_recvBytes, out ind);
            if (arrL == null || arrL.Count != 2)
            {
                return;
            }
            //Debug.Log("hLen is " + hLen);
            int hid = (ushort)arrL[1];
            byte[] tmpBytes = new byte[hLen];
            ind = ind - (int)Proto.calsize("H");
            Array.Copy(m_recvBytes, ind, tmpBytes, 0, hLen);
            int tot_len = hLen + (int)Proto.calsize("i");
            m_recvcount -= tot_len;
            Debug.Log("parser m_recvcount is " + m_recvcount);
            Array.Copy(m_recvBytes, tot_len, m_recvBytes, 0, m_recvcount);
            //发送数据
            NetDelegate.GetIns().DispatchEvent((ushort)hid, tmpBytes);
        }
	}
}
