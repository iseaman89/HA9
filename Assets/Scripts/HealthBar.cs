using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private bool rotateBar = true;
    [SerializeField] private Destructable owner;

    private void Start()
    {
        healthBar = gameObject.GetComponent<Image>();
        if (owner.gameObject.GetComponent<CharacterController>() != null)
        {
            rotateBar = false;
        }
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.InverseLerp(0.0f, owner.HitPoints, owner.HitPointsCurrent);
        if (rotateBar)
        {
            transform.forward = Camera.main.transform.position - transform.position;
        }
    }
}
