using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _interactionBox;
    [SerializeField] private TextMeshProUGUI interactionText;
    protected bool inRadius = false;
    protected GameObject InteractionBox;

    public virtual void Interact()
    {
        if (!inRadius) return;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        inRadius = true;
        _interactionBox.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadius = false;
            _interactionBox.SetActive(false);
        }
        else return;
    }
}
