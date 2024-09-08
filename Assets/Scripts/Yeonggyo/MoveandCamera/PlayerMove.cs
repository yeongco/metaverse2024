using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;
    public float gravity = -9.81f;
    float Horizon;
    float Vertic;
    float yVelocity;
    public bool moveAvailable;
    public bool Ischair;
    public Vector3 dir;

    CharacterController cc;
    Animator anim;

    private void Awake()
    {
        moveAvailable = true;
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Ischair)
        {
            if (!cc.isGrounded)
            {
                yVelocity += gravity * Time.deltaTime;
            }
            else { yVelocity = 0; }


            dir = new Vector3(Horizon, 0, Vertic).normalized;
            if (moveAvailable)
            {
                Horizon = Input.GetAxisRaw("Horizontal");
                Vertic = Input.GetAxisRaw("Vertical");
            }
            else
            {
                dir = Vector3.zero;
            }

            anim.SetBool("IsWalk", dir != Vector3.zero);
            transform.LookAt(transform.position + dir);
            dir.y = yVelocity;

            cc.Move(dir * Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SkyViewZone")
        {
            PlayerCamera.state = 1;
        }

        else if (other.tag == "OceanViewZone")
        {
            PlayerCamera.state = 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SkyViewZone")
        {
            PlayerCamera.state = 0;
        }
        if (other.tag == "OceanViewZone")
        {
            PlayerCamera.state = 0;
        }
    }
}
