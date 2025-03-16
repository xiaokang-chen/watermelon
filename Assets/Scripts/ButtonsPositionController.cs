using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPositionController : MonoBehaviour
{
    public bool usingButton = true;
    public Transform Button1;
    public Transform Button2;
    public Transform Button3;
    public GameObject Bar1;
    public GameObject Bar2;
    public GameObject Bar3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(usingButton){
            if(!Button1.gameObject.activeSelf && Button2.gameObject.activeSelf && Button1.gameObject.transform.position.y > Button2.gameObject.transform.position.y){
                Vector3 pos = Button1.transform.position;
                Button1.transform.position = Button2.transform.position;
                Button2.transform.position = pos;
            }

            if(!Button1.gameObject.activeSelf && Button3.gameObject.activeSelf && Button1.gameObject.transform.position.y > Button3.gameObject.transform.position.y){
                Vector3 pos = Button1.transform.position;
                Button1.transform.position = Button3.transform.position;
                Button3.transform.position = pos;
            }

            if(!Button2.gameObject.activeSelf && Button3.gameObject.activeSelf && Button2.gameObject.transform.position.y > Button3.gameObject.transform.position.y){
                Vector3 pos = Button2.transform.position;
                Button2.transform.position = Button3.transform.position;
                Button3.transform.position = pos;
            }

            if(!Button2.gameObject.activeSelf && Button1.gameObject.activeSelf && Button2.gameObject.transform.position.y > Button1.gameObject.transform.position.y){
                Vector3 pos = Button1.transform.position;
                Button1.transform.position = Button2.transform.position;
                Button2.transform.position = pos;
            }
        }


        int count = 0;
        if(Button1.gameObject.activeSelf)
            count ++;
        if(Button2.gameObject.activeSelf)
            count ++;
        if(Button3.gameObject.activeSelf)
            count ++;  

        if(count == 0){
            Bar1.SetActive(false);
            Bar2.SetActive(false);
            Bar3.SetActive(false);
        }
        else if(count == 1){
            Bar1.SetActive(true);
            Bar2.SetActive(false);
            Bar3.SetActive(false);
        }
        else if(count == 2){
            Bar1.SetActive(false);
            Bar2.SetActive(true);
            Bar3.SetActive(false);
        }
        else if(count == 3){
            Bar1.SetActive(false);
            Bar2.SetActive(false);
            Bar3.SetActive(true);
        }
    }
}
