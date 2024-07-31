using UnityEngine;

public class TriggerBoss : SoundTrigger
{
    [SerializeField] private GameObject _boss;
   
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.CompareTag("Player"))
        {
            if (_currAudio == Audio.bossBattleMusic)
                _boss.SetActive(true);
            else
            {
                _boss.SetActive(false);
            }
        }
    }
}
