using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonIcon : MonoBehaviour
{
    [SerializeField] private Texture2D mouseIcon;


    public void SetMouseIcon()
    {
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.Auto);
    }

    public void ResetMouseIcon()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
