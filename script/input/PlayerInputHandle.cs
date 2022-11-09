using UnityEngine;

namespace script.input
{
    [DefaultExecutionOrder(-10)]
    public class PlayerInputHandle:Singleton<PlayerInputHandle>
    {
        public TouchControl m_playerTouchControl;

        public delegate void PointPressEvent(bool isDrag);
        public event PointPressEvent OnPointPress;
        

        public override void Awake()
        {
            base.Awake();
            m_playerTouchControl = new TouchControl();
         
        }

        private void OnEnable()
        {
            m_playerTouchControl?.Enable();
        }

        private void OnDisable()
        {
            m_playerTouchControl?.Disable();
        }

    

        private void Start()
        {
            var inputAction = m_playerTouchControl.Touch.TestAction;
            
            inputAction.started += _ =>
            {
                OnPointPress?.Invoke(true);//notify drag is true
            };
            inputAction.canceled += _ =>
            {
                OnPointPress?.Invoke(false);
            };
            
           
            
        }

       
        

       
    }
}