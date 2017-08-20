using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncManager{
    public static int NO_SYNC = 1;
    private int m_sync_model_num;

    private Dictionary<int, List<MPosCSHeader>> m_dicNoSycCache;

    public SyncManager()
    {
        m_dicNoSycCache = new Dictionary<int, List<MPosCSHeader>>();
        m_sync_model_num = NO_SYNC;
    }

    public void init()
    {
        NetDelegate.GetIns().AddListener(HeaderConst.HEADER_POS_MSG_ID, this.update_pos);
    }

    public void uinit()
    {
        NetDelegate.GetIns().RemoveListener(HeaderConst.HEADER_POS_MSG_ID);
    }

    public int sync_model(){
        return m_sync_model_num;
    }

    public void tick(){
        if (this.m_sync_model_num == NO_SYNC)
        {
            foreach(var player_id in m_dicNoSycCache.Keys)
            {
                Debug.Log("player_id is "+player_id);
                PlayerManager.get_instance().update_target(player_id, m_dicNoSycCache[player_id]);
            }
            m_dicNoSycCache.Clear();
        }
    }

    public void update_pos(byte[] bytes)
    {
        MPosCSHeader pos_header = new MPosCSHeader(HeaderConst.HEADER_POS_MSG_ID);
        pos_header.Header_Decode(bytes);
        int player_id = (int)pos_header.GetVal("player_id");
        Debug.Log("update_pos " + player_id);
        if (m_dicNoSycCache.ContainsKey(player_id) == false)
        {
            m_dicNoSycCache[player_id] = new List<MPosCSHeader>();
        }
        m_dicNoSycCache[player_id].Add(pos_header);
    }

}
