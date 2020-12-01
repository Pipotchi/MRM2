using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTheme : MonoBehaviour
{
    public AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am.Play("BossTheme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
