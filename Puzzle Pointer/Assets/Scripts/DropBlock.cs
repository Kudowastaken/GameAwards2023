using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlock : MonoBehaviour, IReset
{
    public void Reset()
    {
        ShouldDropBlock = true;
        cartResetTime = cartResetTimeAtStart;
    }

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
        dropOnReset();
        if (cartResetTime > 0)
        {
            cartResetTime -= Time.deltaTime;
        }
        else
        {
            ShouldDropBlock = false;
        }
    }

    private void dropOnReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
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
