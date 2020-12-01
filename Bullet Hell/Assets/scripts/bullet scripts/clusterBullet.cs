using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clusterBullet : Bullet
{
    float maxdistance = 2.5f;
    int clusterdensity = 10;
    Vector3 startposition;
    public GameObject clusterbullet;
    public float clusterbulletspeed;
    AudioManager am;
    protected override void Start()
    {
        base.Start();
        am = FindObjectOfType<AudioManager>();
        startposition = transform.position;
    }

    protected override void Update()
    {
        if ((transform.position - startposition).magnitude > maxdistance)
        {
            DestroyEffect();
            clusterdensity = 360 / clusterdensity;//36
            for (int i = 0; i < 360 / clusterdensity; i++)//10 iterations
            {
                GameObject cb = Instantiate(clusterbullet, transform.position, Quaternion.identity);
                Vector3 unrotatedvector = new Vector3(0, clusterbulletspeed, 0);
                cb.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, 0, i * clusterdensity) * unrotatedvector;//smth
            }
            am.Play("ShotAster", UnityEngine.Random.Range(2.5f,2.7f));
            Destroy(gameObject);
        }
        else
        {
            base.Update();
        }
    }
}
