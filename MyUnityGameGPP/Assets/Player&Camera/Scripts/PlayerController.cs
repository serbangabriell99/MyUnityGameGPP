using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static bool attack = false;
    public static int coinsCollected = 0;
    public static int diamondsCollected = 0;
    private float jumpTime = 0.54f;
    private float time1;
    
    public GameObject pickupEffect;
    public GameObject jumpParticle;
    public GameObject pickupEffectSpeed;
    public GameObject pickupEffectMagnet;
    public GameObject RunParticle;
    public bool gameOver = false;
    public GameObject youLose;
    public GameObject timer;
    public GameObject timerSpeed;
    public GameObject tutorialText;
    public GameObject magnetTutorial;
    public GameObject coinsCollectedText;
    public GameObject diamondsCollectedText;
    public GameObject tutorialTextLevel2;
    public GameObject tutorialTextLevel3;
    public GameObject timerJump;
    
    public GameObject winText;

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
    public bool MagnetCollected = false;

    public static bool level1win = false;

    Animator anim;
    private CharacterController controller;

    private Transform cameraT;

    public Transform teleportTarget1;
    public Transform teleportTarget2;
    public Transform teleportTarget3;
    public Transform teleportTarget4;

    private bool teleport1 = false;
    private bool teleport2 = false;
    private bool teleport3 = false;
    private bool teleport4 = false;

    public static bool dead = false;
    
    void Start()
    {
        
                magnetTutorial.SetActive(false);
                  tutorialText.SetActive(true);
                  timerJump.SetActive(false);
                  timer.SetActive(false);
                  timerSpeed.SetActive(false);
                  youLose.SetActive(false);
                  anim = GetComponent<Animator>();
                  cameraT = Camera.main.transform;
                  controller = GetComponent<CharacterController>();
                  doubleJumpCollected = false;
                  superSpeedCollected = false;
                  MagnetCollected = false;
                  coinsCollected = 0;
                  diamondsCollected = 0;
                  dead = false;
          
                  diamondsCollectedText.SetActive(false);
                  tutorialTextLevel2.SetActive(false);
                  tutorialTextLevel3.SetActive(false);
                  winText.SetActive(false);

    }
    
    void Update()
    { 
        if (coinsCollected > 0 || gameOver)
        {
            tutorialText.SetActive(false);
        }

        if (superSpeedCollected)
        {
            Instantiate(RunParticle, transform.position, transform.rotation);
            walkSpeed = 14;
            runSpeed = 18;
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

        if (gameOver)
        { 
            youLose.SetActive(true);
          Invoke("Restart", 1.5f);  
        }

        if (dead)
        {
            Invoke("Restart", 1f);  
        }

        if (coinsCollected == 18)
        {
            level1win = true;
            coinsCollectedText.SetActive(false);
        }
        else
        {
            level1win = false;
        }

        if (diamondsCollected == 4)
        {
            diamondsCollectedText.SetActive(false);
        }

        if (teleport1)
        {
            transform.position = teleportTarget1.transform.position;
        }

        if (teleport2)
        {
            transform.position = teleportTarget2.transform.position;
        }
        
        if (teleport3)
        {
            transform.position = teleportTarget3.transform.position;
        }
        
        if (teleport4)
        {
            transform.position = teleportTarget4.transform.position;
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
        timerJump.SetActive(false);
    }

    IEnumerator SuperSpeedTime()
    {
        yield return new WaitForSeconds(30);
        superSpeedCollected = false;
        timerSpeed.SetActive(false);
    }
    
    IEnumerator MagnetTime()
    {
        yield return new WaitForSeconds(30);
        MagnetCollected = false;
        timer.SetActive(false);
    }
    
    IEnumerator MagnetTimeTutorial()
    {
        yield return new WaitForSeconds(5);
        magnetTutorial.SetActive(false);
    }

    IEnumerator TutorialLevel2()
    {
        yield return new WaitForSeconds(5);
        tutorialTextLevel2.SetActive(false);
    }
    
    IEnumerator TutorialLevel3()
    {
        yield return new WaitForSeconds(5);
        tutorialTextLevel3.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DoubleJump")
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(other.gameObject);
            doubleJumpCollected = true;
            timerJump.SetActive(true);
            StartCoroutine(DoubleJumpTime());
        }
        
        if (other.tag == "SuperSpeed")
        {
            Instantiate(pickupEffectSpeed, transform.position, transform.rotation);
            Destroy(other.gameObject);
            superSpeedCollected = true;
            timerSpeed.SetActive(true);
            StartCoroutine(SuperSpeedTime());
        }
        
        if (other.tag == "Magnet")
        {
            Instantiate(pickupEffectMagnet, transform.position, transform.rotation);
            Destroy(other.gameObject);
            MagnetCollected = true;
            timer.SetActive(true);
            magnetTutorial.SetActive(true);
            StartCoroutine(MagnetTimeTutorial());
            StartCoroutine(MagnetTime());
        }

        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinsCollected++;
        }
        
        if (other.tag == "Diamond")
        {
            Destroy(other.gameObject);
            diamondsCollected++;
        }
        
        if (other.tag == "SilverCoin" && !MagnetCollected)
        {
            Destroy(other.gameObject);
            gameOver = true;
        }
        
        if (other.CompareTag("SceneTrigger"))
        {
            diamondsCollectedText.SetActive(true);
            tutorialTextLevel2.SetActive(true);
            StartCoroutine(TutorialLevel2());
            teleport1 = true;
        }
        else
        {
            teleport1 = false;
        }

        if (other.tag == "SceneTrigger2")
        {
            teleport2 = true;
        }
        else
        {
            teleport2 = false;
        }
        
        if (other.tag == "SceneTrigger3")
        {
            tutorialTextLevel3.SetActive(true);
            StartCoroutine(TutorialLevel3());
            teleport3 = true;
            
        }
        else
        {
            teleport3 = false;
        }
        
        if (other.tag == "SceneTrigger4")
        {
            teleport4 = true;
        }
        else
        {
            teleport4 = false;
        }
        

        if (other.CompareTag("Water"))
        {
            dead = true;
        }
        
        if (other.CompareTag("WIN"))
        {
            winText.SetActive(true);
        }

        if (other.CompareTag("EnemyWall"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyWall"))
        {
            other.gameObject.SetActive(true);
        }
    }

    void Restart()
    {
        coinsCollected = 0;
        diamondsCollected = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}

 
