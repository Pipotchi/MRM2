using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Color color;
    [HideInInspector]
    public GameObject bulletfollow = null;
    [HideInInspector]
    public bool nearplayer = false;
    [HideInInspector]
    public bool inorbit = false;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public float orbittimer = 0;
    public float orbitthreshold = 0.4f;
    [HideInInspector]
    public bool friendly = false;
    [HideInInspector]
    public bool cw;
    [HideInInspector]
    public Vector3 cross;
    [HideInInspector]
    public Vector3 vel;
    [HideInInspector]
    public float angle;
    [HideInInspector]
    public int listpos;
    CatchBullets catchbullets;
    public GameObject destroyeffect = null;
    public float bulletdmg;
    TrailRenderer trail= null;
    float trailoriginal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(this.GetComponent<TrailRenderer>() != null)
        {
            trail = this.GetComponent<TrailRenderer>();
            trailoriginal = trail.time;
        }
        catchbullets = GameObject.FindWithTag("Player").GetComponent<CatchBullets>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        /*
        if (!friendly)
        {
            if (bulletfollow)
            {
                transform.GetComponent<Rigidbody>().velocity = bulletfollow.transform.position - transform.position;
            }
        }*/

        if (!friendly)
        {
            if (bulletfollow)
            {
                transform.GetComponent<Rigidbody>().velocity = bulletfollow.transform.position - transform.position;
            }

            if (inorbit)
            {
                orbittimer += Time.deltaTime;
            }
            else
            {
                orbittimer = 0;
            }
            if (orbittimer >= orbitthreshold)
            {
                friendly = true;
                gameObject.tag = "Orbiting";
                Vector3 direction = this.GetComponent<Rigidbody>().velocity;
                Vector3 target = player.transform.position - transform.position;
                vel = this.GetComponent<Rigidbody>().velocity;
                cross = Vector3.Cross(direction, target);
                this.GetComponent<SpriteRenderer>().color += new Color(0.3f, 0.3f, 0.3f, 0);
                if (cross.z > 0)
                {
                    cw = true;
                }
                else
                {
                    cw = false;
                }
            }
        }

        if (gameObject.CompareTag("Orbiting"))
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.parent = null;
            this.transform.parent = player.transform;

            if (trail != null)
            {
                if (trail.time > 0.1f)
                {
                    trail.time -= Time.deltaTime * 0.3f;
                }
                else
                {
                    trail.time = 0.1f;
                }
            }


            if (cw && this.transform.parent == player.transform)
            {
                transform.RotateAround(transform.parent.transform.position - (transform.position - transform.parent.transform.position), new Vector3(0, 0, 1), vel.magnitude);
            }
            if (!cw && this.transform.parent == player.transform)
            {
                transform.RotateAround(transform.parent.transform.position - (transform.position - transform.parent.transform.position), new Vector3(0, 0, 1), -(vel.magnitude));
            }

        }

        if (gameObject.CompareTag("FriendlyBullet"))
        {
            if (trail != null)
            {
                trail.time = trailoriginal;
            }
        }
    }
    
    /*
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("FriendlyBullet"))
        {
            if (other.CompareTag("Enemy"))
            {
                ImpactEffect(other.gameObject);
            }
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }*/


    public virtual void ImpactEffect()
    {
        DestroyEffect();
        Destroy(gameObject);
    }

    protected virtual void DestroyEffect()
    {
        if(destroyeffect != null)
        {
            GameObject destroyeffectbullet = Instantiate(destroyeffect, transform.position, Quaternion.identity);
            destroyeffectbullet.transform.localScale = new Vector3((2 * UnityEngine.Random.Range(0, 2)) - 1, 1, 1);
        }
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected virtual Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        //i can use this at some point instead of rotate around :/
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
