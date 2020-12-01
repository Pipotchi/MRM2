using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CatchBullets : MonoBehaviour
{

    Bullet bulletscript;
    int bulletcount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("EnemyBullet"))
        {
            other.GetComponent<Bullet>().player = gameObject;
                AddBullet(other.gameObject);
        }*/
        
        if (other.TryGetComponent<Bullet>(out Bullet bulletscript))
        {
            
            bulletscript = other.GetComponent<Bullet>();
            if (!bulletscript.friendly)
            {

                bulletscript.player = gameObject;
                bulletscript.nearplayer = true;

                if (bulletscript.player)
                {

                    if (Input.GetButton("Slowdown"))
                    {
                        int targetLayer_ = (1 << 0 | 1 << 8);

                        RaycastHit[] hit = Physics.SphereCastAll(bulletscript.transform.position, bulletscript.GetComponent<SphereCollider>().radius * bulletscript.transform.localScale.x, bulletscript.GetComponent<Rigidbody>().velocity, 10f, targetLayer_, QueryTriggerInteraction.Ignore);
                        float hitlength = hit.Length;
                        if (hitlength > 0)
                        {
                            for (int i = 0; i < hitlength; i++)
                            {

                                if (hit[i].collider.gameObject.CompareTag("Player"))
                                {
                                    bulletscript.nearplayer = false;
                                }

                            }
                        }
                        else
                        {
                            bulletscript.nearplayer = true;
                        }
                        if (bulletscript.nearplayer)
                        {
                            Vector3 direction = bulletscript.GetComponent<Rigidbody>().velocity;
                            Vector3 target = bulletscript.player.transform.position - bulletscript.transform.position;
                            bulletscript.angle = Vector3.Angle(direction, target);

                            if (bulletscript.angle < 110)
                            {
                                bulletscript.inorbit = true;
                            }
                        }
                    }
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bulletscript))
        {
            bulletscript = other.GetComponent<Bullet>();
            if (!bulletscript.friendly)
            {
                if (other.CompareTag("EnemyBullet"))
                {

                    
                    bulletscript.player = null;
                    bulletscript.nearplayer = false;
                    bulletscript.inorbit = false;
                }
            }
        }
    }


}
