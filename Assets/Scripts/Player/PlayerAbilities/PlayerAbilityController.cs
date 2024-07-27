using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] HUD _HUD;
    [Header("Prefab")]
    [SerializeField] private PlayerLight _light;
    [SerializeField] GameObject _teleportMarker;

    #region TeleportVariables
    private bool _isTeleporting;
    public int teleportNum;
    private GameObject _teleportObject;
    #endregion

    private bool _isDodgeActivate;

    private void OnEnable()
    {
        InputManager.Instance.OnAttackInput += OnAttack;
        InputManager.Instance.OnTeleportInput += OnTeleport;
        InputManager.Instance.OnDodgeInput += OnDodge;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnAttackInput -= OnAttack;
        InputManager.Instance.OnTeleportInput -= OnTeleport;
        InputManager.Instance.OnDodgeInput -= OnDodge;
    }

    private void Awake()
    {
        InitTeleportMarker();
    }
    private void Start()
    {
        ChargeTeleport(1);
        SetDodgeAbility(true);
    }

    public void SetDodgeAbility(bool value)
    {
        if (value)
        {
            _isDodgeActivate = true;
            _light.Activate();
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
            _HUD.OnTeleport();
            _teleportObject.transform.position = this.transform.position;
            teleportNum--;
            _HUD.UpdateTeleportUI(teleportNum);
        }
        else
        {
            Debug.Log("Teleport back");
            _HUD.OnTeleport();
            this.transform.position = _teleportObject.transform.position;
        }

        _teleportObject.SetActive(!_isTeleporting);
        _isTeleporting = !_isTeleporting;
    }

    /// <summary>
    /// Charge teleport ability with default 1 usage.
    /// </summary>
    public void ChargeTeleport(int charge = 1)
    {
        teleportNum += charge;
        _HUD.UpdateTeleportUI(teleportNum);
    }
    #endregion

    private void OnDodge()
    {
        if (_HUD._DodgeFreeze) return;

        if (_isDodgeActivate) 
        {
            Debug.Log("player dodge");
            this.transform.position = _light.shadow.targetPosition; 
        }
    }
    private void OnAttack()
    {
        if (_HUD._attackFreeze) return;
        Debug.Log("player attack");
        //TODO: player attack Implementation
    }
}
