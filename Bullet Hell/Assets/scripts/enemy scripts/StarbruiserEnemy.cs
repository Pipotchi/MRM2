using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarbruiserEnemy : Enemy
{
    bool jump = false;
    Vector3 jumpstartpos;
    Vector3 jumpendpos;
    Vector3 middlepos;
    float maxdistance;
    float distancetravelled = 0;
    float jumpmaxheight = 2f;
    float jumpheight = 0;
    float jumpdistance = 2;
    public float jumpmax = 3;

    float jh = 0.5f;

    float timer = 0;
    public float interval = 4f;

    protected override void Move()
    {
        if (active)
        {
            if (jump)
            {
                middlepos = transform.position - new Vector3(0, jumpheight, 0) +((jumpendpos - jumpstartpos).normalized * movespeed);
                
                maxdistance = (jumpendpos-jumpstartpos).magnitude;
                distancetravelled = (middlepos-jumpstartpos).magnitude;
                if(distancetravelled < maxdistance/2)
                {
                    jumpheight = (distancetravelled / maxdistance) * jumpmaxheight;
                }
                else
                {
                    jumpheight = (1- (distancetravelled / maxdistance)) * jumpmaxheight;
                }
                transform.position = middlepos;
                transform.position += new Vector3(0, jumpheight, 0);
                //gameObject.GetComponent<Rigidbody>().velocity = (middlepos + new Vector3(0, jumpheight, 0)) - transform.position;
                if((transform.position -jumpstartpos).magnitude > maxdistance)
                {
                    jump = false;
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else
            {

                timer += Time.deltaTime;

                if (timer >= interval)
                {
                    jump = true;
                    jumpstartpos = transform.position;
                    if ((player.transform.position - transform.position).magnitude < jumpmax)
                    {
                        jumpendpos = player.transform.position;
                    }
                    else
                    {
                        jumpendpos = transform.position + (player.transform.position - transform.position).normalized * jumpmax;
                    }
                    timer = 0;
                }
            }
        }
    }

    protected override void Attack()
    {
    }
    //attack by jumping at the player.
    //takes the players position and start position, then
    //during the "jump" state, check what percentage to the finish you are.
    //use this as a multiplier for additional Y height for the enemy
    //during the jump, use a drop shadow to show actual location.
    //on land, release some bullets


}
