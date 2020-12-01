using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    public bool shaking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            ScreenShaking();
            shaking = false;
        }
    }

    public void ScreenShaking()
    {
        Vector3 currentpos = Camera.main.transform.position;
        //Camera.main.transform.position = new Vector3(10,5,-10);
        StartCoroutine(MoveCam(0, currentpos + new Vector3(0, 0.04f, 0)));
        StartCoroutine(MoveCam(0.1f, currentpos - new Vector3(0, 0.02f, 0)));
        StartCoroutine(MoveCam(0.2f, currentpos));
    }

    public IEnumerator MoveCam(float time, Vector3 position)
    {
        yield return new WaitForSeconds(time);
        Camera.main.transform.position = position;
    }
}
