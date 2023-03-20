using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RoomCacher : MonoBehaviour
{
    [SerializeField] RoomManager currentRoom;
    [SerializeField] RoomManager previousRoom;

    private PlayerMovement _playerMovement;
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            previousRoom = currentRoom;
            currentRoom = other.GetComponent<RoomManager>();
            _playerMovement.resetPoint = currentRoom.playerResetPoint.transform.position;
            _playerMovement.resetMoveInput = currentRoom.playerResetMoveInput;
            _playerMovement.resetRotation = currentRoom.playerResetRotation;
            _playerMovement.resetAngle = currentRoom.playerResetAngle;
            if (previousRoom == null) return;
            
            previousRoom.StopAllCoroutines();
            _playerMovement.isMovingRooms = true;
            StartCoroutine(_playerMovement.PlayerRoomTransition());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Room") && other.GetComponent<RoomManager>() == previousRoom)
        {
            _playerMovement.isMovingRooms = false;
            StopCoroutine(_playerMovement.PlayerRoomTransition());
        }
    }
}
