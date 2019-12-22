using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField] private GameObject thePlayer;
    private GameManager theGameManager;
    [SerializeField] private float smoothTime;
    [SerializeField] private int yOffset;

    private Vector2 velocity;

    private void Update()
    {
        Vector3 targetPosition = thePlayer.transform.TransformPoint(new Vector3(0, yOffset, -10));

        if (targetPosition.y < transform.position.y) return;

        targetPosition = new Vector3(0, targetPosition.y);
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

}
