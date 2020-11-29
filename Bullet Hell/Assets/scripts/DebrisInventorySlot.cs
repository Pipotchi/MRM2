using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Debris Inventory")]
public class DebrisInventorySlot : ScriptableObject
{
    public List<GameObject> Container = new List<GameObject>();

    public GameObject PickDebris()
    {
        int random = 0;
        random = UnityEngine.Random.Range(0, Container.Count);
        return Container[random].gameObject;
    }
}
