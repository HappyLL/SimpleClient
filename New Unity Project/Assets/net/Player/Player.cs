using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private int m_player_id;
    private float m_x;
    private float m_y;
    public Text m_tt;
	// Use this for initialization
	void Start () {
        m_tt = gameObject.GetComponentInChildren<Text>();
        m_tt.text = "Player_id: "+ m_player_id.ToString();
       //# Debug.Log("yesyeyeyeyeyeye"+m_player_id);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void set_proto_info(MSCSLoginHeader login){
        m_x = (float)login.GetVal("pos_x");
        m_y = (float)login.GetVal("pos_y");
        this.transform.position = new Vector2(m_x, m_y);
        m_player_id = (int)login.GetVal("player_id");
    }

}
