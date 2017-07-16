using UnityEngine;
using System.Collections;

public class MSCSLoginHeader : Header{
	public MSCSLoginHeader(ushort hid):base(hid){
		this.SetVal ("name", 's', "131123123124124");
		this.SetVal ("id", 'i', 1);
		this.SetVal ("height", 's', "182");
		this.SetVal ("sex", 's', "male");
		this.SetVal ("age", 'i', "18");
	}
}
