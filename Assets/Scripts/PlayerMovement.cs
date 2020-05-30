using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;
    public float Speed = 12f;
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    public bool IsGrounded;

    private Vector3 _velocity;
    private float _gravityConst = -9.81f;
    private float _jumpHeight = 0.001f;
    void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (IsGrounded && _velocity.y < 0)
            _velocity.y = -1f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;

        CharacterController.Move(direction * Speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityConst);

        _velocity.y += _gravityConst * Time.deltaTime * Time.deltaTime;
        CharacterController.Move(_velocity); 
    } 
}
