using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour{
    public bool isEnter;
    public string myString;
    public Monitor monitor;

    public void TouchKey() {
        if (isEnter) {
            monitor.AppendText("\n");
        }
        else {
            monitor.AppendText(myString);
        }
    }
}
