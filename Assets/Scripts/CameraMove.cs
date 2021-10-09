using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _positionToPlayer;

    private void Start()
    {
        _positionToPlayer = transform.position - _player.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, _positionToPlayer.z + _player.position.z);
        transform.position = newPosition;
    }
}
