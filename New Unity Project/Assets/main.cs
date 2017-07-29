using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initializatin
	private ClientNet m_ct;
    private PlayerManager m_player;
	void Start () {
        // 创立了连接
        m_ct = new ClientNet();
        m_player = new PlayerManager();
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
