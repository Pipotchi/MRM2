using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamStart : MonoBehaviour
{
    bool done = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - new Vector3(0, 2, -10)).magnitude > 1 && done == false)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 2, -10), 1.5f * Time.deltaTime);
        }
        if ((transform.position - new Vector3(0, 2, -10)).magnitude < 1)
        {
            done = true;
        }
    }
}
