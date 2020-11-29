using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterickleEnemy : Enemy
{
    protected override void Attack()
    {
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                StartCoroutine(FireBullets(0));
                StartCoroutine(FireBullets(0.5f));
                StartCoroutine(FireBullets(1));
                firetimer = 0;
            }
        }
    }

    IEnumerator FireBullets(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 pointatplayer = player.transform.position - transform.position;
        Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, UnityEngine.Random.Range(-60, 60)));
        int choice = Mathf.RoundToInt(UnityEngine.Random.Range(0.1f, 0.9f));

        if (choice == 0)
        {
            CreateBullet(pointatplayer, firespeed);
        }
        else
        {
            CreateBullet(overshoot1 - transform.position, firespeed);
        }
    }
}