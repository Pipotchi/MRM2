using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FixedScale : MonoBehaviour
{


    //this is dangerous bullshit that will annoy me later on
    public float FixeScale = 1;
    [HideInInspector]
    public GameObject parent = null;
    [HideInInspector]
    public GameObject grandparent = null;
    Bullet bullet;

    private void Start()
    {
        if (transform.parent)
        {
            parent = transform.parent.gameObject;
            if (transform.parent.transform.parent)
            {
                grandparent = transform.parent.transform.parent.gameObject;
            }
        }

        if (transform.GetComponent<Bullet>())
        {
            bullet = transform.GetComponent<Bullet>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (grandparent)
        {

            transform.localScale = new Vector3(FixeScale / grandparent.transform.localScale.x, FixeScale / grandparent.transform.localScale.y, FixeScale / grandparent.transform.localScale.z);
        }

    }
}