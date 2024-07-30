using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 1f;

    [SerializeField] private Transform playerTransform;

    public bool InCameraRange;

    private void Awake()
    {
        playerTransform = Player.Instance.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, speed * Time.deltaTime);
        InCameraRange = Vector2.Distance(transform.position, Player.Instance.transform.position) < 10;
    }
    bool IsPlayerInCameraRange()
    {
        // Convert the player position to viewport space
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(playerTransform.position);

        // Check if the viewport position is within the bounds of the camera's viewport
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0; // Make sure the player is in front of the camera
    }


}
