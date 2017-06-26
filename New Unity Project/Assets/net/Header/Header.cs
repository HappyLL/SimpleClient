using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class Header{
	public ushort m_hid;

	public Header(ushort hid){
		m_hid = hid;
	}

	public byte[] Header_Encode(){
		return null;
	}
		
	public void AddVal(string field, object val){
		Type Ts = this.GetType();
		if (val.GetType() != Ts.GetField (field).FieldType) {
			val = Convert.ChangeType(val, Ts.GetField(field).FieldType);
		}
		Ts.GetField (field).SetValue (this, val);
	}
}
