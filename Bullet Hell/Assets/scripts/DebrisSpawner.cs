using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    public DebrisInventorySlot DebrisInventory;
    public float spawnintervalmin;
    public float spawnintervalmax;
    float spawninterval1;
    float spawntimer1 = 0;
    float spawninterval2;
    float spawntimer2 = 0;

    public float movespeedmin; 
    public float movespeedmax;
    float movespeed;
    public float rotationspeedmin;
    public float rotationspeedmax;
    float rotationspeed;
    float angle;
    Vector3 dir;
    bool over;
    Vector3 spawnloc;
    float spawnlength = 20;


    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        spawninterval1 = UnityEngine.Random.Range(spawnintervalmin, spawnintervalmax);
        spawninterval2 = UnityEngine.Random.Range(spawnintervalmin, spawnintervalmax);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawntimer1 == 0)
        {
            spawninterval1 = UnityEngine.Random.Range(spawnintervalmin, spawnintervalmax);
        }
        spawntimer1 += Time.deltaTime;
        if (spawntimer1 > spawninterval1)
        {
            spawntimer1 = 0;

            angle = UnityEngine.Random.Range(0, 360);
            spawnloc = player.transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(0, spawnlength, 0);
            //pick an angle to spawn.
            //pick a spawn direction thats somewhat close to the player

            GameObject debris = DebrisInventory.PickDebris();
            //pick a random piece of debris

            movespeed = UnityEngine.Random.Range(movespeedmin, movespeedmax);
            rotationspeed = UnityEngine.Random.Range(rotationspeedmin, rotationspeedmax);
            //pick a random movespeed within limits
            //pick a random rotation speed within limits


            //pick whether to be over or under the platform at random

            dir = RotatePointAroundPivot(player.transform.position, spawnloc, new Vector3(0, 0, UnityEngine.Random.Range(-25f, 25f)));
            dir = dir - spawnloc;

            GameObject d1 = Instantiate(debris, spawnloc, Quaternion.identity);
            Rigidbody rb = d1.GetComponent<Rigidbody>();
            rb.velocity = dir.normalized * movespeed;
            rb.angularVelocity = new Vector3(0, 0, rotationspeed);
            //decide its direction
            //instantiate the object

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                over = true;
            }
            else
            {
                over = false;
                d1.GetComponent<SpriteRenderer>().sortingLayerName = "BG";
            }
            if (over)
            {
                if(d1.GetComponent<SpriteRenderer>().sortingLayerName != "BG")
                {
                    d1.GetComponent<SpriteRenderer>().sortingLayerName = "FG";
                }
            }

            //move above/under plat

            //object auto deletes after it moves a set distance away

            //maybe SOME of these rocks can also be picked up and used as ur bullets
            //they will never injure u
        }

        if (spawntimer2 == 0)
        {
            spawninterval2 = UnityEngine.Random.Range(spawnintervalmin, spawnintervalmax);
        }
        spawntimer2 += Time.deltaTime;
        if (spawntimer2 > spawninterval2)
        {
            spawntimer2 = 0;

            angle = UnityEngine.Random.Range(0, 360);
            spawnloc = player.transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(0, spawnlength, 0);
            GameObject debris = DebrisInventory.PickDebris();
            movespeed = UnityEngine.Random.Range(movespeedmin, movespeedmax);
            rotationspeed = UnityEngine.Random.Range(rotationspeedmin, rotationspeedmax);
            dir = RotatePointAroundPivot(player.transform.position, spawnloc, new Vector3(0, 0, UnityEngine.Random.Range(-25f, 25f)));
            dir = dir - spawnloc;

            GameObject d1 = Instantiate(debris, spawnloc, Quaternion.identity);
            Rigidbody rb = d1.GetComponent<Rigidbody>();
            rb.velocity = dir.normalized * movespeed;
            rb.angularVelocity = new Vector3(0, 0, rotationspeed);
            d1.GetComponent<SpriteRenderer>().sortingLayerName = "BG";

        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
