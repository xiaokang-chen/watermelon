using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    public int InputCountMax;

    private int InputCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            InputCount++;
        }

        if(InputCount > InputCountMax)
        {
            gameObject.SetActive(false);
        }
    }
}
