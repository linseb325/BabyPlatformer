using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BabyMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float maxSpeedX;
    public GameObject ragdollDeadBaby;
    public GameObject ragdollRespawn;
    public float powerUpSizeIncrease;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer renderer;
    Camera mainCamera;

    private bool isRunning = false;
    private bool inMidair = false;
    private bool isAlive = true;

    private Vector3 cameraScale;
    private Vector3 backgroundScale;
    private Vector3 babyScaleOriginal;
    private float cameraSizeOriginal;


    // Use this for initialization
    void Start()
    {
        this.mainCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();

		this.ragdollRespawn.transform.position = this.transform.position;
		this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.renderer = this.GetComponent<SpriteRenderer>();
        // (Should already be false) this.ragdollDeadBaby.GetComponent<SpriteRenderer>().enabled = false;
        // (Should already be false) this.ragdollRespawn.GetComponent<SpriteRenderer>().enabled = false;

        this.cameraSizeOriginal = mainCamera.orthographicSize;

        this.babyScaleOriginal = transform.localScale;
        this.cameraScale = this.transform.GetChild(0).localScale;
        this.backgroundScale = this.transform.GetChild(0).GetChild(0).localScale;
        // print("Camera scale: " + this.cameraScale);
        // print("Background scale: " + this.backgroundScale);

    }


    void Update()
    {
        if (isAlive)
        {
			// Vertical input
			// float moveVertical = Input.GetAxis("Vertical");


			// Apply horizontal force, but only if we're below max running speed.
			if (Mathf.Abs(this.rb.velocity.x) < maxSpeedX)
			{
				// Horizontal input
				float moveHorizontal = Input.GetAxis("Horizontal");

				Vector2 horizontalMovementForce = new Vector2(moveHorizontal * speed, 0);
				if (inMidair)
				{
					this.rb.AddForce(horizontalMovementForce / 10f);
				}
				else
				{
					this.rb.AddForce(horizontalMovementForce);
				}
			}


			// Should we animate horizontal movement?
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (!inMidair)
				{
					this.anim.SetTrigger("shouldRun");
				}

				this.isRunning = true;
			}


			// Should we stop moving horizontally?
			if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				this.isRunning = false;
				// Show the idle animation if we're not in midair.
				if (!inMidair)
				{
					// print("Idle because key is up");
					this.anim.SetTrigger("shouldIdle");
				}
				this.rb.velocity = new Vector2(0f, this.rb.velocity.y);
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
			if (inMidair && this.rb.velocity.y < 0.1f)
			{
				this.anim.SetTrigger("shouldFall");
			}
		}


    }


    // What happens when the baby hits something?
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hitting the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            this.inMidair = false;

            // Should we be running when we land, or idle?
            if (isRunning)
            {
                // print("Running because we hit the ground and isRunning == true");
                this.anim.SetTrigger("shouldRun");
            }
            else
            {
                // print("Idle because we hit the ground and isRunning == false");
                this.anim.SetTrigger("shouldIdle");
            }
        } else if (collision.gameObject.CompareTag("bottle")) {
            Destroy(collision.gameObject);
            this.transform.localScale *= powerUpSizeIncrease;
            // this.transform.GetChild(0).localScale /= powerUpSizeIncrease;
            this.mainCamera.orthographicSize *= powerUpSizeIncrease;
            // this.transform.GetChild(0).GetChild(0).localScale *= powerUpSizeIncrease;
            print("Camera scale: " + this.transform.GetChild(0).localScale);
            print("Background scale: " + this.transform.GetChild(0).GetChild(0).localScale);
            print("Picked up a bottle!");
        }
    }

    private void Kill()
    {
        // Time to die
        if (isAlive)
        {
            this.ragdollDeadBaby.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);
            this.isAlive = false;
            this.renderer.enabled = false;
            this.ragdollDeadBaby.GetComponent<SpriteRenderer>().enabled = true;
            this.ragdollDeadBaby.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000f));

            // Respawn 3 seconds after dying
            Invoke("Respawn", 3);
        }
	}

    private void Respawn()
    {
		this.rb.transform.position = this.ragdollRespawn.transform.position;
        this.ragdollDeadBaby.GetComponent<SpriteRenderer>().enabled = false;
        this.ragdollDeadBaby.transform.position = this.ragdollRespawn.transform.position;
		this.isAlive = true;
		this.isRunning = false;
		this.inMidair = false;
        this.transform.localScale = this.babyScaleOriginal;
        this.mainCamera.orthographicSize = cameraSizeOriginal;
        // TODO: Fix background size/scale
		this.renderer.enabled = true;
		this.anim.SetTrigger("shouldIdle");

	}

}
