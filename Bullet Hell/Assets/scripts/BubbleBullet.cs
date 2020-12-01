using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : Bullet
{
    public int clusterdensity = 25;
    public GameObject clusterbullet;
    public float clusterbulletspeed;
    bool popped = false;
    AudioManager am;

    protected override void Start()
    {
        base.Start();
        am = FindObjectOfType<AudioManager>();
    }

    protected override void Update()
    {
        if (popped)
        {
            DestroyEffect();
            clusterdensity = 360 / clusterdensity;//36
            for (int i = 0; i < 360 / clusterdensity; i++)//10 iterations
            {
                GameObject cb = Instantiate(clusterbullet, transform.position, Quaternion.identity);
                Vector3 unrotatedvector = new Vector3(0, clusterbulletspeed, 0);
                cb.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(0, 0, i * clusterdensity) * unrotatedvector).normalized *clusterbulletspeed;//smth
            }

            for (int i = 0; i < 360 / clusterdensity; i++)//10 iterations
            {
                GameObject cb = Instantiate(clusterbullet, transform.position, Quaternion.identity);
                Vector3 unrotatedvector = new Vector3(0, clusterbulletspeed, 0);
                cb.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(0, 0, (i * clusterdensity)+ (clusterdensity/2)) * unrotatedvector).normalized * clusterbulletspeed;//smth
                cb.transform.position += cb.GetComponent<Rigidbody>().velocity.normalized * gameObject.GetComponent<SpriteRenderer>().bounds.extents.x; ;
            }
            am.Play("DestroyObject", 2.5f);

            Destroy(gameObject);
        }
        else
        {
            base.Update();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyBullet"))
        {
            if (gameObject.CompareTag("EnemyBullet"))
            {
                popped = true;
            }
        }
    }
}
