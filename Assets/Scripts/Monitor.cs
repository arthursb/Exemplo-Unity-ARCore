using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour {
    public TextMesh displayText;

    public void Start() {
        displayText.text = "";
    }

    public void AppendText(string newText) {
        displayText.text += newText;
    }
}
