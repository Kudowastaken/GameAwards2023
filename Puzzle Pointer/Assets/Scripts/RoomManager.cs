using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public GameObject playerResetPoint;
    public Vector2 playerResetMoveInput;
    public Vector3 playerResetRotation;
    public float playerResetAngle;
    private IReset[] resetables;

    private void Start()
    {
        resetables = GetComponentsInChildren<IReset>();
    }

    private void Update()
    {
        ResetRoom();
    }

    private void ResetRoom()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var item in resetables)
            {
                item.Reset();
            }
        }
    }
}
