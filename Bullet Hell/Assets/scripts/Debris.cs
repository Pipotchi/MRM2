using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    float maxdistance = 40f;
    Vector3 startposition;
    void Start()
    {
        startposition = transform.position;
    }

    void Update()
    {
        if ((transform.position - startposition).magnitude > maxdistance)
        {
            Destroy(gameObject);
        }
    }
}
