using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] Audio _startAudio;
    [SerializeField] Audio _endAudio;

    private bool _isStart;

    protected Audio _currAudio;
    private void Awake()
    {
        _isStart = true;
        _currAudio = Audio.None;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (SoundManager.Instance.CurrBackgroundMusic != Audio.None) SoundManager.Instance.StopBgSound();
            
            //sensitive to mob trigger ones
            if (_startAudio == Audio.mobFightMusic)
            {
                _currAudio = SoundManager.Instance.CurrBackgroundMusic == _startAudio ? _endAudio : _startAudio;
            }
            else
            {
                if (SoundManager.Instance.CurrBackgroundMusic == _endAudio) _currAudio = _startAudio;
                else _currAudio = _endAudio;
                _isStart = !_isStart;
            }
            SoundManager.Instance.PlayBgSoundLooped(_currAudio);



        }
    }
}
