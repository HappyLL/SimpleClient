using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager{

    private Dictionary<int, Player> m_players = null;
    public static string path = "Prefabs/PlayerPrefab";
    private Player m_owner;

    private static PlayerManager m_ins = null;

    public PlayerManager(){
        m_players = new Dictionary<int, Player>();
        m_owner = null;
        this.register_msg();
    }

    public static PlayerManager get_instance()
    {
        if (m_ins != null)
            return m_ins;
        m_ins = new PlayerManager();
        return m_ins;
    }

    public Player get_owner(){
        return m_owner;
    }

    private void register_msg(){
        NetDelegate.GetIns().AddListener(HeaderConst.HEADER_LOGIN_MSG_ID, this.create_new_player);
    }

    private void cancel_msg(){
        NetDelegate.GetIns().RemoveListener(HeaderConst.HEADER_LOGIN_MSG_ID, this.create_new_player);
    }

    private void create_new_player(byte[] bytes){
        //Debug.Log("create_new_player");
        MSCSLoginHeader login = new MSCSLoginHeader(HeaderConst.HEADER_LOGIN_MSG_ID);
        login.Header_Decode(bytes);
        //Debug.Log(login.GetVal("player_id"));
        //Debug.Log(login.GetVal("pos_x"));
        //Debug.Log(login.GetVal("pos_y"));
        Object cubePreb = Resources.Load(path, typeof(GameObject));
        var gm_obj = GameObject.Instantiate(cubePreb) as GameObject;
        Player comm = gm_obj.GetComponent<Player>();
        comm.set_proto_info(login, m_owner == null);
        m_players[comm.player_id()] = comm;
        if (m_owner == null)
        {
            m_owner = comm;
        }
    }

    public void update_target(int player_id ,List<MPosCSHeader> list_pos)
    {
        //m_players.Cin
        if (m_players.ContainsKey(player_id) == false || list_pos == null)
            return;
        
        m_players[player_id].update_nosync_pos(list_pos);
    }
}
