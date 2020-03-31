using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField]
    private float speed = 13.0f;

    [HideInInspector]
    public bool shootLeft;

    // Update is called once per frame
    void Update()
    {
        if(!shootLeft)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }

    }

    [PunRPC]
    public void ChangeDirection()
    {
        shootLeft = true;
    }
   
}
