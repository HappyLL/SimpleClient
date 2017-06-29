using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public class Header{

	public Dictionary<string, object> m_dParams;
	public Dictionary<string, char> m_dParamTypes;
	public List<string> m_paramNames;
	public string m_fmt;
	public int m_fmtnum;

	public Header(ushort hid){
		m_dParams = new Dictionary<string, object> ();
		m_dParamTypes = new Dictionary<string, char> ();
		m_paramNames = new List<string> ();
		m_fmt = "";
		m_fmtnum = 0;
		this._AppendNewParam ("m_hid", 'H', hid);
	}

	public byte[] Header_Encode(){
		string fmt = this.GetHeaderFmt ();
		ArrayList vals = this._GetValues ();
		return Proto.pack (fmt, vals);
	}

	public bool Header_Decode(byte[] bytes){
		string fmt = this._DecodeFmt (bytes);
		int ind = 0;
		ArrayList arr = Proto.unpack (fmt, bytes, out ind);
		int index = 0;
		foreach (var item in arr) {
			var pName = m_paramNames [index];
			m_dParams [pName] = item;
			index++;
		}
		return true;
	}
		
	public void SetVal(string field, char proto_type, object val){
		if (this._CheckParamExist (field)) {
			this._SetOldParam (field, proto_type, val);
			return;
		}
		this._AppendNewParam (field, proto_type, val);
	}

	public void SetVal(string field, object val){
		if (this._CheckParamExist (field) && this._CheckParamTypeExist(field)) {
			m_dParams [field] = val;
			return;
		}
		Debug.LogError ("[Header][SetVal] set val error");
	}
		
	public object GetVal(string field){
		if (this._CheckParamExist (field)) {
			return m_dParams [field];
		}
		return null;
	}

	public string GetHeaderFmt(){
		return this._GetHFMT ();
	}
		
	private bool _CheckParamExist(string paramName){
		return m_dParams.ContainsKey (paramName);
	}

	private bool _CheckParamTypeExist(string paramName){
		return m_dParamTypes.ContainsKey (paramName);
	}

	private void _SetOldParam(string paramName, char paramType, object val){
		m_dParams [paramName] = val;
		m_dParamTypes [paramName] = paramType;
	}

	private void _AppendNewParam(string paramName, char paramType ,object val){
		m_dParams.Add (paramName, val);
		m_dParamTypes.Add (paramName, paramType);
		m_paramNames.Add (paramName);
		if (paramType == 's') {
			m_fmt = m_fmt + "{" + m_fmtnum + "}";
			m_fmtnum++;
		}
		m_fmt += paramType;
	}

	private string _GetHFMT(){
		List<object> str_lens = new List<object> ();
		foreach (var name in m_paramNames) {
			char pType = m_dParamTypes [name];
			if (pType == 's') {
				string val = (string)m_dParams [name];
				str_lens.Add (val.Length);
			}
		}
		return string.Format(m_fmt, str_lens.ToArray());
	}

	private ArrayList _GetValues(){
		ArrayList array = new ArrayList ();
		foreach (var pName in m_paramNames) {
			array.Add (m_dParams[pName]);
		}
		return array;
	}

	private string _DecodeFmt(byte[] bytes){
		string tmp_fmt = m_fmt;
		int fmt_start = 0;
		int fmt_len = 0;
		int byte_ind = 0;
		int tmp;
		List<object> str_lens = new List<object> ();
		Debug.Log ("[Header][_DecodeFmt] all bytelen is "+bytes.Length);
		while (true){
			int tag_ind = tmp_fmt.IndexOf ('{');
			if (tag_ind == -1)
				break;
			fmt_len = tag_ind - fmt_start;
			string sub_fmt = tmp_fmt.Substring (fmt_start, fmt_len);
			Debug.Log ("[Header][_DecodeFmt] "+sub_fmt);
			int tmp_sz = (int)Proto.calsize (sub_fmt);
			Debug.Log ("[Header][_DecodeFmt] sz is "+tmp_sz);
			byte_ind += tmp_sz;
			int len_sz = (int)Proto.calsize ("i");
			byte[] tmp_byte = new byte[len_sz];
			Array.Copy (bytes, byte_ind, tmp_byte, 0 , len_sz);
			ArrayList arr = Proto.unpack("i", tmp_byte, out tmp);
			if (arr == null || arr.Count != 1) {
				Debug.Log ("[Header][_DecodeFmt] errir arr is nil or len is not one");
				return null;
			}
			str_lens.Add (arr[0]);
			byte_ind = byte_ind + (int)arr[0] + len_sz;
			Debug.Log ("[Header][_DecodeFmt] "+byte_ind);
			tag_ind = tmp_fmt.IndexOf ('}');
			//Debug.Log ("[Header][_DecodeFmt] index is " + tag_ind + " tmp_len is "+tmp_fmt.Length);
			if (tag_ind + 1 >= tmp_fmt.Length)
				break;
			int sub_start = tag_ind + 2;
			tmp_fmt = tmp_fmt.Substring (sub_start, tmp_fmt.Length - sub_start);
		}
		Debug.Log ("[Header][_DecodeFmt] fmt is " + string.Format (m_fmt, str_lens.ToArray()));
		return string.Format (m_fmt, str_lens.ToArray());
	}

	private void _AddToArray(ArrayList target, ArrayList source){
		foreach (var item in source) {
			target.Add (item);
		}
		source.Clear ();
	}
}
