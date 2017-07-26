using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initializatin
	private ClientNet m_ct;
	void Start () {
		// 创立了连接
        MSCSLoginHeader lg = new MSCSLoginHeader(101);
        byte[] ret = Proto.encode_buffer(lg.Header_Encode());
        m_ct = new ClientNet();
        m_ct.SendMsg(ret);
    }
		
	// 游戏主循环
	void Update () {
		this.tick ();
	}

	void NetTick(){
        // 收数据
        if (m_ct == null)
            return;
		m_ct.HandleMsg ();
    }

	void tick(){
        this.NetTick();
    }
}
