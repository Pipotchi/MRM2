using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarwizardEnemy : Enemy
{
    public int bulletcount = 60;
    public float bulletdensityangle = 8;
    public float timedelay = 0.1f;
    Vector3 anglepick;
    float coneangle = 60;
    int isEven;
    bool teleported = false;
    Vector3 middleofroom;

    protected override void Start()
    {
        base.Start();
        coneangle = UnityEngine.Random.Range(60, 250);
        middleofroom = transform.parent.transform.position;
    }

    protected override void Injured(Vector3 damagedirection)
    {
        base.Injured(damagedirection);
        if (!teleported && currentHP < maxHP / 2)
        {
            Vector3 randomize = RotatePointAroundPivot(middleofroom + new Vector3(0,3,0), middleofroom, new Vector3(0, 0, UnityEngine.Random.Range(0, 360)));
            transform.position = randomize;
            teleported = true;

        }
    }

    protected override void Move()
    {
        rb.velocity = Vector3.zero;
    }

    protected override void Attack()
    {
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                for(int i = 0; i < bulletcount; i++)
                {
                    isEven = Mathf.FloorToInt((i*bulletdensityangle)/coneangle);

                    //divide i*bulletdensityangle by coneangle and round down. if the result is odd, the bullets need to add position from left to right.
                    //if odd, the bullets need to move from right to left.
                    //subtract the rounded down value from i*bulletdensityangle. this is how much to add from the left (rotatearound playertransform (-coneangle/2))
                    //or subtract from the right (rotatearound player transform + coneangle/2)..
                    if (isEven % 2 == 0)
                    {
                        float bulletstep = (i * bulletdensityangle) - (isEven*coneangle);
                        anglepick = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, -coneangle/2));
                        anglepick = RotatePointAroundPivot(anglepick, transform.position, new Vector3(0, 0, bulletstep));

                    }
                    else
                    {
                        float bulletstep = (i * bulletdensityangle) - (isEven * coneangle);
                        anglepick = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, coneangle/2));
                        anglepick = RotatePointAroundPivot(anglepick, transform.position, new Vector3(0, 0, -bulletstep));

                    }
                    StartCoroutine(FireBulletDelay(i * timedelay, anglepick - transform.position));
                    //the above is the same but it does all the shots from the time of calling
                    //the below is messy but updates with player position
                    //StartCoroutine(FireBulletDelay(i * timedelay, (RotatePointAroundPivot(RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, coneangle / 2)), transform.position, new Vector3(0, 0, -((i * bulletdensityangle) - (isEven * coneangle))))) - transform.position));
                }

                firetimer = 0;
            }
        }
    }

    IEnumerator FireBulletDelay(float time, Vector3 angle)
    {
        yield return new WaitForSeconds(time);
        CreateBullet(angle, firespeed);
    }

    protected virtual void OnBecameInvisible()
    {
        transform.position = room.transform.position;
    }

}
