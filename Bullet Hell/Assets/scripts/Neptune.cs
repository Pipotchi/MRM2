using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neptune : Enemy
{
    public int bulletcount = 70;
    public float bulletdensityangle = 5;
    public float timedelay = 0.1f;
    Vector3 anglepick;
    float coneangle = 120;
    int isEven;
    public GameObject lefteye;
    public GameObject righteye;
    public GameObject mouth;
    public GameObject bubblebullet;
    public GameObject spikebullet;
    public float bubblespeed;
    public float spikespeed = 2;
    Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = this.GetComponent<Animator>();
    }

    protected override void Update()
    {
        if(currentHP == 0)
        {
            animator.SetBool("dead", true);
        }
        base.Update();
    }

    protected override void Attack()
    {
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                int randomattack = UnityEngine.Random.Range(0, 3);
                if (randomattack == 0)//waves
                {
                    animator.SetBool("idle", false);
                    animator.SetInteger("atk state", 1);
                    fireinterval = 8;
                    StartCoroutine(OscillatingWave(0));
                    
                }
                if (randomattack == 1)//waves + bubbles
                {
                    animator.SetBool("idle", false);
                    animator.SetInteger("atk state", 1);
                    fireinterval = 10;
                    StartCoroutine(OscillatingWave(3));
                    for (int i = 0; i < fireinterval/1.8f; i++)
                    {
                        StartCoroutine(Bubbles(i*1.8f));
                    }
                    StartCoroutine(ReturnToIdle(fireinterval -0.2f));
                }
                if (randomattack == 2)//spikes + bubbles
                {
                    animator.SetBool("idle", false);
                    animator.SetInteger("atk state", 2);
                    fireinterval = 7;
                    for (int i = 0; i < fireinterval / 0.2f; i++)
                    {
                        StartCoroutine(Spikes(i * 0.2f));
                    }
                    StartCoroutine(ReturnToIdle(fireinterval - 0.2f));
                    for (int i = 0; i < fireinterval / 1.8f; i++)
                    {
                        StartCoroutine(Bubbles(i * 1.8f));
                    }
                }

                firetimer = 0;
            }
        }
    }

    protected override void Injured(Vector3 damagedirection)
    {
        float pitch = UnityEngine.Random.Range(0.9f, 1.2f);
        audiomanager.Play("HitEnemy", pitch);
        injuredtimer = injuredmax;
    }

    IEnumerator ReturnToIdle(float time)
    {
        yield return new WaitForSeconds(time);
            animator.SetBool("idle", true);
    }

    IEnumerator OscillatingWave(float time)
    {
        yield return null;
        StartCoroutine(OscillatingBeam(time, lefteye));
        StartCoroutine(OscillatingBeam(time, righteye));
    }

    IEnumerator Bubbles(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 pointatplayer = player.transform.position - transform.position;
        Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, UnityEngine.Random.Range(-20, 20)));
        int choice = Mathf.RoundToInt(UnityEngine.Random.Range(0.1f, 0.9f));

        if (choice == 0)
        {
            CreateDistancedBulletType(pointatplayer, bubblespeed, mouth.transform, bubblebullet);
        }
        else
        {
            CreateDistancedBulletType(overshoot1 - transform.position, bubblespeed, mouth.transform, bubblebullet);
        }
        audiomanager.Play("Bubble", UnityEngine.Random.Range(0.7f, 1));
    }

    IEnumerator Spikes(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 pointatplayer = player.transform.position - transform.position;
        Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, UnityEngine.Random.Range(-20, -5)));
        Vector3 overshoot2 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, UnityEngine.Random.Range(5, 20)));
        int choice = UnityEngine.Random.Range(0, 3);

        if (choice == 0)
        {
            CreateDistancedBulletType(pointatplayer, spikespeed, mouth.transform, spikebullet);
        }
        if(choice ==1)
        {
            CreateDistancedBulletType(overshoot1 - transform.position, spikespeed, mouth.transform, spikebullet);
        }
        if (choice == 2)
        {
            CreateDistancedBulletType(overshoot2 - transform.position, spikespeed, mouth.transform, spikebullet);
        }
        audiomanager.Play("Tooth", UnityEngine.Random.Range(0.7f, 1.2f));
    }


    IEnumerator OscillatingBeam(float time, GameObject origin)
    {
        yield return new WaitForSeconds(time);

        //picks either 1 or -1 to randomize the direction between cw and ccw
        int cw = (2 * UnityEngine.Random.Range(0, 2)) - 1;
        float randomoffset = UnityEngine.Random.Range(-3.1f,6.1f);
        Debug.Log(cw);
        for (int i = 0; i < bulletcount; i++)
        {
            isEven = Mathf.FloorToInt((i * bulletdensityangle) / coneangle);
            
            //divide i*bulletdensityangle by coneangle and round down. if the result is odd, the bullets need to add position from left to right.
            //if odd, the bullets need to move from right to left.
            //subtract the rounded down value from i*bulletdensityangle. this is how much to add from the left (rotatearound playertransform (-coneangle/2))
            //or subtract from the right (rotatearound player transform + coneangle/2)..
            if (isEven % 2 == 0)
            {
                float bulletstep = (i * bulletdensityangle) - (isEven * coneangle);
                anglepick = RotatePointAroundPivot(origin.transform.position - new Vector3(0,5,0), origin.transform.position, new Vector3(0, 0, (-coneangle*cw / 2)+ randomoffset));
                anglepick = RotatePointAroundPivot(anglepick, origin.transform.position, new Vector3(0, 0, bulletstep * cw));

            }
            else
            {
                float bulletstep = (i * bulletdensityangle) - (isEven * coneangle);
                anglepick = RotatePointAroundPivot(origin.transform.position - new Vector3(0, 5, 0), origin.transform.position, new Vector3(0, 0, (coneangle * cw / 2)+ randomoffset));
                anglepick = RotatePointAroundPivot(anglepick, origin.transform.position, new Vector3(0, 0, -bulletstep * cw));

            }
            StartCoroutine(FireBulletDelay(i * timedelay, anglepick - origin.transform.position, origin.transform));
            //the above is the same but it does all the shots from the time of calling
            //the below is messy but updates with player position but doesnt work yet
            //StartCoroutine(FireBulletDelay(i * timedelay, (RotatePointAroundPivot(RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, coneangle / 2)), transform.position, new Vector3(0, 0, -((i * bulletdensityangle) - (isEven * coneangle))))) - transform.position));
        }
        //StartCoroutine(ReturnToIdle((bulletcount+1) * timedelay));
    }

    IEnumerator FireBulletDelay(float time, Vector3 angle, Transform origin)
    {
        yield return new WaitForSeconds(time);
        CreateDistancedBullet(angle, firespeed, origin);
    }

    public void CreateDistancedBullet(Vector3 direction, float speed, Transform origin)
    {
        GameObject newbullet = Instantiate(bullettype, origin.position, Quaternion.identity);
        Rigidbody rb2 = newbullet.GetComponent<Rigidbody>();
        rb2.velocity = direction.normalized * speed;
    }

    public void CreateDistancedBulletType(Vector3 direction, float speed, Transform origin, GameObject bullettype)
    {
        GameObject newbullet = Instantiate(bullettype, origin.position, Quaternion.identity);
        Rigidbody rb2 = newbullet.GetComponent<Rigidbody>();
        rb2.velocity = direction.normalized * speed;
    }
}
