using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Obstacle Inventory")]
public class ObstacleInventory : ScriptableObject
{
    public List<Obstacle> Container = new List<Obstacle>();//need a new object type obstacle

    public GameObject ReturnObstacle(int slot)
    {
        GameObject obstacle;
        obstacle = Container[slot].gameObject;

        return obstacle;
    }

    public void PlaceObstacle(GameObject room)
    {
        GameObject obstacle;
        int random = 0;
        random = UnityEngine.Random.Range(0, Container.Count);
        obstacle = Container[random].gameObject;
        SpriteRenderer sprite1;
        sprite1 = room.transform.GetComponent<SpriteRenderer>();
        float randomx = UnityEngine.Random.Range(-sprite1.bounds.extents.x, sprite1.bounds.extents.x);
        float randomy = UnityEngine.Random.Range(-sprite1.bounds.extents.y, sprite1.bounds.extents.y);
        bool rayhit = false;
        Vector3 placementvector;
        float placementrand;
        

        int targetLayer_ = (1 << 0 | 1 << 8);

        RaycastHit[] hit = Physics.RaycastAll(room.transform.position, new Vector3(randomx, randomy,0), 10f, targetLayer_, QueryTriggerInteraction.Ignore);
        float hitlength = hit.Length;
        if (hitlength > 0)
        {
            for (int i = 0; i < hitlength; i++)
            {

                if (hit[i].collider.gameObject.CompareTag("Room") && rayhit == false)
                {
                    rayhit = true;
                    Debug.Log("obstacle wall hit");

                    placementvector = hit[i].point - room.transform.position;
                    placementrand = UnityEngine.Random.Range(0.01f, 0.75f);
                    placementvector = placementvector * placementrand;

                    GameObject o1 = Instantiate(obstacle, room.transform.position + placementvector, Quaternion.identity);
                    o1.transform.parent = room.transform;
                }

            }
        }
        else
        {
            Debug.Log("obstacle missed");
        }
    }
}

