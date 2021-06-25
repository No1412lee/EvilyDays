using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningText : MonoBehaviour {

    public float fadeSpeed;
    private Text text;
    private float a=1.0f;
	void Start () {
        text = GetComponent<Text>();
        
	}
	
	// Update is called once per frame
	void Update () {
        a = (text.color.a * 255 - Time.deltaTime * fadeSpeed) / 255.0f;
        Color temp = new Color(text.color.r, text.color.g, text.color.b, a);
        text.color = temp;
        if (a < 50/ 255)
        {
            gameObject.SetActive(false);
            text.color = Color.white;
            a = 1.0f;
        }
	}
}
