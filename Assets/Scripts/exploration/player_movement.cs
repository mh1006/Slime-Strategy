using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_movement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private PlayerActions _playerActions;
    private Rigidbody _rbody;
    private Vector3 _moveInput;
    Player_inventory player_Inventory;

    void Awake()
    {
        player_Inventory = GameObject.Find("inventory").GetComponent<Player_inventory>();
        _playerActions = new PlayerActions();
        _rbody = GetComponent<Rigidbody>();
        if (player_Inventory.LevelOne == false)
            transform.position = player_Inventory.Current_Position;
        else
            transform.position = new Vector3(0f, 0.5f, 0f);
        //if (_rbody is null)
        //    Debug.LogError("Rigidbody2D is NULL!");

    }

    //private void Start()
    //{
       
    //}


    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }
    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }
    void FixedUpdate()
    {
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector3>();
        //_moveInput.y = 0f;
        //_moveInput.z = 0f;
        _rbody.velocity = _moveInput * _speed;
        player_Inventory.CurrentPositions(transform.position);
    }
}