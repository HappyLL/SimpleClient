using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr{

    private ClientNet m_ct = null;
    private PlayerManager m_player = null;
    private SyncManager m_syncmgr = null;
    private InputManager m_ipmgr = null;

    private static GameMgr m_ins = null;

    public GameMgr(){
        m_ct = new ClientNet();
        m_player = PlayerManager.get_instance();
        m_syncmgr = new SyncManager();
        m_syncmgr.init();
        m_ipmgr = new InputManager();
    }

    public static GameMgr get_instance()
    {
        if (m_ins != null)
            return m_ins;
        m_ins = new GameMgr();
        return m_ins;
    }

    public void uinit()
    {
        m_syncmgr.uinit();
    }

	public void tick(){
        m_ipmgr.tick();
        // 收数据
        if (m_ct == null)
            return;
        m_ct.HandleMsg();
        m_syncmgr.tick();
    }

    public ClientNet get_owner_client()
    {
        return m_ct;
    }

    public InputManager get_input_mgr()
    {
        return m_ipmgr;
    }
}
