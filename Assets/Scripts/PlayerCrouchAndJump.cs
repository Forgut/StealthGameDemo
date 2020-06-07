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

        private float _speed;
        private float _normallSpeed = 3f;
        private float _crouchSpeed = 1f;

        public Transform LedgeCheckUp;
        public Transform LedgeCheckDown;
        private float _ledgeGrabDistance = 0.3f;
        public bool CanClimb;
        public bool LedgeDownCollision;
        public bool LedgeUpCollision;
        public bool MovedPastLedge;

        public Transform GroundCheck;
        public LayerMask GroundMask;
        private float _groundDistance = 0.45f;

        private Vector3 _velocity;
        private float _gravityConst = -9.81f;
        private float _jumpHeight = 0.0005f;
        private float _ledgeClimbHeight = 0.0003f;

        public bool _isGrounded;

        public bool _crouching;
        private float _originalHeight;
        private float _crouchHeight;

        public void Start()
        {
            _speed = _normallSpeed;
            _originalHeight = Controller.height;
            _crouchHeight = _originalHeight * 0.3f;
            _crouching = false;
            _isGrounded = Physics.CheckSphere(GroundCheck.position, _groundDistance, GroundMask);
        }
        public void Update() 
        {
            UpdateMovement();
            UpdateCrouch();
            UpdateLedgeClimb();
            UpdateJump();
            UpdateFall();
        }

        private void UpdateJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !MovedPastLedge)
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
            if (_crouching)
            {
                Controller.height = _crouchHeight;
                _speed = _crouchSpeed;
            }
            else
            {
                Controller.height = _originalHeight;
                _speed = _normallSpeed;
            }
        }

        private void UpdateLedgeClimb()
        {
            bool spaceAbove = !Physics.CheckSphere(LedgeCheckUp.position, _ledgeGrabDistance, GroundMask);
            bool ledgeBelow = Physics.CheckSphere(LedgeCheckDown.position, _ledgeGrabDistance, GroundMask);
            LedgeUpCollision = !spaceAbove;
            LedgeDownCollision = ledgeBelow;
            CanClimb = spaceAbove && ledgeBelow;
            if (MovedPastLedge)
            {
                MoveForwardOverTheLedge();   
                if (_isGrounded)
                {
                    _velocity.y = -1f;
                    MovedPastLedge = false;
                }
            }
            if (Input.GetKey(KeyCode.Space) && spaceAbove && ledgeBelow && !MovedPastLedge)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityConst);
                MovedPastLedge = true;
            }
            void MoveForwardOverTheLedge()
            {
                Controller.Move(transform.forward * 0.1f * _speed * Time.deltaTime);
            }
        }

        private void UpdateMovement()
        {
            if (MovedPastLedge)
                return;
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            var direction = transform.right * x + transform.forward * z;
            Controller.Move(direction * _speed * Time.deltaTime);
        }
    }
}
