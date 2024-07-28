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

    Vector3 dir;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }
        else { yVelocity = 0; }

        Horizon = Input.GetAxisRaw("Horizontal");
        Vertic = Input.GetAxisRaw("Vertical");

        dir = new Vector3(Horizon, 0, Vertic).normalized;
        transform.LookAt(transform.position + dir);
        dir.y = yVelocity;

        cc.Move(dir * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCamera.normal = false;
        if (other.tag == "SkyViewZone")
        {
            PlayerCamera.sky = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SkyViewZone")
        {
            PlayerCamera.sky = false;
            PlayerCamera.normal = true;
        }
    }
}
