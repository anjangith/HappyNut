﻿using Managers;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles the clicking of buttons in menus.
    /// </summary>
    public class ButtonMethods : MonoBehaviour {

        /// <summary>
        /// The Play button was pressed. Open Level 1.
        /// </summary>
        public void PlayPressed()
        {
            GameManager.GameManagerInst.StartGame();
        }

        /// <summary>
        /// Quit Button pressed close the application.
        /// </summary>
        public void QuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// The Exit button was pressed. Open Main Menu.
        /// </summary>
        public void ExitToMainMenu()
        {
            GameManager.GameManagerInst.ShowStartScreen();
        }

    }
}
