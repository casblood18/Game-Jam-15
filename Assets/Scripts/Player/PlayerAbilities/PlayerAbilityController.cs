using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Light _light;
    [SerializeField] GameObject _teleportMarker;

    #region TeleportVariables
    private bool _isTeleporting;
    private int _teleportNum;
    private GameObject _teleportObject;
    #endregion

    private bool _isRollActivate;

    private void OnEnable()
    {
        InputManager.Instance.OnTeleportInput += OnTeleport;
        InputManager.Instance.OnDodgeInput += OnRoll;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnTeleportInput -= OnTeleport;
        InputManager.Instance.OnDodgeInput -= OnRoll;
    }

    private void Awake()
    {
        InitTeleportMarker();
    }
    private void Start()
    {
        SetRollAbility(true);
    }

    public void SetRollAbility(bool value)
    {
        if (value)
        {
            _isRollActivate = true;
            _light.Activate();
        }
            
    }

    #region Teleport
    private void InitTeleportMarker()
    {
        //Initiate Teleport Marker when game awake
        _teleportObject = Instantiate(_teleportMarker);
        _teleportObject.SetActive(false);
        _teleportNum = 1;
    }

    private void OnTeleport()
    {
        if (!_isTeleporting)
        {
            if (_teleportNum == 0) return;
            
            //Debug.Log("Teleport");
            _teleportObject.transform.position = this.transform.position;
            _teleportNum--;
        }
        else
        {
            //Debug.Log("Teleport back");
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
        _teleportNum += charge;
    }
    #endregion

    private void OnRoll()
    {
        if (_isRollActivate) 
        { 
            this.transform.position = _light.shadow.targetPosition; 
        }
        
    }
}
