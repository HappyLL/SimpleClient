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

    }

    public int player_id()
    {
        return m_player_id;
    }

    public void update_nosync_pos(List<MPosCSHeader> list_pos)
    {
        Debug.Log("update_nosync_pos");
        foreach(var item in list_pos)
        {
            m_x = (float)item.GetVal("pos_x");
            m_y = (float)item.GetVal("pos_y");
            this.transform.position = new Vector2(m_x, m_y);
        }
    }

    public void pull_data(List<string> inputs)
    {
        float delta_x = 0f;
        float delta_y = 0f;
        foreach(string input in inputs){
            if(input == "up")
            {
                delta_y += 1;
            }
            else if(input == "down")
            {
                delta_y -= 1;
            }
            else if(input == "left")
            {
                delta_x -= 1;
            }
            else if(input == "right")
            {
                delta_x += 1;
            }
        }
        if (delta_x == 0 && delta_y == 0)
            return;
        float target_x = delta_x + m_x;
        float target_y = delta_y + m_y;
        MPosCSHeader header = new MPosCSHeader(HeaderConst.HEADER_POS_MSG_ID);
        header.SetVal("player_id", m_player_id);
        header.SetVal("pos_x", target_x);
        header.SetVal("pos_y", target_y);
        Debug.Log("player_id is "+m_player_id+" target_x " + target_x + " target_y " + target_y);
        var bin = Proto.encode_buffer(header.Header_Encode());
        var ct = GameMgr.get_instance().get_owner_client();
        if (ct!=null){
            ct.SendMsg(bin);
        }

    }
}
