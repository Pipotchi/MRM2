using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFiringPattern : MonoBehaviour
{
    public GameObject bullet;
    GameObject player;
    float firetimer = 0;
    public float fireinterval = 2.5f;
    public float firespeed;
    public bool aim = true;
    [HideInInspector]
    public bool active = true;
    public float burstduration1 = 2;
    public float breakduration1 = 2;
    float burstcount1 = 0;
    float breakcount1 = 0;
    


    // Start is called before the first frame update
    void Start()
    {
        burstcount1 = 0;
        breakcount1 = 0;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (burstcount1 < burstduration1)
        {
            burstcount1 += Time.deltaTime;
            //IF PLAYER IS IN THE SAME ROOM:
            if (active)
            {
                firetimer += Time.deltaTime;

                if (firetimer >= fireinterval)
                {
                    if (aim)
                    {
                        Vector3 pointatplayer = player.transform.position - transform.position;
                        CreateBullet(pointatplayer, firespeed, Quaternion.identity);
                        firetimer = 0;
                    }
                    else
                    {
                        Vector3 pointatplayer = player.transform.position - transform.position;
                        CreateBullet(pointatplayer, firespeed, transform.rotation);
                        firetimer = 0;
                    }
                }
            }
        }

        if(burstcount1 > burstduration1)
        {
            breakcount1 += Time.deltaTime;
        }
        if(burstcount1 > burstduration1 && breakcount1 > breakduration1)
        {
            burstcount1 = 0;
            breakcount1 = 0;
        }
    }

    void CreateBullet(Vector3 direction, float speed, Quaternion rot)
    {
        GameObject newbullet = Instantiate(bullet, transform.position, rot);
        Rigidbody rb = newbullet.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * speed;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
