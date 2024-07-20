using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    private const string REBINDSKEY = "rebinds";

    [SerializeField] private InputActionAsset actions;

    public void OnEnable()
    {
        string rebinds = PlayerPrefs.GetString(REBINDSKEY);

        if (string.IsNullOrEmpty(rebinds)) return;

        actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void OnDisable()
    {
        string rebinds = actions.SaveBindingOverridesAsJson();
        
        PlayerPrefs.SetString(REBINDSKEY, rebinds);
    }
}
