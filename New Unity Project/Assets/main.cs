using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initializatin

	void Start () {
        // 创立了连接
        GameMgr.get_instance();
    }
		
	// 游戏主循环
	void Update () {
		this.tick ();
	}

    void OnDestroy(){
        GameMgr.get_instance().uinit();
    }

	void tick(){
        GameMgr.get_instance().tick();
    }
}
