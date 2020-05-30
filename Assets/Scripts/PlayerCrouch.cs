using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerCrouch : MonoBehaviour
    {
        public CharacterController Controller;
        private bool _crouching;
        private float _originalHeight;
        private float _crouchHeight;

        public void Start()
        {
            _originalHeight = Controller.height;
            _crouchHeight = _originalHeight * 0.3f;
            _crouching = false;
        }
        public void Update() { 
            if (Input.GetKeyDown(KeyCode.C))
            {
                _crouching = !_crouching;
                Controller.height = _crouching ? _crouchHeight : _originalHeight;
            }
        }
    }
}
