using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugTool : MonoBehaviour
{

    public HostMethods hm;
    public void SendMessage(InputField inp)
    {
        hm.SendMessage(inp.text);
    }
}
