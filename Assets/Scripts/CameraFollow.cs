using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 1f;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = Player.Instance.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, speed * Time.deltaTime);
    }
}
