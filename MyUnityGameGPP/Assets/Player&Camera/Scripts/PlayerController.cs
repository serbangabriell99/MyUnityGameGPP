using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool attack = false;
    public static bool isMagnet;
    private float jumpTime = 0.54f;
    private float time1;
    public GameObject pickupEffect;
    public GameObject jumpParticle;
    public GameObject pickupEffectSpeed;
    public GameObject RunParticle;

    public float walkSpeed = 4;
    public float runSpeed = 8;
    public float gravity = -25;
    public float jumpHeight = 2;
    [Range(0,1)]
    public float airControlPercent;
    
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;
    
    public float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private float velocityY;

    private bool can_doubleJump = false;
    private bool doubleJumpCollected = false;
    private bool superSpeedCollected = false;
    
    Animator anim;
    private CharacterController controller;

    private Transform cameraT;

    void Start()
    {
        anim = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        isMagnet = false;
    }
    
    void Update()
    {

        if (superSpeedCollected)
        {
            Instantiate(RunParticle, transform.position, transform.rotation);
            walkSpeed = 18;
            runSpeed = 25;
        }
        else
        {
            walkSpeed = 8;
            runSpeed = 14;
        }
        //input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        
        Move(inputDir, running);

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("attack", true);
            attack = true;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            StartCoroutine(attackTime());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        //animator
        float animationSpeedPercent = ((running) ? currentSpeed/runSpeed : currentSpeed/walkSpeed * .5f);
        anim.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        if (anim.GetBool("jumping") && Time.time > time1)
        {
            anim.SetBool("jumping", false);
        }
        
        if (anim.GetBool("doubleJump") && Time.time > time1 )
        {
            anim.SetBool("doubleJump", false);
        }

        if (!controller.isGrounded)
        {
            anim.SetBool("inAir", true);
        }
        else
        {
            anim.SetBool("inAir", false);
        }
    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, 
                                        ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime) ); 
        }
        
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            can_doubleJump = true;
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
            time1 = Time.time + jumpTime;
            anim.SetBool("jumping", true); 
        }
        else
        {
            if (can_doubleJump && doubleJumpCollected)
            {
                 anim.SetBool("doubleJump", true);
                 Instantiate(jumpParticle, transform.position, transform.rotation);
                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight * 2);
                velocityY = jumpVelocity;
                time1 = Time.time + jumpTime;

                can_doubleJump = false;   
            }
            
        }
    }



    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }

    

    IEnumerator attackTime()
    {
        yield return new WaitForSeconds(0.40f);
        anim.SetBool("attack", false);
        attack = false;
    }

    IEnumerator DoubleJumpTime()
    {
        yield return new WaitForSeconds(30);
        doubleJumpCollected = false;
    }

    IEnumerator SuperSpeedTime()
    {
        yield return new WaitForSeconds(30);
        superSpeedCollected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DoubleJump")
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(other.gameObject);
            doubleJumpCollected = true;
            StartCoroutine(DoubleJumpTime());
        }
        
        if (other.tag == "SuperSpeed")
        {
            Instantiate(pickupEffectSpeed, transform.position, transform.rotation);
            Destroy(other.gameObject);
            superSpeedCollected = true;
            StartCoroutine(SuperSpeedTime());
        }

        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
    }
}

 
