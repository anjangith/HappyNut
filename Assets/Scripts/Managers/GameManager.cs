using System;
using System.Linq;
using Pawns;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// This Class takes care of all logic for score and ending a game.
    /// Anything that has to do with the game concept.
    /// </summary>
    [RequireComponent(typeof(UIManager))]
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// How many times the player has died. Useful for knowing if we should reset the map.
        /// </summary>
        private int deaths;

        /// <summary>
        /// GameManager keeps track of how many players are in the game.
        /// </summary>
        public bool[] PlayersAlive;

        /// <summary>
        /// For using the class as a singleton.
        /// </summary>
        public static GameManager GameManagerInst;

        /// <summary>
        /// Game points.
        /// </summary>
        public int Score;

        public Text MoneyTextUi;

        private int money = 0;

        /// <summary>
        /// Refrence to UIManager. Is set by UIManagers Awake.
        /// </summary>
        public UIManager UiManager { get; set; }

        /// <summary>
        /// Points to buy something in store.
        /// </summary>
        public int Money {
            get { return money; }
            set {
                money = value;
                MoneyTextUi.text = money.ToString();
            }
        }

        /// <summary>
        /// Initialize as a Singleton.
        /// </summary>
        public void Awake()
        {
            if (GameManagerInst)
            {
                deaths = GameManagerInst.deaths;
                money = GameManagerInst.money;
                Score = GameManagerInst.Score;
                PlayersAlive = GameManagerInst.PlayersAlive;
                Destroy(GameManagerInst);
            }
            GameManagerInst = this;

        }

        /// <summary>
        /// Set every player to be isAlive
        /// <param name="isAlive">Are all players dead or alive</param>
        /// </summary>
        private void SetPlayersAlive(bool isAlive)
        {
            for (var playerIndex = 0; playerIndex < PlayersAlive.Length; ++playerIndex)
            {
                PlayersAlive[playerIndex] = isAlive;
            }
        }

        /// <summary>
        /// Player will tell the GameManager when he died.
        /// GameManager will check if all players are dead before calling GameEnded.
        /// </summary>
        /// <param name="deadPlayer">The players index</param>
        public void PlayerDied(int deadPlayer)
        {
            PlayersAlive[deadPlayer] = false;

            int numPlayersAlive = 0;
            for (var playerIndex = 0; playerIndex < PlayersAlive.Length; ++playerIndex)
            {
                if (PlayersAlive[playerIndex])
                {
                    numPlayersAlive++;
                }
            }
            if (numPlayersAlive < 1)
            {
                GameEnded();
            }
        }

        /// <summary>
        /// Spawns the Game Over Screen and Stops Time.
        /// </summary>
        private void GameEnded()
        {
            deaths++;

            if (UiManager)
            {
                UiManager.ShowPage("GameOver");
            }

            //Pause the game so no movement occurs
            //Time.timeScale = 0;
        }

        /// <summary>
        /// Calls RetryGame.
        /// Set TimeScale back to 1
        /// </summary>
        public void StartGame()
        {
            StartCoroutine(AudioManager.audioManager.EButtonPressed(() =>
            {
                ResetGame();
                SetPlayersAlive(true);

                Time.timeScale = 1;
                SceneManager.LoadScene("StickyBoy");
            }));
        }

        private void ResetGame()
        {

        }

        public event EventHandler OnRetryGame;

        /// <summary>
        /// Set TimeScale back to 1 and Hide GameOverScreen.
        /// Also reset all players.
        /// </summary>
        public void RetryGame()
        {
            StartGame();
            EventHandler handler = OnRetryGame;
            if (handler != null)
            {
                handler(this, null);
            }
        }

        //private void ResetPlayers()
        //{
        //    var players = FindObjectsOfType<Player>().ToList();
        //    players.ForEach(player => player.Reset());
        //}
        public void ShowStartScreen()
        {
            SceneManager.LoadScene("TitleScreen");
        }

        public void LoadScene(string scene)
        {
            Debug.Log("Loading " + scene);
            SceneManager.LoadScene(scene);
        }
    }
}
