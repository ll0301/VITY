using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public string owner = "";
    public string username = "";
    public string myPoint = "";
    public string playerNumber = "";
    public string group = "";
    public bool firstData;
    public bool characterSet;

    //추가 
    public bool throwMode;
    public bool throwItem;
    public bool pickup;
    public bool drop;
    


    public bool move;
    public bool punch;
   // public bool receivePacket;
    public bool alive;
    public bool winner;

    public string pX = "";
    public string pY = "";
    public string pZ = "";

    public string rY = "";

    public string runX = "";
    public string runZ = "";
}
