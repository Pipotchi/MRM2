using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public AudioManager am;
    public GameObject startroom;
    public RoomInventory BasicRoomInventory;
    public GameObject room1;
    public GameObject room2;

    public GameObject enemy;
    public InventorySlot EnemyInventory;
    public InventorySlot BossInventory;
    public ObstacleInventory BasicObstacleInventory;
    public TextMeshProUGUI level;
    public RectTransform bosshp;
    public TextMeshProUGUI highscore;
    
    

    Room room;
    Room secondroom;

    bool higherroom = false;
    bool dooractive = false;
    public int highestroomnumber = 0;
    int totalenemiesleft = 0;
    bool enemiesleft;
    float camspeed = 2;
    public bool bossroom = false;

    //an array of difficulties to have a custom difficulty curve. when adding new room difficulty, pulls from this[i]
    

    int childcount;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject firstroom = Instantiate(startroom, transform.position, Quaternion.identity);
        firstroom.transform.parent = gameObject.transform;
        room = firstroom.GetComponent<Room>();
        room.roomnumber = 1;
        room.roomdifficulty = 1;
        bosshp.gameObject.SetActive(false);
        am = FindObjectOfType<AudioManager>();
        am.Play("Theme");

        highscore.text = PlayerPrefs.GetInt("Highscore",1).ToString();
    }

    // Update is called once per frame
    void Update()
    {

        

        childcount = transform.childCount;
        totalenemiesleft = 0;
        highestroomnumber = 0;

        room = this.gameObject.transform.GetChild(0).GetComponent<Room>();
        for (int i = 0; i < childcount; i++)
        {
            if (this.gameObject.transform.GetChild(i).CompareTag("Room"))
            {
                highestroomnumber++;
                int roomchildcount = this.gameObject.transform.GetChild(i).childCount;
                for (int j = 0; j < roomchildcount; j++)
                {
                    if (this.gameObject.transform.GetChild(i).transform.GetChild(j).CompareTag("Enemy"))
                    {
                        totalenemiesleft++;
                    }
                }
            }
        }
        //generate the next room
        if (totalenemiesleft < 1)
        {
            if (transform.GetChild(transform.childCount - 1).GetComponent<Room>().roomnumber == 16)//specific roomnumbers for bosses
            {
                bossroom = true;
                //am.Stop("Theme");
                //am.Play("BossTheme");
                room = this.gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<Room>();
                SpriteRenderer sprite1;
                sprite1 = room.transform.GetComponent<SpriteRenderer>();

                //code to pick a room here
                GameObject roomtoadd = BasicRoomInventory.Container[2].gameObject;

                SpriteRenderer sprite2;
                sprite2 = roomtoadd.transform.GetComponent<SpriteRenderer>();

                GameObject nextroom = Instantiate(roomtoadd, new Vector2(room.transform.position.x, room.transform.position.y + sprite1.bounds.extents.y + sprite2.bounds.extents.y - 0.5f), Quaternion.identity);
                nextroom.GetComponent<Room>().roomdifficulty = room.roomdifficulty + 1;
                nextroom.GetComponent<Room>().roomnumber = room.roomnumber + 1;
                nextroom.transform.parent = gameObject.transform;
                nextroom.GetComponent<SpriteRenderer>().sortingOrder = -nextroom.GetComponent<Room>().roomnumber;

                //add the boss
                GameObject bosstoadd = BossInventory.Container[UnityEngine.Random.Range(0,BossInventory.Container.Count-1)].gameObject;
                GameObject boss = Instantiate(bosstoadd, new Vector2(nextroom.transform.position.x, nextroom.transform.position.y + sprite2.bounds.extents.y - 0.5f), Quaternion.identity);
                boss.transform.parent = nextroom.transform;

                bosshp.gameObject.SetActive(true);
                bosshp.gameObject.GetComponent<BossHP>().boss = boss.GetComponent<Enemy>();


                //also deactivate the door of current room here
                nextroom.transform.Find("bridge").GetComponent<SpriteRenderer>().sortingOrder = nextroom.GetComponent<SpriteRenderer>().sortingOrder;
                room.transform.Find("Door").GetComponent<BoxCollider>().enabled = false;

                //update level ui
                level.text = room.roomdifficulty.ToString();

                if(room.roomdifficulty > PlayerPrefs.GetInt("Highscore",1))
                {
                    Debug.Log("HIGHSCORE UPDATE");
                    PlayerPrefs.SetInt("Highscore", room.roomdifficulty);
                    highscore.text = room.roomdifficulty.ToString();
                }
            }
            else //generate a normal room with baddies
            {
                bossroom = false;
                
                room = this.gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<Room>();
                SpriteRenderer sprite1;
                sprite1 = room.transform.GetComponent<SpriteRenderer>();

                //code to pick a room here
                GameObject roomtoadd = BasicRoomInventory.PickRoom();

                SpriteRenderer sprite2;
                sprite2 = roomtoadd.transform.GetComponent<SpriteRenderer>();

                GameObject nextroom = Instantiate(roomtoadd, new Vector2(room.transform.position.x, room.transform.position.y + sprite1.bounds.extents.y + sprite2.bounds.extents.y - 0.5f), Quaternion.identity);
                nextroom.GetComponent<Room>().roomdifficulty = room.roomdifficulty + 1;
                nextroom.GetComponent<Room>().roomnumber = room.roomnumber + 1;
                nextroom.transform.parent = gameObject.transform;
                nextroom.GetComponent<SpriteRenderer>().sortingOrder = -nextroom.GetComponent<Room>().roomnumber;

                //add some enemies based on difficulty and parent the room to them
                PopulateRoom(nextroom.GetComponent<Room>());

                //also deactivate the door of current room here
                nextroom.transform.Find("bridge").GetComponent<SpriteRenderer>().sortingOrder = nextroom.GetComponent<SpriteRenderer>().sortingOrder;
                room.transform.Find("Door").GetComponent<BoxCollider>().enabled = false;

                //update level ui
                level.text = room.roomdifficulty.ToString();

                if (room.roomdifficulty > PlayerPrefs.GetInt("Highscore", 1))
                {
                    Debug.Log("HIGHSCORE UPDATE");
                    PlayerPrefs.SetInt("Highscore", room.roomdifficulty);
                    highscore.text = room.roomdifficulty.ToString();
                }
            }
        }

        room = this.gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<Room>();
        if (room.playerinside)
        {
            if (bossroom)
            {
                if (am.FindSound("BossTheme") == false)
                {
                    am.Play("BossTheme");
                }
                am.Stop("Theme");
                
            }
            else
            {
                if (am.FindSound("Theme") == false)
                {
                    am.FadeIn("Theme", 0.249f);
                }
                if (am.FindSound("Theme") == true)
                {
                    am.FadeUp("Theme", 0.249f);
                }
                if (am.FindSound("BossTheme") == true)
                {
                    am.FadeOut("BossTheme", 0);
                }
            }

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(room.transform.position.x, room.transform.position.y +0.5f, Camera.main.transform.position.z), camspeed *Time.deltaTime);

            if (gameObject.transform.childCount > 1)
            {
                GameObject lastroom = this.gameObject.transform.GetChild(gameObject.transform.childCount - 2).gameObject;

                lastroom.transform.Find("Door").GetComponent<BoxCollider>().enabled = true;
            }
        }
        else
        {
            if (am.FindSound("Theme") == true)
            {
                am.FadeOut("Theme", 0.1f);
            }

            if (am.FindSound("BossTheme") == true)
            {
                am.FadeOut("BossTheme", 0);
            }
        }

        void PopulateRoom(Room emptyroom)
        {
            List<GameObject> enemylist = EnemyInventory.PickEnemies(emptyroom.roomdifficulty);

            for(int i = 0; i < enemylist.Count; i++)
            {
                EnemyInventory.PlaceEnemy(emptyroom.gameObject, enemylist[i]);

                //GameObject newenemy = Instantiate(enemylist[i], emptyroom.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                //newenemy.transform.parent = emptyroom.transform;
                //newenemy.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.3f;
            }
            for(int i = 0; i < 3; i++)
            {
                BasicObstacleInventory.PlaceObstacle(emptyroom.gameObject);
            }
        }
    }
}
