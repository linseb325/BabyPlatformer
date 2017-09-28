using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BabyMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float maxSpeedX;

    Rigidbody2D rb;
    Animator anim;
    // Collider2D collider;

    private bool isRunning;
    private bool inMidair;


    // Use this for initialization
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        isRunning = false;
        inMidair = false;
        // this.collider = this.GetComponent<Collider2D>();
    }


    void Update()
    {

        // Vertical input
        // float moveVertical = Input.GetAxis("Vertical");


        // Apply horizontal force, but only if we're below max running speed.
        if (Mathf.Abs(this.rb.velocity.x) < maxSpeedX)
        {
			// Horizontal input
			float moveHorizontal = Input.GetAxis("Horizontal");

			Vector2 horizontalMovementForce = new Vector2(moveHorizontal * speed, 0);
            if (inMidair) {
                this.rb.AddForce(horizontalMovementForce / 10f);
            }
            else {
				this.rb.AddForce(horizontalMovementForce);
			}
		}


        // Should we animate horizontal movement?
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!inMidair)
            {
                print("Start running because key is down this frame");
                this.anim.SetTrigger("shouldRun");
            }

            this.isRunning = true;
        }


        // Should we jump? (Second condition ensures we're not in midair trying to double jump)
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(this.rb.velocity.y) < 0.00001f)
        {
            this.anim.SetTrigger("shouldJump");
            this.inMidair = true;
            Vector2 jumpForce = new Vector2(0, this.jumpPower);
            this.rb.AddForce(jumpForce);
        }

        // Show the falling animation when we're moving downward.
        if (inMidair && this.rb.velocity.y < 0.2f)
        {
            this.anim.SetTrigger("shouldFall");
        }


        // Should we stop moving horizontally?
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.isRunning = false;
            // Show the idle animation if we're not in midair.
            if (!inMidair)
            {
                print("Idle because key is up");
                this.anim.SetTrigger("shouldIdle");
            }
            this.rb.velocity = new Vector2(0f, this.rb.velocity.y);
            //this.rb.angularVelocity = 0f;
        }

    }


    void LateUpdate()
    {
        
    }


    void FixedUpdate()
    {

    }

    // What happens when the baby hits something?
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collision!");

        // Hitting the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            this.inMidair = false;

            // Should we be running when we land, or idle?
            if (isRunning)
            {
                print("Running because we hit the ground and isRunning == true");
                this.anim.SetTrigger("shouldRun");
            }
            else
            {
                print("Idle because we hit the ground and isRunning == false");
                this.anim.SetTrigger("shouldIdle");
            }
        }
        else if (collision.gameObject.CompareTag("spikes"))
        {
            this.gameObject.SetActive(false);
        }

    }

}
