using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float movespeed;
    public GameObject bullettype;
    public float maxHP = 3;
    [HideInInspector] public float currentHP;
    public float randomtimemin = 3;
    public float randomtimemax = 9;
    float randomtimecount = 0;
    float randommoveduration = 5;
    float randomtime;
    public bool active;
    Vector3 movevector;
    [HideInInspector] public float firetimer = 0;
    public float fireinterval = 2.5f;
    public float firespeed;
    [HideInInspector] public Room room;
    [HideInInspector] public GameObject player;
    SpriteRenderer spriterenderer;
    [HideInInspector] public Rigidbody rb;
    public int difficulty;
    [HideInInspector] public float injuredtimer = 0;
    [HideInInspector] public float injuredmax = 0.25f;
    [HideInInspector] public AudioManager audiomanager;
    public GameObject explosionring;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindWithTag("Player");
        room = transform.parent.GetComponent<Room>();
        spriterenderer = transform.GetComponent<SpriteRenderer>();
        rb = transform.GetComponent<Rigidbody>();
        //rb.maxDepenetrationVelocity = 0f;
        audiomanager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(currentHP <= 0)
        {
            Camera.main.GetComponent<ScreenShake>().shaking = true;
            audiomanager.Play("DestroyObject");
            explosionring = Instantiate(explosionring, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        InjuredReaction();
        Active();
        Move();
        Animate();
        Attack();

        if (rb.velocity.magnitude > 1f)
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyBullet"))
        {
            Bullet friendlybullet = other.GetComponent<Bullet>();
            currentHP -= friendlybullet.bulletdmg;
            friendlybullet.ImpactEffect();
            Injured(friendlybullet.GetComponent<Rigidbody>().velocity);
        }
    }

    protected virtual void Injured(Vector3 damagedirection)
    {
        float pitch = UnityEngine.Random.Range(0.9f, 1.2f);
        audiomanager.Play("HitEnemy", pitch);
        transform.position += damagedirection.normalized * 0.2f;
        injuredtimer = injuredmax;
    }

    protected virtual void InjuredReaction()
    {
        if (injuredtimer > 0)
        {
            injuredtimer -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    protected virtual void Active()
    {
        if (room.active)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }

    protected virtual void Move()
    {
        if (active)
        {
            

            randomtime = UnityEngine.Random.Range(randomtimemin, randomtimemax);
            randomtimecount += Time.deltaTime;
            randommoveduration += Time.deltaTime;
            if (randommoveduration > 1)
            {
                if (randomtimecount <= randomtime)
                {
                    movevector = (player.transform.position - transform.position).normalized * movespeed;
                }
                else
                {

                    movevector = (player.transform.position - transform.position).normalized * movespeed;
                    movevector = new Vector3(-movevector.y, movevector.x, 0) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
                    randomtimecount = 0;
                    randommoveduration = 0;
                }
            }

            rb.velocity = movevector;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    protected virtual void Animate()
    {
        if(rb.velocity.x > 0)
        {
            spriterenderer.flipX = true;
        }
        else
        {
            spriterenderer.flipX = false;
        }
    }

    protected virtual void Attack()
    {
        if (active)
        {
            firetimer += Time.deltaTime;

            if (firetimer >= fireinterval)
            {
                Vector3 pointatplayer = player.transform.position - transform.position;
                Vector3 overshoot1 = RotatePointAroundPivot(player.transform.position, transform.position, new Vector3(0, 0, UnityEngine.Random.Range(-60,60)));
                int choice = Mathf.RoundToInt(UnityEngine.Random.Range(0.1f, 0.9f));
                
                if (choice == 0)
                {
                    CreateBullet(pointatplayer, firespeed);
                }
                else
                {
                    CreateBullet(overshoot1 - transform.position, firespeed);
                }
                float pitch = UnityEngine.Random.Range(2.2f, 2.5f);
                audiomanager.Play("ShotAster", pitch);
                firetimer = 0;
            }
        }
    }

    protected virtual void CreateBullet(Vector3 direction, float speed)
    {
        GameObject newbullet = Instantiate(bullettype, transform.position, Quaternion.identity);
        Rigidbody rb2 = newbullet.GetComponent<Rigidbody>();
        rb2.velocity = direction.normalized * speed;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
