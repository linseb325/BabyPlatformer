using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleMovement : MonoBehaviour {

    public float maxSpeed;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        this.rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {
        if (this.rb.velocity.x < maxSpeed) {
            Vector3 right = Vector3.right * 2f;
            this.rb.AddForce(right);
        }
    }
}
