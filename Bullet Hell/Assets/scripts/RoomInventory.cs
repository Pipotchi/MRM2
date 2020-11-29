using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Room Inventory")]
public class RoomInventory : ScriptableObject
{
    public List<Room> Container = new List<Room>();

    public GameObject ReturnRoom(int slot)
    {
        GameObject returnroom;
        returnroom = Container[slot].gameObject;

        return returnroom;
    }

    public GameObject PickRoom()
    {
        int random = 0;
        random = UnityEngine.Random.Range(0, Container.Count);
        Debug.Log(random);
        return Container[random].gameObject;
    }
}
