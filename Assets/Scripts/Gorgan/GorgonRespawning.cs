using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorgonRespawning : MonoBehaviour
{
    [SerializeField] private GameObject gorgonGO;

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += Respawn;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= Respawn;
    }

    private void Respawn()
    {
        //add any conditions which wouldn't allow respawning 

        gorgonGO.SetActive(true);
    }
}
