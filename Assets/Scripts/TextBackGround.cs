using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBackGround : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(text.text == ""){
            image.enabled = false;
        }else{
            image.enabled = true;
        }
    }
}
