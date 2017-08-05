using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private int m_player_id;
    private float m_x;
    private float m_y;
    private bool m_self;
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

    public void set_proto_info(MSCSLoginHeader login, bool is_self){
        m_x = (float)login.GetVal("pos_x");
        m_y = (float)login.GetVal("pos_y");
        this.transform.position = new Vector2(m_x, m_y);
        m_player_id = (int)login.GetVal("player_id");
        m_self = is_self;

        NetDelegate.GetIns().AddListener(HeaderConst.HEADER_POS_MSG_ID, this.update_pos);
    }

    public void update_pos(byte[] bytes)
    {
        MPosCSHeader pos_header = new MPosCSHeader(HeaderConst.HEADER_POS_MSG_ID);
        pos_header.Header_Decode(bytes);
        int player_id = (int)pos_header.GetVal("player_id");
        if (m_player_id != player_id)
            return;
        m_x = (float)pos_header.GetVal("pos_x");
        m_y = (float)pos_header.GetVal("pos_y");
        this.transform.position = new Vector2(m_x, m_y);
    }

}
