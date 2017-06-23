using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ArrayList l = new ArrayList();
        l.Add(10);
        l.Add("1111111111");
        var p = Proto.pack("I10s", l);
        foreach(var a in p){
            Debug.Log(a);
        }
        int start = 0;
        //Debug.Log("Bytes Count is " + p.Length);
        l = Proto.unpack("I10s", p, out start);
        foreach (var a in l)
        {
           // Debug.Log(a);
        }
        Debug.Log(start);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
