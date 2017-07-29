using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetDelegate{
	public delegate void net_listener(byte[] bytes);
	private Dictionary<ushort, net_listener> m_dlisten;

	public static NetDelegate m_ins = null;
	private NetDelegate(){
		m_dlisten = new Dictionary<ushort, net_listener> ();
	}

	public static NetDelegate GetIns(){
		if (m_ins != null)
			return m_ins;
		m_ins = new NetDelegate ();
		return m_ins;
	}

	public void AddListener(ushort hid, net_listener listen){
		if (m_dlisten.ContainsKey (hid) == false)
			m_dlisten [hid] = listen;
		else
			m_dlisten [hid] += listen;
	}

	public void RemoveListener(ushort hid, net_listener listen=null){
		if (m_dlisten.ContainsKey (hid) == false)
			return;
        if (listen == null)
            m_dlisten.Remove(hid);
        else
            m_dlisten[hid] -= listen;
	}

	public void DispatchEvent(ushort hid, byte[] bytes){
		if (m_dlisten.ContainsKey (hid))
			m_dlisten [hid] (bytes);
	}
}
