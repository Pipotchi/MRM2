using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    public GameObject bullet;
    [HideInInspector]
    public Bullet b1;
    public float bulletspreadangle = 90;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    Vector3 firevector;
    float raycasts;
    int childcount;

    // Start is called before the first frame update
    void Start()
    {
        firevector = new Vector3(0, 1, 0);
        raycasts = Mathf.Floor(360 / bulletspreadangle);
        //int targetLayer_ = (1 << 0 | 1 << 8);
        for (int i = 0; i < raycasts; i++)
        {
            int targetLayer_ = (1 << 0 | 1 << 8);
            RaycastHit[] hit = Physics.RaycastAll(transform.position, Quaternion.Euler(0, 0, bulletspreadangle * i) * firevector, 10, targetLayer_, QueryTriggerInteraction.Collide);

            float hitlength = hit.Length;
            if (hitlength > 0)
            {
                for (int j = 0; j < hitlength; j++)
                {

                    if (hit[j].transform.CompareTag("BulletPattern"))
                    {

                        //if (hit[j].transform.parent.transform.parent == this)
                        //{
                        var bulletpos = new GameObject();
                        bulletpos.transform.position = hit[j].point;
                        bulletpos.transform.parent = this.transform;

                        GameObject newbullet = Instantiate(bullet, bulletpos.transform.position, Quaternion.identity);
                        b1 = newbullet.GetComponent<Bullet>();
                        b1.bulletfollow = bulletpos;
                        //newbullet.transform.parent = bulletpos.transform;
                        //}
                    }

                }
            }
        }
        //at the end, we need to remove the bulletpattern tag from all children so they dont pick up
        //later raycasts in the future.

        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += position;
        transform.Rotate(rotation, Space.Self);
        transform.localScale += scale;

    }
}
