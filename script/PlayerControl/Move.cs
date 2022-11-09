using System;
using System.Collections;
using script.Buffer;
using script.input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace script.PlayerControl
{
    public class Move:MonoBehaviour
    {

        private Camera _cameraMain;

        private Camera cameraMain
        {
            get
            {
                if (_cameraMain == null)
                {
                    _cameraMain=Camera.main;
                }

                return _cameraMain;
            }
        }
        // private TouchControl _playerTouchControl;

        [SerializeField]
        public BufferValue speed;
        private Transform _transform;

        private bool _isTouched;
        private void Start()
        {
            
            _transform = transform;
            // action = PlayerInputHandle.Instance.m_playerTouchControl.Touch.TouchPress;

        }

       

        

     


        public void ListenPressAction(InputAction action)
        {
            action.started += _ =>
            {
                _isTouched = true;
            };
            action.canceled += _ =>
            {
                _isTouched = false;
            };
        }

        private void Update()
        {
            if (_isTouched)
            {
                
                var screenPos = Pointer.current.position.ReadValue();
                var screenCoord = new Vector3(screenPos.x, screenPos.y,cameraMain.nearClipPlane);  //x,y,depth
                var worldCoord = cameraMain.ScreenToWorldPoint(screenCoord);
            
                
                worldCoord.y = transform.position.y;// same height as player (this is a plane game)
            
                // ScreenIndicate.Instance.IndicateWorldPos(worldCoord);

                var pos= Vector3.MoveTowards(_transform.position, worldCoord, speed.GetResultValF() * Time.deltaTime);
                // Debug.Log("pos: "+pos);
                _transform.position = pos;

            }
        }
        
        
        
    }
}