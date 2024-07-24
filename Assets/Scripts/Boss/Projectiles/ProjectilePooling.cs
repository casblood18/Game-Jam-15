using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePooling : MonoBehaviour
{
    public static ProjectilePooling Instance;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileLifetime;
    private ObjectPool<GameObject> projectilePool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        projectilePool = new ObjectPool<GameObject>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnedToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    private GameObject CreatePooledItem()
    {
        return Instantiate(projectilePrefab);
    }

    private void OnTakeFromPool(GameObject projectile)
    {
        projectile.SetActive(true);
    }

    private void OnReturnedToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject projectile)
    {
        Destroy(projectile);
    }

   public GameObject GetProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectile = projectilePool.Get();
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;

        StartCoroutine(ReleaseProjectileAfterTime(projectile, projectileLifetime));

        return projectile;
    }


    public void ReleaseProjectile(GameObject projectile)
    {
        projectilePool.Release(projectile);
    }

    private IEnumerator ReleaseProjectileAfterTime(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseProjectile(projectile);
    }
}
