using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    public Vector2 direction;
    public float damage;
    public float destroyTime = 4f;

    private void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        StartCoroutine(DestroyAfterTime(destroyTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //add damage dealing to enemy
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}