using UnityEngine;

namespace script
{
    public class LogViewSimple:MonoBehaviour
    {

        public static LogViewSimple m_instance;

        private void Awake()
        {
            m_instance = this;
        }

        private string _logText;
        private int _maxCount=20, _nowCount,_fontSize=7;
        void Start()
        {
            _maxCount = Screen.height / _fontSize;
            Application.logMessageReceived += ShowLogOnView;
        }

        public static void LogOnScreen(object obj)
        {
            if (m_instance == null)
            {
                return;
            }
            m_instance.ShowLogOnView(obj.ToString(),"",LogType.Assert);
        }
        public  void ShowLogOnView(string condition, string stackTrace, LogType type)
        {
            _logText += "\n" + condition+" "+stackTrace;
            _nowCount++;
            if (_nowCount == _maxCount)
            {
                _nowCount = 0;
                _logText = "";
            }
        }

        void OnGUI()
        {
            // Debug.Log("OnGUI "+_logText);
            //文字大小
            GUI.skin.label.fontSize = _fontSize;
            //UI中心对齐
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.Label (new Rect(0,0,Screen.width,Screen.height),_logText);
        }
    }
}
