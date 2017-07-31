using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager{

    private List<GameObject> m_players = null;
    public static string path = "Prefabs/PlayerPrefab";

    public PlayerManager(){
        m_players = new List<GameObject>();
        this.register_msg();
    }

    private void register_msg(){
        NetDelegate.GetIns().AddListener(HeaderConst.HEADER_LOGIN_MSG_ID, this.create_new_player);
    }

    private void cancel_msg(){
        NetDelegate.GetIns().RemoveListener(HeaderConst.HEADER_LOGIN_MSG_ID, this.create_new_player);
    }

    private void create_new_player(byte[] bytes){
        Debug.Log("create_new_player");
        MSCSLoginHeader login = new MSCSLoginHeader(HeaderConst.HEADER_LOGIN_MSG_ID);
        login.Header_Decode(bytes);
        Debug.Log(login.GetVal("player_id"));
        Debug.Log(login.GetVal("pos_x"));
        Debug.Log(login.GetVal("pos_y"));
        Object cubePreb = Resources.Load(path, typeof(GameObject));
        var gm_obj = GameObject.Instantiate(cubePreb) as GameObject;
        m_players.Add(gm_obj);
        Player comm = gm_obj.GetComponent<Player>();
        comm.set_proto_info(login);
    }
}
