using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

    private Vector2 startPosition, targetPosition, velocity = Vector2.zero;
    private float randomFloat, smoothTime = 0.1f;

	// Use this for initialization
	void Start () {
        targetPosition = transform.position;
        if(Random.Range(0, 2) == 0)
        {
            randomFloat = -10;
        }
        else
        {
            randomFloat = 10;
        }
        startPosition = new Vector2(targetPosition.x + randomFloat, targetPosition.y);

        transform.position = startPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(targetPosition, transform.position) > 0.01f)
        {
            transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
	}
}
