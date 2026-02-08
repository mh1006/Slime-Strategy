using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_move : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private PlayerActions _playerActions;
    private Rigidbody _rbody;
    private Vector3 _moveInput;
    Player_inventory player_Inventory;
    GameObject gameObject;

    private bool _disableMovement;

    void Awake()
    {
        gameObject = GameObject.Find("inventory");
        if (gameObject != null)
            player_Inventory = gameObject.GetComponent<Player_inventory>();
        _playerActions = new PlayerActions();
        _rbody = GetComponent<Rigidbody>();
        
        //if (_rbody is null)
        //    Debug.LogError("Rigidbody2D is NULL!");

    }

    private void Start()
    {
        _speed = 10;
        // if (player_Inventory != null)
            // if (player_Inventory.LevelOne == false)
                // transform.position = player_Inventory.Current_Position;
            // else
                transform.position = new Vector3(0f, 0.5f, 0f);
    }


    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }
    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }

    public void DisableMovement()
    {
        _disableMovement = true;
    }

    public void EnableMovement()
    {
        _disableMovement = false;
    }

    private void FixedUpdate()
    {
        if (_disableMovement)
        {
            _rbody.velocity = Vector3.zero;
            return;
        }
        
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector3>();
        //_moveInput.y = 0f;
        //_moveInput.z = 0f;
        _rbody.velocity = _moveInput * _speed;
        if (player_Inventory != null)
            player_Inventory.CurrentPositions(transform.position);
    }
}
