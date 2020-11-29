using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarbusterEnemy : Enemy
{
    protected override void Attack()
    {
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
}
