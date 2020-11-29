using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float maxHP;
    [HideInInspector] public float currentHP;
    [HideInInspector] public GameObject player;
    float injuredtimer = 0;
    float injuredmax = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
        InjuredReaction();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            currentHP -= bullet.bulletdmg;
            injuredtimer = injuredmax;
            bullet.ImpactEffect();
        }

        if (other.CompareTag("EnemyBullet"))
        {
            Vector3 direction = other.GetComponent<Rigidbody>().velocity;
            Vector3 target = player.transform.position - other.transform.position;
            float angle = Vector3.Angle(direction, target);

            Bullet bullet = other.GetComponent<Bullet>();
            bullet.ImpactEffect();

            if (angle < 45)
            {
                currentHP -= bullet.bulletdmg;
                injuredtimer = injuredmax;
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Vector3 separatedir = (transform.position - other.transform.position);
            separatedir = new Vector3(separatedir.x, 0, 0).normalized;
            if (separatedir.x != 0)
            {
                transform.position += new Vector3(separatedir.x, 0, 0);
            }
        }
    }

    protected virtual void InjuredReaction()
    {
        if (injuredtimer > 0)
        {
            injuredtimer -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
