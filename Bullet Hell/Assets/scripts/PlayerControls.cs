using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    Vector3 movevector;
    Vector3 leftmove;
    Vector3 rightmove;
    Vector3 upmove;
    Vector3 downmove;

    float movespeed;
    public float fastspeed;
    public float slowspeed;
    public float firespeed;
    public float rolltimer = 1.5f;
    float rollcount = 0;
    public float rolldistance = 1.5f;
    Vector3 rolllocation;
    float rollmeasure;
    bool rolling;
    public float rollspeed;
    int childcount;
    [HideInInspector] public bool dead = false;
    public bool hitrock = false;
    Vector3 rollpos;
    float distancetravelled;
    Vector3 lastposition;
    public float dustinterval;
    float slowinterval;
    float fastinterval;
    public GameObject dust;
    public GameObject dashcloud;
    public GameObject hitbat;
    public GameObject swing;
    AudioManager audiomanager;
    [HideInInspector] public bool insideroom = true;
    public GameObject indicator;
    bool showind = false;

    // Start is called before the first frame update
    void Start()
    {
        slowinterval = dustinterval;
        fastinterval = dustinterval / 4;
        distancetravelled = 0;
        controller = this.GetComponent<CharacterController>();
        movevector = Vector3.zero;
        animator = this.GetComponent<Animator>();
        audiomanager = FindObjectOfType<AudioManager>();

        indicator = Instantiate(indicator, transform.position, Quaternion.identity);
        indicator.transform.parent = transform;
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showind)
        {
            indicator.SetActive(true);
            var pos = Input.mousePosition;
            pos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(pos);
            worldpos.z = 0;

            var dir = worldpos - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }


        lastposition = transform.position;

        if (insideroom)
        {

            if (Input.GetButton("Slowdown"))
            {
                showind = true;
                animator.SetBool("slowdown", true);
                movespeed = slowspeed;

                transform.Find("Hitbox").gameObject.GetComponent<SpriteRenderer>().enabled = true;

                dustinterval = fastinterval;
            }
            else
            {
                animator.SetBool("slowdown", false);
                movespeed = fastspeed;

                transform.Find("Hitbox").gameObject.GetComponent<SpriteRenderer>().enabled = false;

                dustinterval = slowinterval;
            }

            //as long as the player isnt rolling
            if (!rolling && !dead)
            {

                if (Input.GetButton("Left"))
                {
                    leftmove = new Vector3(-movespeed, 0, 0);
                }
                else
                {
                    leftmove = Vector3.zero;
                }
                if (Input.GetButton("Right"))
                {
                    rightmove = new Vector3(movespeed, 0, 0);
                }
                else
                {
                    rightmove = Vector3.zero;
                }
                if (Input.GetButton("Up"))
                {
                    upmove = new Vector3(0, movespeed, 0);
                }
                else
                {
                    upmove = Vector3.zero;
                }
                if (Input.GetButton("Down"))
                {
                    downmove = new Vector3(0, -movespeed, 0);
                }
                else
                {
                    downmove = Vector3.zero;
                }

                movevector = (leftmove + rightmove + upmove + downmove).normalized * movespeed;
                controller.Move(movevector);

                animator.SetFloat("speedx", movevector.x);
                animator.SetFloat("speedy", movevector.y);
                if (movevector.magnitude != 0)
                {
                    animator.SetBool("running", true);
                }
                else
                {
                    animator.SetBool("running", false);
                }


                if (Input.GetButtonUp("Slowdown"))
                {
                    childcount = transform.childCount;
                    for (int i = 0; i < childcount; i++)
                    {
                        //starts at 2 as 0 is our hitbox and 1 is our indicator
                        StartCoroutine(FireOrbitingBullets(i * 0.05f, 2));
                        if (i % 7 == 0)
                        {
                            StartCoroutine(SwingAnim(i * 0.05f, i));
                        }
                    }

                    StartCoroutine(StopInd(0.02f +(childcount * 0.05f)));
                    //startcoroutine stopindicator (timedelay = childcount* 0.05f + 0.01f)
                }
            }

            rollcount += Time.deltaTime;

            if (rollcount > rolltimer)
            {
                if (Input.GetButton("Roll") && !dead)
                {
                    var pos = Input.mousePosition;
                    pos.z = Camera.main.nearClipPlane;
                    Vector3 worldpos = Camera.main.ScreenToWorldPoint(pos);
                    worldpos.z = 0;

                    rolllocation = transform.position + ((worldpos - transform.position).normalized * rolldistance);
                    rollcount = 0;
                    rolling = true;

                    GameObject dash1 = Instantiate(dashcloud, transform.position, Quaternion.identity);
                    var dir = rolllocation - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    angle -= 90;
                    dash1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    audiomanager.Play("PlayerDash", UnityEngine.Random.Range(0.7f, 1f));
                }

            }
            if (rolling)
            {
                rollpos = transform.position;
                controller.Move((Vector3.MoveTowards(transform.position, rolllocation, rollspeed)) - transform.position);
                if (Vector3.Distance(transform.position, rolllocation) < 0.01f || hitrock)
                {
                    rolling = false;
                }

                if ((transform.position - rollpos).magnitude < 0.1f)
                {
                    rolling = false;
                }
            }

            controller.Move(new Vector3(0, 0, -transform.position.z));

            if (!rolling)
            {
                distancetravelled += (transform.position - new Vector3(lastposition.x, lastposition.y, 0)).magnitude;

                if (distancetravelled > dustinterval)
                {
                    distancetravelled -= dustinterval;
                    GameObject d1 = Instantiate(dust, transform.position - new Vector3(0, 0.2f, 0), Quaternion.identity);
                    d1.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));

                }
            }
        }
        else
        {

            Debug.Log("force forward");
            controller.Move(new Vector3(0, movespeed, 0));
        }
        
    }
    IEnumerator SwingAnim(float time, int isEven)
    {
        yield return new WaitForSeconds(time);
        GameObject s1 = Instantiate(swing, transform.position, Quaternion.identity);
        Debug.Log("create swing");
        var pos = Input.mousePosition;
        pos.z = Camera.main.nearClipPlane;
        Vector3 worldpos = Camera.main.ScreenToWorldPoint(pos);
        worldpos.z = 0;

        var dir = worldpos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90;
        s1.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (isEven % 2 == 0)
        {
            s1.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            s1.transform.localScale = new Vector3(-1, 1, 1);
        }

        audiomanager.Play("PlayerFire", UnityEngine.Random.Range(0.3f, 0.7f));
    }

    IEnumerator FireOrbitingBullets(float time, int bulletnumber)
    {
        yield return new WaitForSeconds(time);
        if (bulletnumber < transform.childCount)
        {
            if (transform.GetChild(bulletnumber).CompareTag("Orbiting"))
            {
                GameObject hb1 = Instantiate(hitbat, transform.GetChild(bulletnumber).transform.position, Quaternion.identity);
                hb1.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));

                var pos = Input.mousePosition;
                pos.z = Camera.main.nearClipPlane;
                Vector3 worldpos = Camera.main.ScreenToWorldPoint(pos);
                worldpos.z = 0;

                transform.GetChild(bulletnumber).transform.tag = "FriendlyBullet";
                transform.GetChild(bulletnumber).transform.GetComponent<Rigidbody>().velocity = (worldpos - transform.position).normalized * firespeed;
                transform.GetChild(bulletnumber).transform.parent = null;

                
            }
        }
    }

    IEnumerator StopInd(float time)
    {
        yield return new WaitForSeconds(time);

        if (!Input.GetButton("Slowdown"))
        {
            indicator.SetActive(false);
            showind = false;
        }

    }


}
