using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] HUD _HUD;
    [SerializeField] float offSet = 0.1f;
    [Header("Prefab")]
    [SerializeField] public PlayerLight _light;
    [SerializeField] GameObject _teleportMarker;
    RaycastHit2D hit;

    #region TeleportVariables
    private bool _isTeleporting;
    public int teleportNum;
    private GameObject _teleportObject;
    #endregion

    private bool _isDodgeActivate;
    public bool _isDodgeGet;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject projectilePrefab;

  
    [SerializeField] private float _dodgePositionDelay = 0.1f;
    private void OnEnable()
    {
        InputManager.Instance.OnAttackInput += OnAttack;
        InputManager.Instance.OnTeleportInput += OnTeleport;
        InputManager.Instance.OnDodgeInput += OnDodge;
        Player.Instance.Stats.OnResetPlayerStats += ResetPlayerAbility;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackInput -= OnAttack;
        InputManager.Instance.OnTeleportInput -= OnTeleport;
        InputManager.Instance.OnDodgeInput -= OnDodge;
        Player.Instance.Stats.OnResetPlayerStats -= ResetPlayerAbility;
    }
    private void ResetPlayerAbility()
    {
        Debug.Log("reset player teleport to " + Player.Instance.Stats.Teleport);
        UpdateTeleportNum(Player.Instance.Stats.Teleport);
    }

    private void Awake()
    {
        InitTeleportMarker();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }
    private void Start()
    {
        SetDodgeAbility(false);
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
            Debug.Log("Teleporting");
            MovingAbilityPreparation();
            Player.Instance.playerAnimation.TeleportInAnimation();
            SoundManager.Instance.PlaySoundOnce(Audio.teleport);
            _HUD.OnTeleport();
        }

        _teleportObject.SetActive(!_isTeleporting);
        _isTeleporting = !_isTeleporting;
    }

    public void MovingAbilityPreparation()
    {
        Player.Instance.playerMovement.CanMove = false;
        _light.shadow.Deactivate();
    }

    public void TeleportOut()
    {
        Debug.Log("Teleport second animation");
        SoundManager.Instance.PlaySoundOnce(Audio.teleport);
        _HUD.OnTeleport();
        this.transform.position = _teleportObject.transform.position;
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

    #region Dodge
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
    private void OnDodge()
    {

        if (_HUD._DodgeFreeze) return;

        if (_isDodgeActivate)
        {
            MovingAbilityPreparation();
            ActivateRollAnimation();


            CheckOutOfRange();
            SoundManager.Instance.PlaySoundOnce(Audio.dodge);

        }
    }

    // Coroutine to handle the delay
    public void EnableDodgeAfterAnimation()
    {
        Player.Instance.playerAnimation.FlipXNormal();
        MovingAbilityFinish();
        Debug.Log("EnableDodgeAfterAnimation");
    }

    public void MovingAbilityFinish()
    {
        StartCoroutine(WaitForCameraRange());
        
    }
    private CameraFollow _cameraFollow;
    IEnumerator WaitForCameraRange()
    {
        while (!_cameraFollow.InCameraRange)
        {
            yield return null;
        }
        if (_isDodgeGet) _light.shadow.Activate();

        Player.Instance.playerMovement.CanMove = true;
        Debug.Log("finishAnimation");
    }

    IEnumerator TransferPlayerInDodge(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.transform.position = targetPos;
        Debug.Log("TransferPlayerInDodge");
        
    }
    Vector2 targetPos;
    private void CheckOutOfRange()
    {
        
        float rayLength = Vector2.Distance(Player.Instance.FootPosition.position, _light.shadow.targetPosition);
        Vector2 direction = (_light.shadow.targetPosition - Player.Instance.FootPosition.position).normalized;
        hit = Physics2D.Raycast(Player.Instance.FootPosition.position, direction, rayLength, _layerMask);
        
        // Check if the raycast hits something
        if (hit.collider != null)
        {
            targetPos = hit.point - direction * offSet + new Vector2(0, 0.528f);
            StartCoroutine(TransferPlayerInDodge(_dodgePositionDelay));
        }
        else
        {
            targetPos = _light.shadow.targetPosition + new Vector3(0, 0.528f, 0);
            StartCoroutine(TransferPlayerInDodge(_dodgePositionDelay));
        }
    }

    private void ActivateRollAnimation()
    {
        Player.Instance.playerAnimation.RollAnimation();
    }
    #endregion

    private void OnAttack()
    {
        if (_HUD._attackFreeze) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        CheckAttackAnimation(mousePosition);

        Vector2 direction = (mousePosition - transform.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);

        Projectile proj = projectile.GetComponent<Projectile>();
        proj.direction = direction;

        SoundManager.Instance.PlaySoundOnce(Audio.attack);
    }

    private void CheckAttackAnimation(Vector3 mousePosition)
    {
        if (mousePosition.y < transform.position.y)
        {
            Player.Instance.playerAnimation.SetAttackAnimation();
        }
    }
}
