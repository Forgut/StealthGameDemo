using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerCrouchAndJump : MonoBehaviour
    {
        public CharacterController Controller;

        public Transform GroundCheck;
        public LayerMask GroundMask;
        private float _groundDistance = 0.4f;

        private Vector3 _velocity;
        private float _gravityConst = -9.81f;
        private float _jumpHeight = 0.0005f;

        private bool _isGrounded;

        private bool _crouching;
        private float _originalHeight;
        private float _crouchHeight;

        public void Start()
        {
            _originalHeight = Controller.height;
            _crouchHeight = _originalHeight * 0.3f;
            _crouching = false;
            _isGrounded = Physics.CheckSphere(GroundCheck.position, _groundDistance, GroundMask);
        }
        public void Update() 
        {
            UpdateCrouch();
            UpdateJump();
            UpdateFall();
        }

        private void UpdateJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _crouching = false;
                UpdateCrouchHeight();

                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityConst);
            }
        }

        private void UpdateFall()
        {
            _isGrounded = Physics.CheckSphere(GroundCheck.position, _groundDistance, GroundMask);
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = -1f;

            _velocity.y += _gravityConst * Time.deltaTime * Time.deltaTime;
            Controller.Move(_velocity);
        }

        private void UpdateCrouch()
        {
            if (Input.GetKeyDown(KeyCode.C) && _isGrounded)
            {
                _crouching = !_crouching;
                UpdateCrouchHeight();
            }
        }

        private void UpdateCrouchHeight()
        {
            Controller.height = _crouching ? _crouchHeight : _originalHeight;
        }
    }
}
