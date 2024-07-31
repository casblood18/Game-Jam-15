using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private Canvas canvas;
    private Image _healthBarImage;
    private float _maxHealth;

    private void Awake()
    {
        canvas = this.transform.parent.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        _healthBarImage = GetComponent<Image>();
    }

    public void InitHealthBar(float maxHealth)
    {
        _maxHealth = maxHealth;
        _healthBarImage.fillAmount = 1;
    }

    public void UpdateHealthBar(float currHealth)
    {
        _healthBarImage.fillAmount = currHealth/_maxHealth;
    }
}
