using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] Shadow _shadow;
    [SerializeField] private Transform _player;
    private float _angleOffset = 90f;
    private bool _enabled;
    private Light2D lightSource;

    public Shadow shadow => _shadow;

    private void Awake()
    {
        lightSource = this.transform.GetComponent<Light2D>();
        lightSource.enabled = false;
    }
    public void Activate()
    {
        lightSource.enabled = true;
        _enabled = true;
        _shadow.Activate();
    }
    public void Deactivate()
    {
        lightSource.enabled = false;
        _enabled = false;
        _shadow.Deactivate();
    }

    private void Update()
    {
        if (!_enabled) return;

        Vector2 cursorScreenPosition = Input.mousePosition;
        Vector2 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);

        // Update the light's position
        transform.position = cursorWorldPosition;
        Vector3 direction = (transform.position - _player.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle+ _angleOffset);
    }
}
