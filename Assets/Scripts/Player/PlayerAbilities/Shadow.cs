using UnityEngine;

public class Shadow : MonoBehaviour
{
    private Transform _lightSourceTransform;
    private SpriteRenderer _shadowSprite;
    public Vector3 targetPosition { get; private set; }
    private bool _enabled;

    public void Activate(Transform lightSource)
    {
        _lightSourceTransform = lightSource;
        _shadowSprite.enabled = true;
        _enabled = true;
    }
    public void Deactivate()
    {
        _shadowSprite.enabled = false;
        _enabled = false;
    }

    [SerializeField] private float _offset = 5f;

    private void Awake()
    {
        _shadowSprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _shadowSprite.enabled = false;
    }

    private void UpdateSprite()
    {
        if (Player.Instance.playerSpriteRenderer.sprite == null) return;
        if (_shadowSprite.sprite != Player.Instance.playerSpriteRenderer.sprite)
            _shadowSprite.sprite = Player.Instance.playerSpriteRenderer.sprite;
    }

    void Update()
    {
        if (!_enabled) return;

        UpdateSprite();

        //Light Distance
        Vector3 direction = (_lightSourceTransform.position - transform.position);
        float _distanceToLight = direction.magnitude;

        //Fip: when delta y<0,flip y; x<0,flip x
        if (direction.y < 0) _shadowSprite.flipX = false;
        else _shadowSprite.flipX = true;
        //if (direction.y < 0) _shadowSprite.flipY = true;
        //else _shadowSprite.flipY = false;
        //Scale
        Vector3 _localScale = this.transform.localScale;
        _localScale.y = _distanceToLight / _offset;
        this.transform.localScale = _localScale;

        //Rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        //Position
        targetPosition = this.transform.GetChild(0).position*2 - this.transform.position;
        

    }
}
