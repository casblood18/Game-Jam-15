using UnityEngine;

public class TriggerBoss : SoundTrigger
{
    [SerializeField] private GameObject _bossPref;

    [HideInInspector] public GameObject _boss;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.CompareTag("Player"))
        {
            if (_currAudio == Audio.bossBattleMusic)
                _boss = Instantiate(_bossPref);
            else
            {
                DestroyBoss();
            }
        }
    }

    public void DestroyBoss()
    {
        HUD.Instance.DeactivateBossHealth();
        Destroy(_boss.gameObject);
    }
}
