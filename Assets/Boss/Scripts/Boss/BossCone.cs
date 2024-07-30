using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCone : MonoBehaviour
{
    [SerializeField] private Transform bossTransform;
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Vector3 directionToPlayer = player.position - bossTransform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
