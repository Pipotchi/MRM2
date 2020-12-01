using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public Enemy boss;
    RectTransform hplength;

    // Start is called before the first frame update
    void Start()
    {
        hplength = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float length = boss.currentHP / boss.maxHP;
        if (boss)
        {
            hplength.localScale = new Vector3(length, 1, 1);
        }
        if(length <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
