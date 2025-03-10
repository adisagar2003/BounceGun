using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class DebugUI
    {

        public static void OnGUI(string debugString)
        {
            GUIStyle debugWindowStyle = new GUIStyle();
            debugWindowStyle.normal.textColor = Color.red;
            debugWindowStyle.fontSize = 32;
            GUI.Label(new Rect(10, 70, 200, 200), debugString, debugWindowStyle);
        }

    }
}
