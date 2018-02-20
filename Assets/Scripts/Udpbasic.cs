using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Udpbasic : MonoBehaviour
{

  UdpClient udpc;
  IPEndPoint ep;
  // Use this for initialization
  void Start()
  {
    //		udpc = new UdpClient(args[0],2055);
    //		ep = null;
  }

  // Update is called once per frame
  void SendName()
  {
    //		string name = "Hello Moto";
    //		if(name == "") break;
    //		byte[] sdata = Encoding.ASCII.GetBytes(name);
    //		udpc.Send (sdata, sdata.Length);
  }
}
