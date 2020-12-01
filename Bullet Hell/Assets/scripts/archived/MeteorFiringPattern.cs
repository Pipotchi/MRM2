using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFiringPattern : MonoBehaviour
{
    public GameObject bullet;
    GameObject player;
    float firetimer = 0;
    public float fireinterval = 2.5f;
    public float firespeed;
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //IF PLAYER IS IN THE SAME ROOM:
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                Vector3 pointatplayer = player.transform.position - transform.position;
                CreateBullet(pointatplayer, firespeed);
                Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, 25));
                CreateBullet(overshoot1 - transform.position, firespeed);
                Vector3 overshoot2 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, -25));
                CreateBullet(overshoot2 - transform.position, firespeed);

                firetimer = 0;
            }
        }

    }

    void CreateBullet(Vector3 direction, float speed)
    {
        GameObject newbullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody rb = newbullet.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * speed;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
