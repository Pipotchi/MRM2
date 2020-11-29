using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AsteringEnemy : Enemy
{
    Vector3 targetpoint = Vector3.zero;
    public GameObject lasertarget;
    public GameObject laser;
    GameObject l1;
    GameObject lt1;
    float lasertargetcount;
    public float lasertargettime = 1.5f;

    float lasercount;
    public float lasertime = 1;
    bool warning = false;
    bool firing = false;
    float angle;
    float trailcount = 0;
    float trailtime = 0.05f;

    protected override void Start()
    {
        base.Start();
        lt1 = Instantiate(lasertarget, transform.position, Quaternion.identity);
        l1 = Instantiate(laser, transform.position, Quaternion.identity);
        //lt1.transform.parent = transform;
        //l1.transform.parent = transform;
        lt1.SetActive(false);
        l1.SetActive(false);
    }

    protected override void Update()
    {
        if(currentHP <= 0)
        {
            Destroy(l1);
            Destroy(lt1);
        }
        base.Update();

        lt1.transform.position = transform.position;
        l1.transform.position = transform.position;

        var dir = targetpoint - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90;
        lt1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        l1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Attack()
    {

        if (active)
        {
            if(l1.activeSelf)
            {
                trailcount += Time.deltaTime;
                if (trailcount > trailtime)
                {
                    CreateBullet(targetpoint - transform.position, firespeed * 10);
                    trailcount = 0;
                }
            }
            
            firetimer += Time.deltaTime;
            lasercount -= Time.deltaTime;
            lasertargetcount -= Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                targetpoint = player.transform.position;
                //var dir = targetpoint - transform.position;
                //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //angle -= 90;

                lasertargetcount = lasertargettime;
                warning = true;

                Vector3 overshoot1 = RotatePointAroundPivot(targetpoint, transform.position, new Vector3(0, 0, 5));
                Vector3 overshoot2 = RotatePointAroundPivot(targetpoint, transform.position, new Vector3(0, 0, -5));
                Vector3 overshoot3 = RotatePointAroundPivot(targetpoint, transform.position, new Vector3(0, 0, 10));
                Vector3 overshoot4 = RotatePointAroundPivot(targetpoint, transform.position, new Vector3(0, 0, -10));
                StartCoroutine(FireBulletDelay(0.2f, overshoot1 - transform.position));
                StartCoroutine(FireBulletDelay(0.4f, overshoot2 - transform.position));
                StartCoroutine(FireBulletDelay(0.6f, overshoot3 - transform.position));
                StartCoroutine(FireBulletDelay(0.8f, overshoot4 - transform.position));
                StartCoroutine(FireBulletDelay(1.0f, overshoot3 - transform.position));
                StartCoroutine(FireBulletDelay(1.2f, overshoot2 - transform.position));
                StartCoroutine(FireBulletDelay(1.4f, overshoot1 - transform.position));
                StartCoroutine(FireBulletDelay(1.6f, overshoot2 - transform.position));
                StartCoroutine(FireBulletDelay(1.8f, overshoot3 - transform.position));
                StartCoroutine(FireBulletDelay(2.0f, overshoot4 - transform.position));
                firetimer = 0;
            }

            if(lasertargetcount > 0 && warning)
            {
                lt1.SetActive(true);
                //lt1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                lt1.SetActive(false);
            }

            if(lasertargetcount < 0 && warning)
            {
                l1.SetActive(true);
                //l1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                lasercount = lasertime;
                warning = false;
                firing = true;
            }

            if (lasercount < 0 && firing)
            {
                l1.SetActive(false);
                firing = false;
            }
        }
        else
        {
            l1.SetActive(false);
            lt1.SetActive(false);
            lasercount = 0;
            lasertargetcount = 0;

        }


    }

    IEnumerator FireBulletDelay(float time, Vector3 angle)
    {
        yield return new WaitForSeconds(time);
        CreateBullet(angle, firespeed);
    }
}
