using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingBullet : Bullet
{
    protected override void Update()
    {
        base.Update();
        if (gameObject.tag != "Orbiting")
        {
            Vector3 moveVec = transform.GetComponent<Rigidbody>().velocity;
            //angle between 0,1,0 and movement vector rotate by
            float rotateamount = Vector3.Angle(new Vector3(0, 1, 0), moveVec);
            if (moveVec.x < 0)
            {
                rotateamount *= -1;
            }
            Debug.Log(rotateamount);
            transform.rotation = Quaternion.Euler(0, 0, 360 - rotateamount);
        }
    }
}
