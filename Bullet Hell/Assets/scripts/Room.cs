using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomdifficulty;
    public int roomnumber;
    public bool playerinside = false;
    public bool playerleft = false;
    public int countingenemies;
    public int enemiesleft;
    public bool active = false;

    int childcount;
    MeteorFiringPattern meteorfire;
    MeteorMovementPattern meteormove;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountEnemies();
        enemiesleft = countingenemies;

        if (playerinside)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).CompareTag("Enemy"))
                {
                    /*
                    meteorfire = gameObject.transform.GetChild(i).GetComponent<MeteorFiringPattern>();
                    meteorfire.active = true;
                    meteormove = gameObject.transform.GetChild(i).GetComponent<MeteorMovementPattern>();
                    meteormove.active = true;
                    */
                    active = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).CompareTag("Enemy"))
                {
                    /*meteorfire = gameObject.transform.GetChild(i).GetComponent<MeteorFiringPattern>();
                    meteorfire.active = false;
                    meteormove = gameObject.transform.GetChild(i).GetComponent<MeteorMovementPattern>();
                    meteormove.active = false;*/

                    active = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                playerinside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinside = false;
            playerleft = true;
        }
    }

    void CountEnemies()
    {
        countingenemies = 0;
        childcount = transform.childCount;
        for (int i = 0; i < childcount; i++)
        {
            if (this.gameObject.transform.GetChild(i).CompareTag("Enemy"))
            {
                countingenemies++;
            }
        }
    }
}
