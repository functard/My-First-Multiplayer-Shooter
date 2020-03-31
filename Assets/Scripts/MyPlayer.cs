using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private Image loseScreen;

    [SerializeField]
    private Image winScreen;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawnPointRight;

    [SerializeField]
    private Transform bulletSpawnPointLeft;

    [SerializeField]
    private Image healthBar;

    private float health = 100;

    public PhotonView pv;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float jumpSpeed = 600;

    [SerializeField]
    private Text userNameText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;

    private Rigidbody2D rb;

    private GameObject sceneCamera;

    [SerializeField]
    private GameObject playerCamera;

    private Vector3 smoothMove;


    private void Start()
    {
        
        // if local player
        if (pv.IsMine)
        {
            //set name
            userNameText.text = PhotonNetwork.NickName;
            rb = GetComponent<Rigidbody2D>();

            //de-activate scene camera
            sceneCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);

            //activate player camera
            playerCamera.SetActive(true);
        }
        else
        {
            //set the enemy nickname
            userNameText.text = pv.Owner.NickName;

        }
    }

    private void Update()
    {
        //if local player
        if (pv.IsMine)
        {

            ProcessInputs();
        }
        else
        {
            if (health <= 0)
                winScreen.gameObject.SetActive(true);
            SmoothMovement();
        }
    }

    private void ProcessInputs()
    {
        if (health <= 0)
        {
            //show lose screen
            loseScreen.gameObject.SetActive(true);
        }

        Movement();
     
        //if looking right
        if (Input.GetAxis("Horizontal") > 0.1)
        {
            spriteRenderer.flipX = false;
            //update direction
            pv.RPC("OnDirectionChanged_Right", RpcTarget.Others);
        }

        //if looking left
        if (Input.GetAxis("Horizontal") < -0.1)
        {
            spriteRenderer.flipX = true;
            //update direction
            pv.RPC("OnDirectionChanged_Left", RpcTarget.Others);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Return))
        {
            Shoot();
        }

    }
    [PunRPC]
    void OnDirectionChanged_Left()
    {
        spriteRenderer.flipX = true;
    }

    [PunRPC]
    void OnDirectionChanged_Right()
    {
        spriteRenderer.flipX = false;
    }

    [PunRPC]
    void UpdateHealthBar()
    {
        health -= 15;
        //set fill amount according to hp
        healthBar.fillAmount = health / 100;

    }

    private void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    // smooth latency
    private void SmoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpSpeed);
    }

    private void Shoot()
    {
        GameObject bullet;

        //if looking left
        if (spriteRenderer.flipX)
        {
             bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPointLeft.position, Quaternion.identity);

        }
        //if looking right
        else
        {
             bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPointRight.position, Quaternion.identity);

        }
        if (spriteRenderer.flipX)
        {
            //update 
            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);    
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if local
        if(pv.IsMine)
        {
            if (collision.gameObject.tag == "Ground")
                isGrounded = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            //destroy bullet
            PhotonNetwork.Destroy(collision.gameObject);

            // if colliding self
            if (pv.IsMine)
               return;

            else
            {
                //damage enemy
                pv.RPC("UpdateHealthBar", RpcTarget.All);
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if local player
        if(pv.IsMine)
        {
            if(collision.gameObject.tag == "Ground")
                isGrounded = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if sending data
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);

        }
        //if getting data
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }
    }

}
