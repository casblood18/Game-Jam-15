using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private SpriteRenderer playerSprite;

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
        SetDodgeAbility(false);
    }

    public void SetDodgeAbility(bool value)
    {
        if (value)
        {
            _isDodgeActivate = true;
            _light.Activate();
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
            teleportNum--;
            _HUD.UpdateTeleportUI(teleportNum);
        }
        else
        {
            playerAnimation.TeleportInAnimation();
        }

        _teleportObject.SetActive(!_isTeleporting);
        _isTeleporting = !_isTeleporting;
    }

    public void TeleportOut()
    {
        Debug.Log("Teleport back");
        SoundManager.Instance.PlaySoundOnce(Audio.teleport);
        _HUD.OnTeleport();
        this.transform.position = _teleportObject.transform.position;
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
            ActivateRollAnimation();
            SoundManager.Instance.PlaySoundOnce(Audio.dodge);
            this.transform.position = _light.shadow.targetPosition; 
        }
    }

    private void ActivateRollAnimation()
    {
        Vector2 moveInput = InputManager.Instance.PlayerInputActions.Player.Move.ReadValue<Vector2>();

        // Check for horizontal movement
        if (moveInput.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            FlipXNormal();
        }
        playerAnimation.RollAnimation();
    }

    public void FlipXNormal()
    {
        playerSprite.flipX = true;
    }
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
            playerAnimation.SetAttackAnimation();
        }
    }
}
