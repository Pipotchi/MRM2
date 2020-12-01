using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Enemy Inventory")]
public class InventorySlot : ScriptableObject
{
    public List<Enemy> Container = new List<Enemy>();

    public GameObject ReturnEnemy(int slot)
    {
        GameObject enemy;
        enemy = Container[slot].gameObject;

        return enemy;
    }

    public List<GameObject> PickEnemies(int maxdifficulty)
    {
        List<GameObject> enemies = new List<GameObject>();
        List<GameObject> enemiesofdifficulty;
        float enemylimit = maxdifficulty;
        int enemycount = 0;
        int random = 0;

        Debug.Log("NEW ROOM MAX DIFFICULTY:" + maxdifficulty);

        while (enemycount < maxdifficulty)
        {
            
            enemiesofdifficulty = new List<GameObject>();
            random = UnityEngine.Random.Range(1, maxdifficulty-enemycount +1);//pick a random difficulty, collect all enemies at that difficulty
            for(int i = 0; i < Container.Count; i++)
            {
                if(Container[i].difficulty == random)
                {
                    enemiesofdifficulty.Add(Container[i].gameObject);
                }
            }

            if (enemiesofdifficulty.Count > 0)//pick a random enemy of that difficulty
            {
                enemies.Add(enemiesofdifficulty[UnityEngine.Random.Range(0, enemiesofdifficulty.Count)]);
                enemycount += random;
                Debug.Log("added:" + random);
            }
            else//if there arent any enemies at this difficulty then try again
            {
                Debug.Log("dropped:" + random);
            }
        }

        return enemies;
    }

    public void PlaceEnemy(GameObject room, GameObject enemy)
    {
        SpriteRenderer sprite1;
        sprite1 = room.transform.GetComponent<SpriteRenderer>();
        float randomx = UnityEngine.Random.Range(-sprite1.bounds.extents.x, sprite1.bounds.extents.x);
        float randomy = UnityEngine.Random.Range(-sprite1.bounds.extents.y, sprite1.bounds.extents.y);
        bool rayhit = false;
        Vector3 placementvector;
        float placementrand;


        int targetLayer_ = (1 << 0 | 1 << 8);

        RaycastHit[] hit = Physics.RaycastAll(room.transform.position, new Vector3(randomx, randomy, 0), 8.5f, targetLayer_, QueryTriggerInteraction.Ignore);
        float hitlength = hit.Length;
        if (hitlength > 0)
        {
            for (int i = 0; i < hitlength; i++)
            {

                if (hit[i].collider.gameObject.CompareTag("Room") && rayhit == false)
                {
                    rayhit = true;
                    placementvector = hit[i].point - room.transform.position;
                    placementrand = UnityEngine.Random.Range(0.01f, 0.7f);
                    if ((hit[i].point.y - room.transform.position.y) > 4.4f || (hit[i].point.y - room.transform.position.y) < -4.4f)
                    {
                        placementrand = UnityEngine.Random.Range(0.01f, 0.4f);
                    }
                    placementvector = placementvector * placementrand;

                    GameObject e1 = Instantiate(enemy, room.transform.position + placementvector, Quaternion.identity);
                    e1.transform.parent = room.transform;
                    e1.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.3f;
                }
            }
        }
        if(!rayhit)
        {
            GameObject e1 = Instantiate(enemy, room.transform.position, Quaternion.identity);
            e1.transform.parent = room.transform;
            e1.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.1f;
        }
    }
}
