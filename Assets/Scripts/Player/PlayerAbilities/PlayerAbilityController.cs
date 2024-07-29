using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] HUD _HUD;
    [SerializeField] float offSet = 0.1f;
    [Header("Prefab")]
    [SerializeField] private PlayerLight _light;
    [SerializeField] GameObject _teleportMarker;
    private Player player;
    RaycastHit2D hit;

    #region TeleportVariables
    private bool _isTeleporting;
    public int teleportNum;
    private GameObject _teleportObject;
    #endregion

    private bool _isDodgeActivate;
    public bool _isDodgeGet;

    [SerializeField] private LayerMask _layerMask;

    private void OnEnable()
    {
        InputManager.Instance.OnAttackInput += OnAttack;
        InputManager.Instance.OnTeleportInput += OnTeleport;
        InputManager.Instance.OnDodgeInput += OnDodge;
        player.Stats.OnResetPlayerStats += ResetPlayerAbility;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnAttackInput -= OnAttack;
        InputManager.Instance.OnTeleportInput -= OnTeleport;
        InputManager.Instance.OnDodgeInput -= OnDodge;
        player.Stats.OnResetPlayerStats -= ResetPlayerAbility;
    }
    private void ResetPlayerAbility()
    {
        Debug.Log("reset player teleport to " + Player.Instance.Stats.Teleport);
        UpdateTeleportNum(Player.Instance.Stats.Teleport);
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        InitTeleportMarker();
    }
    private void Start()
    {
        ChargeTeleport(1);
        SetDodgeAbility(false);
    }
    public void SetDodgeAbility(bool value)
    {
        if (value)
        {
            _isDodgeActivate = true;
            _light.Activate();
            _isDodgeGet = true;
        }
        else
        {
            _isDodgeActivate = false;
            _light.Deactivate();
        }
    }

    #region Teleport
    private void InitTeleportMarker()
    {
        //Initiate Teleport Marker when game awake
        _teleportObject = Instantiate(_teleportMarker);
        _teleportObject.SetActive(false);
    }

    private void OnTeleport()
    {
        if (_HUD._teleportFreeze) return;

        if (!_isTeleporting)
        {
            if (teleportNum == 0) return;

            Debug.Log("Teleport");
            SoundManager.Instance.PlaySoundOnce(Audio.teleport);
            _HUD.OnTeleport();
            _teleportObject.transform.position = this.transform.position;
            UpdateTeleportNum(teleportNum-1);
        }
        else
        {
            Debug.Log("Teleport back");
            SoundManager.Instance.PlaySoundOnce(Audio.teleport);
            _HUD.OnTeleport();
            this.transform.position = _teleportObject.transform.position;
        }

        _teleportObject.SetActive(!_isTeleporting);
        _isTeleporting = !_isTeleporting;
    }

    private void UpdateTeleportNum(int value)
    {
        teleportNum = value;
        _HUD.UpdateTeleportUI(teleportNum);
    }

    /// <summary>
    /// Charge teleport ability with default 1 usage.
    /// </summary>
    public void ChargeTeleport(int charge = 1)
    {
        UpdateTeleportNum(teleportNum + charge);
    }
    #endregion

    private void OnDodge()
    {
        if (_HUD._DodgeFreeze) return;

        if (_isDodgeActivate) 
        {
            CheckOutOfRange();
            Debug.Log("player dodge");
            SoundManager.Instance.PlaySoundOnce(Audio.dodge);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(transform.position, _light.shadow.targetPosition - this.transform.position, Color.red);
    //}
    private void CheckOutOfRange()
    {
        
        float rayLength = Vector2.Distance(player.FootPosition.position, _light.shadow.targetPosition);
        Vector2 direction = (_light.shadow.targetPosition - player.FootPosition.position).normalized;
        hit = Physics2D.Raycast(player.FootPosition.position, direction, rayLength, _layerMask);
        
        // Check if the raycast hits something
        if (hit.collider != null)
        {
            //Debug.Log("Raycast hit: " + hit.collider.name);
            //Debug.DrawRay(transform.position, direction * rayLength, Color.blue);
            Vector2 targetPos = hit.point - direction * offSet + new Vector2(0, 0.528f);
            this.transform.position = targetPos;
        }
        else
        {
            Vector3 targetPos = _light.shadow.targetPosition + new Vector3(0, 0.528f, 0);
            this.transform.position = targetPos;
        }
    }

    private void OnAttack()
    {
        if (_HUD._attackFreeze) return;
        Debug.Log("player attack");

        SoundManager.Instance.PlaySoundOnce(Audio.attack);
        //TODO: player attack Implementation

    }
}
