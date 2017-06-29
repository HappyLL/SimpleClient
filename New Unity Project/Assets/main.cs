using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initializatin
	private ClientNet m_ct;
	void Start () {
		// 创立了连接
		//m_ct = new ClientNet ();
		if (test ())
			return;
		Debug.Log("game start");
		Header h = new Header (101);
		h.SetVal ("m_pos", 'i', 100);
		h.SetVal ("m_x", 'd', 100.00);
		h.SetVal ("m_y", 'd', 100.00);
		h.SetVal ("m_ss", 's', "adasdasdasdasd");
		h.SetVal ("m_j", 'I', 111);
		h.SetVal ("m_sss", 's', "asdasdasdasd");
		h.SetVal ("m_ssss", 'i', 1001);
		h.SetVal ("m_sssss", 's', "100000000");
		var val = h.GetVal ("m_pos");
		Debug.Log(h.GetHeaderFmt ());
		byte[] bytes = h.Header_Encode ();
		h.Header_Decode (bytes);
    }

	bool test(){
		return false;
	}

	// 游戏主循环
	void Update () {
		
	}
}
