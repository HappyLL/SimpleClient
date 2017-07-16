using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initializatin
	private ClientNet m_ct;
	void Start () {
		// 创立了连接
		m_ct = new ClientNet ();
		this.NetTick();
    }
		
	// 游戏主循环
	void Update () {
		this.tick ();
	}

	void NetTick(){
		// 收数据
		m_ct.RecvMsg ();
		m_ct.HandleMsg ();
	}

	void tick(){
		
	}
}
