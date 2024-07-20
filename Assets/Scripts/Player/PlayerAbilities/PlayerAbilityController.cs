using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject _teleportMarker;

    private bool _isTeleporting;
    private int _teleportNum;
    private GameObject _teleportObject;
    
    private void OnEnable()
    {
        InputManager.InputInstance.OnTeleportInput += OnTeleport;
    }

    private void Awake()
    {
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

}
