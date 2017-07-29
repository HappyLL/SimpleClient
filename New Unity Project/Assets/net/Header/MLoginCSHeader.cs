using UnityEngine;
using System.Collections;

public class MSCSLoginHeader : Header{
	public MSCSLoginHeader(ushort hid):base(hid){
		this.SetVal ("player_id", 'i', 1);
        this.SetVal("pos_x", 'f', 0);
        this.SetVal("pos_y", 'f', 0);

    }
}
