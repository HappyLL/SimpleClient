using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject btn_up = GameObject.Find("Canvas/up");
        GameObject btn_down = GameObject.Find("Canvas/down");
        GameObject btn_left = GameObject.Find("Canvas/left");
        GameObject btn_right = GameObject.Find("Canvas/right");
        Button btn1 = btn_up.GetComponent<Button>();
        btn1.onClick.AddListener(up);
        Button btn2 = btn_down.GetComponent<Button>();
        btn2.onClick.AddListener(down);
        Button btn3 = btn_left.GetComponent<Button>();
        btn3.onClick.AddListener(left);
        Button btn4 = btn_right.GetComponent<Button>();
        btn4.onClick.AddListener(right);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void up(){
        Debug.Log("up");
        GameMgr.get_instance().get_input_mgr().push_input_logic("up");
    }

    public void down(){
        Debug.Log("down");
        GameMgr.get_instance().get_input_mgr().push_input_logic("down");
    }

    public void left(){
        Debug.Log("left");
        GameMgr.get_instance().get_input_mgr().push_input_logic("left");
    }

    public void right(){
        Debug.Log("right");
        GameMgr.get_instance().get_input_mgr().push_input_logic("right");
    }

}
