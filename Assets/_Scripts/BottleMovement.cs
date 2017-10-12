using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleMovement : MonoBehaviour {

    public float movementSpeed;
    private Rigidbody2D rb;
    private bool shouldLerp = false;
    private bool firstLerp = true;
    private Vector3 firstLerpStartPos;

    public GameObject startMarker;
    public GameObject endMarker;

    private float startTime;
    private float journeyLength;

	// Use this for initialization
	void Start () {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.startMarker.GetComponent<SpriteRenderer>().enabled = false;
        this.endMarker.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
    private void Update()
    {
        if (shouldLerp)
        {
            if (Vector2.Distance(transform.position, endMarker.transform.position) < 0.1f)
            {
                print("Changing direction");
                firstLerp = false;
                swapMarkers();
                this.journeyLength = Vector2.Distance(startMarker.transform.position, endMarker.transform.position);
                this.startTime = Time.time;
            }
            else if (firstLerp)
            {
                moveFirstLerp();
            }
            else
            {
                move();
            }
        }
        else if (this.rb.velocity.x < movementSpeed) {
            Vector3 moveRight = Vector3.right * 2f;
            this.rb.AddForce(moveRight);
        }
    }


    // Start lerping when we hit the ground.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (Mathf.Abs(this.transform.position.y - this.startMarker.transform.position.y) < .1f)
            {
                if (!shouldLerp)
                {
                    print("Hit the ground. Gonna start lerping now");
                    this.firstLerpStartPos = this.transform.position;
                    this.startTime = Time.time;
                    this.journeyLength = Vector3.Distance(endMarker.transform.position, this.transform.position);
                    this.shouldLerp = true;
                }
            }
        }
    }


    private void swapMarkers()
    {
        GameObject temp = startMarker;
        startMarker = endMarker;
        endMarker = temp;
    }

    private void move()
    {
        float distCovered = (Time.time - startTime) * movementSpeed;
        float fracJourneyComplete = distCovered / journeyLength;
        this.transform.position = Vector2.Lerp(startMarker.transform.position, endMarker.transform.position, fracJourneyComplete);
    }

    private void moveFirstLerp()
    {
        float distCovered = (Time.time - startTime) * movementSpeed;
        float fracJourneyComplete = distCovered / journeyLength;
        this.transform.position = Vector2.Lerp(firstLerpStartPos, endMarker.transform.position, fracJourneyComplete);
    }





}
