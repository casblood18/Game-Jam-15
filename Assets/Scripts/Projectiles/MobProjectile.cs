using System.Collections;
using UnityEngine;

public class MobProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    Vector3 direction;
    public float destroyTime = 5f;

    [SerializeField] private Transform target;

    public void ProjectileDirection(Transform target)
    {
        this.target = target;
    }

    private void Start()
    {
        direction = (target.position - transform.position).normalized;
        StartCoroutine(DestroyAfterTime(destroyTime));
    }
    void Update()
    {
        if (target != null)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                SoundManager.Instance.PlaySoundOnce(Audio.bossAttack);
                playerHealth.DamageTaken(damage);
            }
            Destroy(gameObject);
        }
    }
}
