using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager{
    private List<string> m_ipcache;

    public InputManager(){
        m_ipcache = new List<string>();
    }

    public void push_input_logic(string input)
    {
        var owner = PlayerManager.get_instance().get_owner();
        if (owner == null)
            return;
        m_ipcache.Add(input);
    }

    public void tick(){
        var owner = PlayerManager.get_instance().get_owner();
        if (owner == null)
            return;
        owner.pull_data(m_ipcache);
        m_ipcache.Clear();
    }
}
