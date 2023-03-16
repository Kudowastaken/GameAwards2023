using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlock : MonoBehaviour
{
    private BoxCollider2D myBoxCollider;
    public bool ShouldDropBlock { get; private set; }
    [SerializeField] private float cartResetTime;
    private float cartResetTimeAtStart;
    private void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        cartResetTimeAtStart = cartResetTime;
    }

    private void Update()
    {
        if (cartResetTime > 0)
        {
            cartResetTime -= Time.deltaTime;
        }
        else
        {
            ShouldDropBlock = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShouldDropBlock = true;
            cartResetTime = cartResetTimeAtStart;
        }
    }
}
