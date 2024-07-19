using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RedProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] public float damage;

    CollisionManager collisionManager;
    public float Damage => damage;
    public Vector3 Direction { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSpan);
        collisionManager = GetComponent<CollisionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionManager.HandleCollision(gameObject, collision.gameObject);
        Destroy(gameObject);
    }

    public void Moving()
    {
        transform.Translate(Direction * (speed * Time.deltaTime));
    }
}
