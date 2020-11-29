using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MeteorMovementPattern : MonoBehaviour
{

    public bool active = false;
    Vector3 movevector;
    public float movespeed;
    Vector3 strafevector;
    public float randomtimemin = 3;
    public float randomtimemax = 9;
    float randomtimecount = 0;
    float randommoveduration = 5;
    float randomtime;
    bool randomleft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            randomtime = UnityEngine.Random.Range(randomtimemin, randomtimemax);
            randomtimecount += Time.deltaTime;
            randommoveduration += Time.deltaTime;
            if (randommoveduration > 1)
            {
                if (randomtimecount <= randomtime)
                {
                    movevector = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * movespeed;
                }
                else
                {

                    movevector = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * movespeed;
                    movevector = new Vector3(-movevector.y, movevector.x, 0) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
                    randomtimecount = 0;
                    randommoveduration = 0;
                }
            }

            gameObject.GetComponent<Rigidbody>().velocity = movevector;
        }
    }
}
