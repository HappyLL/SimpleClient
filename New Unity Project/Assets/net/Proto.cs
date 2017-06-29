using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
//先不考虑大小端问题
public class Proto{

    public static byte[] pack(string fmt, ArrayList list){
        int fmt_len = fmt.Length;
        int arr_len = list.Count;
        byte[] ret = { };
        int cnt = 0;
        int char_len = 0;
        for(int i = 0; i < fmt_len; ++i){
            char ele = fmt[i];
            if (char.IsDigit(ele)){
                char_len = char_len * 10 + (ele - '0');
                continue;
            }
            else if (char.IsLetter(ele)){
                if (cnt >= arr_len)
                {
                    Debug.Log("[Proto] pack error arrlen less");
                    return null;
                }
                byte[] tmp = { };
                object val = list[cnt];
                switch (ele){
                    case 'h':
                        tmp = BitConverter.GetBytes(Convert.ToInt16(val));
                        break;
                    case 'H':
                        tmp = BitConverter.GetBytes(Convert.ToUInt16(val));
                        break;
                    case 'i':
                        tmp = BitConverter.GetBytes(Convert.ToInt32(val));
                        break;
                    case 'I':
                        tmp = BitConverter.GetBytes(Convert.ToUInt32(val));
                        break;
					case 's':
						tmp = BitConverter.GetBytes (Convert.ToInt32 (char_len));
						char_len = 0;
						ret = ret.Concat(tmp).ToArray();
                        string str = Convert.ToString(val);
                        tmp = System.Text.Encoding.Default.GetBytes(str);
                        break;
                    case 'c':
                        tmp = BitConverter.GetBytes(Convert.ToChar(val));
                        break;
                    case 'd':
                        tmp = BitConverter.GetBytes(Convert.ToDouble(val));
                        break;
                    case 'f':
                        tmp = BitConverter.GetBytes(Convert.ToDouble(val));
                        break;
                }
                ret = ret.Concat(tmp).ToArray();
                cnt++;
            }
            else{
                Debug.Log("[Proto] pack error fmt is " + fmt);
                return null;
            }
        }
        return ret;
    }

    public static ArrayList unpack(string fmt, byte[] bytes, out int ind){
        int fmt_len = fmt.Length;
        ArrayList ret = new ArrayList();
        int char_len = 0;
        int start = 0;
        for (int i = 0; i < fmt_len; ++i){
            char ele = fmt[i];
            if (char.IsDigit(ele))
            {
                char_len = char_len * 10 + (ele - '0');
                continue;
            }
            else if (char.IsLetter(ele))
            {
                object tmp = null;
                switch (ele)
                {
                    case 'h':
                        tmp = BitConverter.ToInt16(bytes, start);
                        start += sizeof(Int16);
                        break;
                    case 'H':
                        tmp = BitConverter.ToUInt16(bytes, start);
                        start += sizeof(UInt16);
                        break;
                    case 'i':
                        tmp = BitConverter.ToInt32(bytes, start);
                        start += sizeof(Int32);
                        break;
                    case 'I':
                        tmp = BitConverter.ToUInt32(bytes, start);
                        start += sizeof(UInt32);
                        break;
					case 's':
						BitConverter.ToInt32 (bytes, start);
						start += sizeof(Int32);
						tmp = System.Text.Encoding.Default.GetString (bytes, start, char_len);
		                    //Debug.Log("char tmp is "+tmp);
						start += char_len;
						char_len = 0;
                        break;
                    case 'c':
                        tmp = BitConverter.ToChar(bytes, start);
                        start += sizeof(char);
                        break;
                    case 'd':
                        tmp = BitConverter.ToDouble(bytes, start);
                        start += sizeof(double);
                        break;
                    case 'f':
                        tmp = BitConverter.ToSingle(bytes, start);
                        start += sizeof(float);
                        break;
                }
                if (tmp != null)
                {
                    ret.Add(tmp);
                }
            }
            else
            {
                Debug.Log("[Proto] unpack error fmt is " + fmt);
                ind = start;
                return null;
            }
        }
        ind = start;
        return ret;
    }
		
	public static uint calsize(string fmt){
		int fmt_len = fmt.Length;
		uint char_len = 0;
		uint start = 0;
		for (int i = 0; i < fmt_len; ++i){
			char ele = fmt[i];
			if (char.IsDigit(ele))
			{
				char_len = char_len * 10 + (uint)(ele - '0');
				continue;
			}
			else if (char.IsLetter(ele))
			{
				switch (ele)
				{
				case 'h':
					start += sizeof(Int16);
					break;
				case 'H':
					start += sizeof(UInt16);
					break;
				case 'i':
					start += sizeof(Int32);
					break;
				case 'I':
					start += sizeof(UInt32);
					break;
				case 's':
					//Debug.Log("char tmp is "+tmp);
					start += char_len;
					break;
				case 'c':
					start += sizeof(char);
					break;
				case 'd':
					start += sizeof(double);
					break;
				case 'f':
					start += sizeof(float);
					break;
				}
			}
			else
			{
				Debug.Log("[Proto] calsize error fmt is " + fmt);
				return start;
			}
		}
		return start;
	}
    
}
