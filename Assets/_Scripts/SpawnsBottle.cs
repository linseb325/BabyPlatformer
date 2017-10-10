using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsBottle : MonoBehaviour {

    public GameObject bottle;
    private float bottleHeight;
    private bool didSpawn;

	// Use this for initialization
    void Start () {
        this.bottleHeight = bottle.GetComponent<BoxCollider2D>().bounds.size.y;
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Vector3 point = collision.contacts[0].point;
			if (point.y > collision.gameObject.transform.position.y)
			{
				// We've hit the bottom of the box (or close).
				if (!didSpawn)
				{
					spawnBottle();
				}
			}

		}
    }

    private void spawnBottle() {
        Vector3 floorColliderSize = this.GetComponent<Collider2D>().bounds.size;
        float floorHeight = floorColliderSize.y;
        float offset = 0.5f * (bottleHeight + floorHeight) + .05f;
        Vector3 floorCenter = this.GetComponent<Collider2D>().bounds.center;
        this.bottle.transform.position = floorCenter + new Vector3(0, offset, 0);
        this.bottle.gameObject.SetActive(true);
        didSpawn = true;
    }
}
