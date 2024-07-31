using System.Collections.Generic;
using UnityEngine;

public enum table
{
    forestTable,
    bossTable
}
public class RespawnManager : Singleton<RespawnManager>
{
    [SerializeField] Transform[] alchemyTables;
    public Dictionary<table, Transform> alchemyTableDic;

    private bool _isBossActivate;
    private Transform _respawnDestination;

    protected override void Awake()
    {
        base.Awake();
        alchemyTableDic = new Dictionary<table, Transform>();
    }
    private void Start()
    {
        alchemyTableDic.Add(table.forestTable, alchemyTables[0]);
        alchemyTableDic.Add(table.bossTable, alchemyTables[1]);
        _respawnDestination = alchemyTableDic[table.forestTable];
    }

    public void SetRespawnTable(AlchemyTable value)
    {
        Debug.Log("set respawn table to " + value.ToString());
        _respawnDestination = value.transform;
    }
    public void RespawnPlayer()
    {
        if (SoundManager.Instance.CurrBackgroundMusic != Audio.None) SoundManager.Instance.StopBgSound();
        SoundManager.Instance.PlayBgSoundLooped(Audio.townMusic);
        Debug.Log("respawn player");
        Player.Instance.ResetPlayer();
        Player.Instance.transform.position = _respawnDestination.position;
        SoundManager.Instance.PlaySoundOnce(Audio.respawn);
    }
    
}
