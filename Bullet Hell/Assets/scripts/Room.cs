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
    GameObject player;
    bool visible = false;

    int childcount;
    MeteorFiringPattern meteorfire;
    MeteorMovementPattern meteormove;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!visible)
        {
            gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 3f * Time.deltaTime);
            if (gameObject.GetComponent<SpriteRenderer>().color.a >= 1)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                visible = true;
            }
        }

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
            player.GetComponent<PlayerControls>().insideroom = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinside = false;
            playerleft = true;

            player.GetComponent<PlayerControls>().insideroom = false;
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
