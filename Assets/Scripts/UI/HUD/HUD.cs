using NS.RomanLib;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private float _enlargeSize = 1.2f;
    private bool _isAttack, _isTeleport, _isDodge;
    private int _teleportNum => Player.Instance.GetComponent<PlayerAbilityController>().teleportNum;
    //test
    [SerializeField] private float _playerHealthOffsetOnY = 90f;
    [SerializeField] private float _coolDownAttack = 0.2f;
    [SerializeField] private float _coolDownTeleport = 1f;
    [SerializeField] private float _coolDownDodge = 0.5f;

    public bool _attackFreeze;
    public bool _teleportFreeze;
    public bool _DodgeFreeze;

    private PlayerHealth _playerHealth;
    private static class UIClassNames
    {
        public const string DIALOGUE = "hud-dialogue";
        public const string BOSS_HEALTH_BAR = "boss-health-bar";
        public const string DIALOGUE_TEXT = "hud-dialogue-text";
        public const string DIALOGUE_NAME = "hud-dialogue-name";
        public const string PLAYER_HEALTH_BAR = "player-health-bar";
        public const string PLAYER_HEALTH_BAR_STICK = "player-health-bar-stick";
        public const string PLAYER_HEALTH_BAR_CONTAINER = "player-health-bar-container";
    }

    private static class UINames
    {
        public const string ATTACK = "Attack";
        public const string TELEPORT = "Teleport";
        public const string DODGE = "Dodge";
        public const string NPC_SPRITE = "NPCSprite";
        public const string PLAYER_AVATAR = "Avatar";
        public const string TELEPORT_NUM = "TeleportNum";
    }

    [SerializeField] private UIDocument _uiDocument;

    //UI
    private VisualElement _root;
    private VisualElement _attackUI;
    private VisualElement _teleportUI;
    private VisualElement _dodgeUI;
    private VisualElement _dialogueUI;
    private VisualElement _bossHealthBar;
    private Label _dialogueText;
    private Label _dialogueName;
    private VisualElement _NPCSprite;
    private VisualElement _playerAvatar;
    private ProgressBar _playerHealthBar;
    private ProgressBar _playerHealthBarStick;
    private Label _teleportNumLabel;

    private void OnEnable()
    {
        InputManager.Instance.OnAttackInput += OnAttack;
        InputManager.Instance.OnDodgeInput += OnDodge;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnAttackInput -= OnAttack;
        InputManager.Instance.OnDodgeInput -= OnDodge;
    }

    private void Awake()
    {
        _root = _uiDocument.rootVisualElement;
        _attackUI = _root.Q<VisualElement>(UINames.ATTACK);
        _teleportUI = _root.Q<VisualElement>(UINames.TELEPORT);
        _dodgeUI = _root.Q<VisualElement>(UINames.DODGE);
        _dialogueUI = _root.Q<VisualElement>(className: UIClassNames.DIALOGUE);
        _bossHealthBar = _root.Q<VisualElement>(className: UIClassNames.BOSS_HEALTH_BAR);
        _dialogueText = _root.Q<Label>(className: UIClassNames.DIALOGUE_TEXT);
        _dialogueName = _root.Q<Label>(className: UIClassNames.DIALOGUE_NAME);
        _NPCSprite = _root.Q<VisualElement>(UINames.NPC_SPRITE);
        _playerAvatar = _root.Q<VisualElement>(UINames.PLAYER_AVATAR);
        //_playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        _playerHealthBar = _root.Q<ProgressBar>(className: UIClassNames.PLAYER_HEALTH_BAR);
        _playerHealthBarStick = _root.Q<ProgressBar>(className: UIClassNames.PLAYER_HEALTH_BAR_STICK);
        _teleportNumLabel = _root.Q<Label>(UINames.TELEPORT_NUM);

        UpdateHealthBarPosition();
        OnPlayerInit();
        InitializeUI();
    }
    #region Initialization
    private void InitializeUI()
    {
        SetDialogueUI(false);
        _bossHealthBar.style.display = DisplayStyle.None;
        var container = _bossHealthBar.Q<VisualElement>(className: "unity-progress-bar__container");
        container.style.width = 700f;
        container.style.maxHeight = StyleKeyword.None;
        container.style.height = 50f;

    }
    public void OnPlayerInit()
    {
        _playerHealthBar.highValue = Player.Instance.Stats.MaxHealth;
        _playerHealthBarStick.highValue = Player.Instance.Stats.MaxHealth;

        _playerHealthBar.value = Player.Instance.Stats.MaxHealth;
        _playerHealthBarStick.value = Player.Instance.Stats.MaxHealth;

        _playerHealthBar.title = _playerHealthBar.highValue + "/" + _playerHealthBar.highValue;
        _playerHealthBarStick.title = _playerHealthBar.highValue + "/" + _playerHealthBar.highValue;

        var container = _playerHealthBarStick.Q<VisualElement>(className: "unity-progress-bar__container");
        container.style.width = 100f;
    }
    #endregion

    #region Updates
    private void FixedUpdate()
    {
        if (Player.Instance != null) 
        UpdateSprite();
    }
    private void Update()
    {
        if (Player.Instance != null && Camera.main != null)
            UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Player.Instance.transform.position);
        Vector2 uiPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y);

        // Update the health bar's position
        _playerHealthBarStick.style.left = uiPosition.x - _playerHealthBarStick.resolvedStyle.width/2;
        _playerHealthBarStick.style.top = uiPosition.y - _playerHealthBarStick.resolvedStyle.height / 2 + _playerHealthOffsetOnY;
    }

    private void UpdateSprite()
    {
        StyleBackground newSprite =  new StyleBackground(Player.Instance.playerSpriteRenderer.sprite);
        
        if (_playerAvatar.style.backgroundImage != newSprite)
            _playerAvatar.style.backgroundImage = newSprite;
    }

    public void UpdateTeleportUI(int value)
    {
        _teleportNumLabel.text = value.ToString();
    }
    #endregion

    public void OnPlayerHealthChanged(float healthValue)
    {
        _playerHealthBar.value = healthValue;
        _playerHealthBarStick.value = healthValue;
        _playerHealthBar.title = _playerHealthBar.value + "/" + _playerHealthBar.highValue;
        _playerHealthBarStick.title = _playerHealthBar.value + "/" + _playerHealthBar.highValue;
    }
    
    #region Ability
    private void OnAttack()
    {
        if (_attackFreeze) return;
        //Debug.Log("OnAttack");
        StartCoroutine(AnimateAttack());
    }

    public void OnTeleport()
    {
        if (_teleportFreeze) return;
        //Debug.Log("OnTeleport");
        StartCoroutine(AnimateTeleport());
    }

    private void OnDodge()
    {
        if (_DodgeFreeze) return;
        //Debug.Log("OnDodge");
        StartCoroutine(AnimateDodge());
    }

    private IEnumerator AnimateAttack()
    {
        _attackFreeze = true;
        float elapsedTime = 0;
        float duration = _coolDownAttack;
        float currentValue = 0;
        float startValue = 0;
        float endValue = 1;
        var radialElement = _attackUI.Q<RadialFillElement>();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            radialElement.value = currentValue;
            yield return null;
        }
        currentValue = 0;
        radialElement.value = currentValue;
        //Debug.Log("attackFinish");
        _attackFreeze = false;
    }
    private IEnumerator AnimateTeleport()
    {
        _teleportFreeze = true;
        float elapsedTime = 0;
        float duration = _coolDownTeleport;
        float currentValue = 0;
        float startValue = 0;
        float endValue = 1;
        var radialElement = _teleportUI.Q<RadialFillElement>();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            radialElement.value = currentValue;
            yield return null;
        }
        currentValue = 0;
        radialElement.value = currentValue;
       // Debug.Log("TeleportFinish");
        _teleportFreeze = false;
    }
    private IEnumerator AnimateDodge()
    {
        _DodgeFreeze = true;
        float elapsedTime = 0;
        float duration = _coolDownDodge;
        float currentValue = 0;
        float startValue = 0;
        float endValue = 1;
        var radialElement = _dodgeUI.Q<RadialFillElement>();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            radialElement.value = currentValue;
            yield return null;
        }
        currentValue = 0;
        radialElement.value = currentValue;
        //Debug.Log("DodgeFinish");
        _DodgeFreeze = false;
    }
    #endregion

    #region Dialogue
    public void SetDialogueUI(bool value)
    {
        if (value)
            _dialogueUI.style.display = DisplayStyle.Flex;
        else
            _dialogueUI.style.display = DisplayStyle.None;
    }

    public void SetDialogueContext(string _talkText)
    {
        _dialogueText.text = _talkText;
    }
    public void SetDialogueNPC(string _npcName, Sprite ncpAvatar)
    {
        _dialogueName.text = _npcName;
        _NPCSprite.style.backgroundImage = new StyleBackground(ncpAvatar);
    }
    #endregion
}
