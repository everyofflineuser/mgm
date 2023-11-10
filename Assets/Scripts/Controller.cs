using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Avatar = Alteruna.Avatar;

[RequireComponent(typeof(InputSynchronizable), typeof(Avatar), typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90f;
 
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    [HideInInspector]
    public bool canMove = true;
 
    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;
 
    private Avatar _avatar;
    private SyncedAxis _horizontal;
    private SyncedAxis _vertical;
		
    private SyncedKey _jump;
    private SyncedKey _sprint;
		
    private InputSynchronizable _input;
		
    private float MouseX => Input.GetAxisRaw("Mouse X");
    private float MouseY => Input.GetAxisRaw("Mouse Y");
		
    private void InitializeInput()
    {
        if (_avatar.IsOwner)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
			
        _input = GetComponent<InputSynchronizable>();
			
        _horizontal = new SyncedAxis(_input, "Horizontal");
        _vertical = new SyncedAxis(_input, "Vertical");
			
        _jump = new SyncedKey(_input, KeyCode.Space, SyncedKey.KeyMode.KeyDown); 
        _sprint = new SyncedKey(_input, KeyCode.LeftShift);
    }
 
    void Start()
    {
        _avatar = GetComponent<Avatar>();
        InitializeInput();
 
        if (!_avatar.IsOwner)
            return;
        
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        playerCamera.transform.SetParent(transform);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
        if (!_avatar.IsOwner)
            return;
        
        bool isRunning = false;
 
        // Press Left Shift to run
        isRunning = _sprint;
 
        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * _vertical : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * _horizontal : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        if (_jump && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
 
        // Player and Camera rotation
        if (canMove && playerCamera != null)
        {
            rotationX += -MouseY * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, MouseX * lookSpeed, 0);
        }
    }
}