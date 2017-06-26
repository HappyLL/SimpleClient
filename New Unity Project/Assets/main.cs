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
		Debug.Log("game start");
		Header h = new Header (101);
		h.AddVal ("m_pos", 100);
    }
	
	// 游戏主循环
	void Update () {
		
	}
}
