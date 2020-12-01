using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelManager : MonoBehaviour
{
    public AudioManager am;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    int num;
    bool release = false;
    bool p1 = false;
    bool p2 = false;
    bool p3 = false;
    bool p4 = false;
    float volume;
    float initialwait = 0;
    float wait = 0;

    // Start is called before the first frame update
    void Start()
    {
        am.Play("BossTheme");
        num = 1;
        panel1 = Instantiate(panel1, Vector3.zero, Quaternion.identity);
        release = true;
        p1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (p1)
        {
            panel1.transform.position = Vector3.Lerp(panel1.transform.position, new Vector3(0, -14.55f, 0), 2 * Time.deltaTime);
        }
        else
        {
            panel1.transform.position = Vector3.Lerp(panel1.transform.position, new Vector3(0, -30, 0), 2 * Time.deltaTime);
        }

        if (p2)
        {
            panel2.transform.position = Vector3.Lerp(panel2.transform.position, new Vector3(0, -14.55f, 0), 2 * Time.deltaTime);
        }
        else
        {
            panel2.transform.position = Vector3.Lerp(panel2.transform.position, new Vector3(0, -30, 0), 2 * Time.deltaTime);
        }

        if (p3)
        {
            panel3.transform.position = Vector3.Lerp(panel3.transform.position, new Vector3(0, -14.55f, 0), 2 * Time.deltaTime);
        }
        else
        {
            panel3.transform.position = Vector3.Lerp(panel3.transform.position, new Vector3(0, -30, 0), 2 * Time.deltaTime);
        }

        if (p4)
        {
            panel4.transform.position = Vector3.Lerp(panel4.transform.position, new Vector3(0, -14.55f, 0), 2 * Time.deltaTime);
        }
        else
        {
            panel4.transform.position = Vector3.Lerp(panel4.transform.position, new Vector3(0, -30, 0), 1 * Time.deltaTime);
        }

        initialwait += Time.deltaTime;
        wait += Time.deltaTime;

        if (Input.GetButtonDown("Slowdown") && wait > 0.5f && initialwait >1)
        {
            wait = 0;
            if (num == 4 && release)
            {
                am.FadeOut("BossTheme", 0);
                StartCoroutine(SwapScenes(3));
                p4 = false;
                num = 5;
            }
            if (num == 3 && release)
            {
                //Destroy(panel3);
                panel4 = Instantiate(panel4, Vector3.zero, Quaternion.identity);
                p4 = true;
                p3 = false;
                num = 4;
            }
            if (num == 2 && release)
            {
                //Destroy(panel2);
                panel3 = Instantiate(panel3, Vector3.zero, Quaternion.identity);
                p3 = true;
                p2 = false;
                num = 3;
            }
            if (num == 1 && release)
            {
                //Destroy(panel1);
                panel2 = Instantiate(panel2, Vector3.zero, Quaternion.identity);
                p2 = true;
                p1 = false;
                num = 2;
            }
        }


    }

    IEnumerator SwapScenes(float time)
    {
        yield return new WaitForSeconds(time);
        am.Stop("BossTheme");
        SceneManager.LoadScene("GameScene");
    }
}
