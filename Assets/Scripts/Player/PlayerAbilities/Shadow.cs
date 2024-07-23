using UnityEngine;

public class Shadow : MonoBehaviour
{
    private Transform _lightSourceTransform;
    private SpriteRenderer _playerCurrSprite;
    private SpriteRenderer _shadowSprite;
    public Vector3 targetPosition { get; private set; }
    private bool _enabled;

    public void Activate(Transform lightSource)
    {
        _lightSourceTransform = lightSource;
        _enabled = true;
    }

    [SerializeField] private float _offset = 5f;

    private void Awake()
    {
        _playerCurrSprite = transform.parent.GetComponent<SpriteRenderer>();
        _shadowSprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void UpdateSprite()
    {
        if (_shadowSprite.sprite != _playerCurrSprite.sprite)
            _shadowSprite.sprite = _playerCurrSprite.sprite;
    }

    void Update()
    {
        if (!_enabled) return;

        UpdateSprite();

        //Light Distance
        Vector3 direction = (_lightSourceTransform.position - transform.position);
        float _distanceToLight = direction.magnitude;

        //Scale
        Vector3 _localScale = this.transform.localScale;
        _localScale.y = _distanceToLight / _offset;
        this.transform.localScale = _localScale;

        //Rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        //Position
        float shadowOffset = -0.5f;
        transform.localPosition = direction.normalized * shadowOffset;
        targetPosition = this.transform.GetChild(0).position*2 - this.transform.position;
        

    }
}
