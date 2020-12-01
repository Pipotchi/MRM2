using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterblastEnemy : Enemy
{
    int explodedensity = 10;
    public float explodebulletspeed;

    protected override void Update()
    {
        if (currentHP <= 0)
        {
            explodedensity = 360 / explodedensity;//36
            for (int i = 0; i < 360 / explodedensity; i++)//10 iterations
            {
                GameObject cb = Instantiate(bullettype, transform.position, Quaternion.identity);
                Vector3 unrotatedvector = new Vector3(0, explodebulletspeed, 0);
                cb.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, 0, i * explodedensity) * unrotatedvector;//smth
            }
            float pitch = UnityEngine.Random.Range(0.7f, 1f);
            audiomanager.Play("DestroyObject", pitch);
            explosionring = Instantiate(explosionring, transform.position, Quaternion.identity);
            Camera.main.GetComponent<ScreenShake>().shaking = true;
            Destroy(gameObject);
        }
        InjuredReaction();
        Active();
        Move();
        Animate();
        Attack();
    }

    protected override void Attack()
    {
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                Vector3 pointatplayer = player.transform.position - transform.position;
                CreateBullet(pointatplayer, firespeed);
                Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, 12));
                CreateBullet(overshoot1 - transform.position, firespeed);
                Vector3 overshoot2 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, -12));
                CreateBullet(overshoot2 - transform.position, firespeed);

                float pitch = UnityEngine.Random.Range(1f, 1.3f);
                audiomanager.Play("ShotAster", pitch);
                firetimer = 0;
            }
        }
    }
}
