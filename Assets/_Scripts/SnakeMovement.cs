using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    private Rigidbody2D rb;

    public float movementSpeed;
    public GameObject startMarker;
    public GameObject endMarker;

    // public float speed;
    private float startTime;
    private float journeyLength;

    private bool moveLeft = false;


	// Use this for initialization
	void Start ()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.startTime = Time.time;
        this.journeyLength = Vector3.Distance(endMarker.transform.position, this.transform.position);
        this.startMarker.GetComponent<SpriteRenderer>().enabled = false;
        this.endMarker.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector2.Distance(this.transform.position, endMarker.transform.position) < .1f)
        {
			// END OF JOURNEY: Reset start time, swap start/finish markers, and recalculate journey length.
			this.startTime = Time.time;
            this.swapMarkers();
            this.journeyLength = Vector3.Distance(endMarker.transform.position, startMarker.transform.position);
        }
        else {
            this.move();
        }
	}

    // Move
    private void move()
    {
        float distCovered = (Time.time - startTime) * movementSpeed;
        float fracJourneyComplete = distCovered / journeyLength;
        this.transform.position = Vector2.Lerp(startMarker.transform.position, endMarker.transform.position, fracJourneyComplete);
    }

    private void toggleMovementDirection()
    {
        if (moveLeft)
        {
            moveLeft = false;
        }
        else {
            moveLeft = true;
        }
    }

    private void swapMarkers()
    {
        GameObject temp = startMarker;
        startMarker = endMarker;
        endMarker = temp;
    }


}
