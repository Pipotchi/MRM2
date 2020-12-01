using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodering : MonoBehaviour
{
    Vector3 originalscale;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        originalscale = transform.localScale;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(6, 6, 6) * Time.deltaTime;
        sr.color -= new Color(0, 0, 0, 1) * Time.deltaTime;
        if(transform.localScale.magnitude > originalscale.magnitude * 3)
        {
            Destroy(gameObject);
        }
    }
}
