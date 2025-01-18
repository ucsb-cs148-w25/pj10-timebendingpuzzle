using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAlpha : MonoBehaviour
{
    public TextMeshProUGUI helloWorldText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTextAlpha(){
        
        if (helloWorldText != null){
            Color currentColor = helloWorldText.color;
            helloWorldText.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        }
    }
}
