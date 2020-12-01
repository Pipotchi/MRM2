using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    public float maxHP = 2;
    float currentHP;
    float injuredmax = 3;
    float injuredtimer = 0;
    float blinkinterval = 0.3f;
    SpriteRenderer sprite;
    GameObject player;
    public TextMeshProUGUI respawntext;
    AudioManager audiomanager;
    public GameObject heart1;
    public GameObject heart2;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        respawntext.enabled = false;
        player = transform.parent.gameObject;
        currentHP = maxHP;
        sprite = player.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP == 2){
            heart1.SetActive(true);
            heart2.SetActive(true);
        }
        if (currentHP == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
        }
        if (currentHP == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
        }


        injuredtimer -= Time.deltaTime;
        if (injuredtimer > 0)
        {
            int isEven = Mathf.FloorToInt((injuredtimer) / blinkinterval);
            if (isEven % 2 == 0)
            {
                sprite.enabled = true;
                sprite.color = new Color(1, 0, 0, 1);
            }
            else
            {
                sprite.enabled = false;
            }
        }
        else
        {
            sprite.enabled = true;
            sprite.color = new Color(1, 1, 1, 1);
        }

        if (currentHP <= 0)
        {
            sprite.color = new Color(0, 0, 0, 1);
            respawntext.enabled = true;
            player.GetComponent<PlayerControls>().dead = true;
            player.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;

            if (Input.GetButton("Space"))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if (injuredtimer < 0)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                currentHP -= bullet.bulletdmg;
                injuredtimer = injuredmax;
                bullet.ImpactEffect();
                audiomanager.Play("DestroyObject", 0.5f);
            }
        }
    }

    
}
