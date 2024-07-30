using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerHealth _playerHealth;
    void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    public void HandleCollision(GameObject collide1, GameObject collide2)
    {
        if (collide1.CompareTag("Red") && collide2.CompareTag("Blue"))
        {
            //instantiate Purple special projectile?
            //Could add a spawner class for the new projectiles or something or just for all
            Destroy(collide1);
            Destroy(collide2);
        }
        else if(collide1.CompareTag("Blue") && collide2.CompareTag("Yellow"))
        {
            //instantiate Green special projectile?
            Destroy(collide1);
            Destroy(collide2);
        }
        else if(collide1.CompareTag("Yellow") && collide2.CompareTag("Red"))
        {
            //instantiate Orange special projectile?
            Destroy(collide1);
            Destroy(collide2);
        }
        else if ((collide1.CompareTag("Red") 
            || collide1.CompareTag("Blue") 
            || collide1.CompareTag("Green"))
            && collide2.CompareTag("Player"))
        {
            IProjectile projectile = collide1.GetComponent<IProjectile>();
            _playerHealth.DamageTaken(projectile.Damage);
        }
    }
}
