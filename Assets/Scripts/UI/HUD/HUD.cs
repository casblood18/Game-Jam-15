using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private float _enlargeSize = 1.2f;
    private bool _isAttack, _isTeleport, _isDodge;
    private Dictionary<VisualElement, Coroutine> _activeAnimations = new Dictionary<VisualElement, Coroutine>();

    //test
    [SerializeField] private float _playerHealthOffsetOnY = 90f;

    private PlayerHealth _playerHealth;
    private static class UIClassNames
    {
        public const string DIALOGUE = "hud-dialogue";
        public const string BOSS_BLOOD = "hud-boss-blood";
        public const string DIALOGUE_TEXT = "hud-dialogue-text";
        public const string DIALOGUE_NAME = "hud-dialogue-name";
        public const string PLAYER_HEALTH_BAR = "player-health-bar";
        public const string PLAYER_HEALTH_BAR_STICK = "player-health-bar-stick";
    }


    private static class UINames
    {
        public const string ATTACK = "Attack";
        public const string TELEPORT = "Teleport";
        public const string DODGE = "Dodge";
        public const string NPC_SPRITE = "NPCSprite";
        public const string Player_AVATAR = "Avatar";
    }

    [SerializeField] private UIDocument _uiDocument;


    //UI
    private VisualElement _root;
    private VisualElement _attackUI;
    private VisualElement _teleportUI;
    private VisualElement _dodgeUI;
    private VisualElement _dialogueUI;
    private VisualElement _bossBloodUI;
    private Label _dialogueText;
    private Label _dialogueName;
    private VisualElement _NPCSprite;
    private VisualElement _playerAvatar;
    private ProgressBar _playerHealthBar;
    private ProgressBar _playerHealthBarStick;

    private void OnEnable()
    {
        InputManager.Instance.OnAttackInput += OnAttack;
        InputManager.Instance.OnTeleportInput += OnTeleport;
        InputManager.Instance.OnDodgeInput += OnRoll;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnAttackInput -= OnAttack;
        InputManager.Instance.OnTeleportInput -= OnTeleport;
        InputManager.Instance.OnDodgeInput -= OnRoll;
    }

    private void Awake()
    {
        _root = _uiDocument.rootVisualElement;
        _attackUI = _root.Q<VisualElement>(UINames.ATTACK);
        _teleportUI = _root.Q<VisualElement>(UINames.TELEPORT);
        _dodgeUI = _root.Q<VisualElement>(UINames.DODGE);
        _dialogueUI = _root.Q<VisualElement>(className: UIClassNames.DIALOGUE);
        _bossBloodUI = _root.Q<VisualElement>(className: UIClassNames.BOSS_BLOOD);
        _dialogueText = _root.Q<Label>(className: UIClassNames.DIALOGUE_TEXT);
        _dialogueName = _root.Q<Label>(className: UIClassNames.DIALOGUE_NAME);
        _NPCSprite = _root.Q<VisualElement>(UINames.NPC_SPRITE);
        _playerAvatar = _root.Q<VisualElement>(UINames.Player_AVATAR);
        _playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        _playerHealthBar = _root.Q<ProgressBar>(className: UIClassNames.PLAYER_HEALTH_BAR);
        _playerHealthBarStick = _root.Q<ProgressBar>(className: UIClassNames.PLAYER_HEALTH_BAR_STICK);
        UpdateHealthBarPosition();
        OnPlayerInit();
        InitializeUI();
    }

    private void FixedUpdate()
    {
        if (Player.Instance != null) 
        UpdateSprite();
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

    private void InitializeUI()
    {
        SetDialogueUI(false);
        _bossBloodUI.style.display = DisplayStyle.None;
    }

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

    private Texture2D SpriteToTexture2D(object npcAvatar)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerHealthChanged(float healthValue)
    {
        _playerHealthBar.value = healthValue;
        _playerHealthBarStick.value = healthValue;
        _playerHealthBar.title = _playerHealthBar.value + "/" + _playerHealthBar.highValue;
        _playerHealthBarStick.title = _playerHealthBar.value + "/" + _playerHealthBar.highValue;
    }
    public void OnPlayerInit()
    {
        
        _playerHealthBar.highValue = Player.Instance.Stats.MaxHealth;
        _playerHealthBarStick.highValue = Player.Instance.Stats.MaxHealth;

        _playerHealthBar.value = Player.Instance.Stats.MaxHealth;
        _playerHealthBarStick.value = Player.Instance.Stats.MaxHealth;

        _playerHealthBar.title = _playerHealthBar.highValue + "/" + _playerHealthBar.highValue;
        _playerHealthBarStick.title = _playerHealthBar.highValue + "/" + _playerHealthBar.highValue;
    }

    
    private void OnAttack()
    {
        AnimateSkillUI(_attackUI, "AnimateAttack");
    }

    private void OnTeleport()
    {
        AnimateSkillUI(_teleportUI, "AnimateTeleport");
    }
    private void OnRoll()
    {
        AnimateSkillUI(_dodgeUI, "AnimateDodge");
    }
    private void AnimateSkillUI(VisualElement UI, string coroutineName)
    {
        if (_activeAnimations.ContainsKey(UI))
        {
            StopCoroutine(_activeAnimations[UI]);
        }
        _activeAnimations[UI] = StartCoroutine(coroutineName);
    }

    private IEnumerator AnimateAttack()
    {
        
        _attackUI.transform.scale = Vector3.one * _enlargeSize;
        yield return new WaitForSeconds(0.2f);
        _attackUI.transform.scale = Vector3.one;
    }
    private IEnumerator AnimateTeleport()
    {
        _teleportUI.transform.scale = Vector3.one * _enlargeSize;
        yield return new WaitForSeconds(0.2f);
        _teleportUI.transform.scale = Vector3.one;
    }
    private IEnumerator AnimateDodge()
    {
        _dodgeUI.transform.scale = Vector3.one * _enlargeSize;
        yield return new WaitForSeconds(0.2f);
        _dodgeUI.transform.scale = Vector3.one;
    }




}
