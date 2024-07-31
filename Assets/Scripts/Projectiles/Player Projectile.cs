using System.Collections;
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("enemy hit");
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.DamageTaken(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            BossHealth.OnDamageBoss?.Invoke(damage);
        }
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