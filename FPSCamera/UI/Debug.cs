#if DEBUG
namespace FPSCamera.UI
{
    using CSkyL.UI;
    using System.Collections.Generic;
    using UnityEngine;

    internal class Debug : CSkyL.Game.UnityGUI
    {
        public static Debug Panel {
            get => _panel ?? (_panel = CSkyL.Game.CamController.I.AddComponent<Debug>());
        }

        protected override void _Init() => enabled = true;

        public void AppendMessage(string msg)
        {
            if (_msgCount < msgLimit) ++_msgCount;
            else _message = _message.Substring(_message.IndexOf('\n') + 1);
            _message += msg + "\n";
        }

        public void RegisterAction(string actionName, System.Action action)
        { _nameList.Add(actionName); _actionList.Add(action); }

        protected override void _UnityGUI()
        {
            var boxWidth = Mathf.Min(Helper.ScreenWidth / 5f, 400f);
            var boxHeight = Mathf.Min(Helper.ScreenHeight / 2f, 1000f);

            GUI.color = new Color(0f, 0f, 0f, .9f);
            GUI.Box(new Rect(0f, boxHeight / 2f, boxWidth, boxHeight), "");
            GUI.color = new Color(1f, 1f, 1f, 1f);

            var style = new GUIStyle
            {
                fontSize = 12,
                normal = { textColor = Color.white }
            };

            const float margin = 5f;
            float curY = margin + boxHeight / 2f, curX = margin;
            boxWidth -= 2f * margin;
            const int btnPerLine = 3;
            float btnW = (boxWidth - margin * 2f) / btnPerLine, btnH = boxHeight / 16f;
            for (int i = 0; i < _nameList.Count; ++i) {
                if (GUI.Button(new Rect(curX, curY, btnW, btnH), _nameList[i]))
                    _actionList[i].Invoke();

                if (i % btnPerLine == btnPerLine - 1) { curX = margin; curY += btnH; }
                else curX += btnW;
            }
            if (curX > margin) curY += btnH;

            GUI.Label(new Rect(margin, curY,
                               boxWidth, boxHeight * (1 + 1 / 2f) - curY),
                      _message);
        }

        private const uint msgLimit = 20u;
        private uint _msgCount = 0u;
        private string _message = "";

        private readonly List<string> _nameList = new List<string>();
        private readonly List<System.Action> _actionList = new List<System.Action>();

        private static Debug _panel;
    }
}
#endif