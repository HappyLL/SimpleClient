using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPosCSHeader: Header {
    public MPosCSHeader(ushort hid):base(hid){
        this.SetVal("player_id", 'i', -1);
        this.SetVal("pos_x", 'f', 0);
        this.SetVal("pos_y", 'f', 0);
        this.SetVal("v_x", 'f', 0);
        this.SetVal("v_y", 'f', 0);
    }
}
