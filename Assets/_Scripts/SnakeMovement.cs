using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    private Rigidbody2D rb;

    public float movementSpeed;

    public Transform startMarker;
    public Transform endMarker;
    // public float speed;
    private float startTime;
    private float journeyLength;

    private bool moveLeft = false;


	// Use this for initialization
	void Start ()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.startTime = Time.time;
        this.journeyLength = Vector3.Distance(endMarker.position, this.transform.position);

	}
	
	// Update is called once per frame
	void Update ()
    {
        // this.rb.AddForce(Vector2.left * movementSpeed * Time.deltaTime);

        // If we're at the destination, 
        if (Vector2.Distance(this.transform.position, endMarker.position) < .1f)
        {
            this.startTime = Time.time;
            this.swapMarkers();
            this.journeyLength = Vector3.Distance(endMarker.position, startMarker.position);
        }
        else {
            this.move();
        }
	}

    // Move
    private void move()
    {
        float distCovered = (Time.time - startTime) * movementSpeed;
        float fracJourney = distCovered / journeyLength;
        this.transform.position = Vector2.Lerp(startMarker.position, endMarker.position, fracJourney);
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
        Transform temp = startMarker;
        startMarker = endMarker;
        endMarker = temp;
    }


}
