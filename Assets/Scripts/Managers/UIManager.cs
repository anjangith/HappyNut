using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] allPages;

        private bool isLandscape = true;

        public void Start()
        {
            GameManager.GameManagerInst.UiManager = this;
        }

        private void HidePages(bool isActive = false, Action action = null)
        {
            foreach (var page in allPages)
            {
                page.SetActive(isActive);
                action?.Invoke();
            }
        }

        /// <summary>
        /// Find a page in the list allPages and set it active.
        /// </summary>
        /// <param name="objectName"></param>
        public void ShowPage(string objectName)
        {
            StartCoroutine(AudioManager.audioManager.EButtonPressed(() =>
            {
                HidePages();
                GameObject page;
                try
                {
                    page = allPages.ToList().First(objectGame => objectGame.name.Contains(objectName) ||
                                                                 objectGame.name.Contains(objectName.ToLower()));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    Debug.LogError(objectName + " was not found!");
                    return;
                }
                page.SetActive(true);

                if (isLandscape)
                {
                    var landscape = page.transform.Find("Landscape");
                    if (!landscape)
                    {
                        Debug.LogError(objectName + "'s landscape child was not found.");
                        return;
                    }
                    landscape.gameObject.SetActive(true);
                }
                else
                {
                    var portrait = page.transform.Find("Portrait");
                    if (!portrait)
                    {
                        Debug.LogError(objectName + "'s portrait child was not found.");
                        return;
                    }
                    portrait.gameObject.SetActive(true);
                }
            }));
        }
        
    }
}
