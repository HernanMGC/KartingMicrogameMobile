using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KartGame.Track
{
    /// <summary>
    /// A class to display information about a particular racer's timings.  WARNING: This class uses strings and creates a small amount of garbage each frame.
    /// </summary>
    public class CoinDisplay : MonoBehaviour
    {
        /// <summary>
        /// The different information that can be displayed on screen.
        /// </summary>
        public enum DisplayOptions
        {
            /// <summary>
            /// Displays ammount of coins picked since the session started.
            /// </summary>
            SessionCoins,
            /// <summary>
            /// Displays best ammount of coins picked in a race ever.
            /// </summary>
            HistoricalCoins,
            /// <summary>
            /// Displays total ammount of coins picked in a race ever.
            /// </summary>
            HistoricalTotalCoins
        }


        [Tooltip("The timings to be displayed and the order to display them.")]
        public List<DisplayOptions> initialDisplayOptions = new List<DisplayOptions>();
        [Tooltip("A reference to the track manager.")]
        public TrackManager trackManager;
        [Tooltip("A reference to the TextMeshProUGUI to display the information.")]
        public TextMeshProUGUI textComponent;
        [Tooltip("A reference to the racer to display the information for.")]
        [RequireInterface(typeof(IRacer))]
        public Object initialRacer;

        List<Action> m_DisplayCalls = new List<Action>();
        IRacer m_Racer;
        StringBuilder m_StringBuilder = new StringBuilder(0, 300);

        void Awake()
        {
            for (int i = 0; i < initialDisplayOptions.Count; i++)
            {
                switch (initialDisplayOptions[i])
                {
                    case DisplayOptions.SessionCoins:
                        m_DisplayCalls.Add(DisplaySessionCoins);
                        break;
                    case DisplayOptions.HistoricalCoins:
                        m_DisplayCalls.Add(DisplayHistoricalCoins);
                        break;
                    case DisplayOptions.HistoricalTotalCoins:
                        m_DisplayCalls.Add(DisplayHistoricalTotalCoins);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (initialRacer)
            {
                m_Racer = (IRacer)initialRacer;
            }
        }

        void Update()
        {
            m_StringBuilder.Clear();

            for (int i = 0; i < m_DisplayCalls.Count; i++)
            {
                m_DisplayCalls[i].Invoke();
            }

            textComponent.text = m_StringBuilder.ToString();
        }

        void DisplaySessionCoins()
        {
            float sessionCoins = m_Racer.GetCoins();

            m_StringBuilder.AppendLine($"Coins: {sessionCoins}");
        }

        void DisplayHistoricalCoins()
        {
            int historicalCoins = trackManager.HistoricalCoins;
            if (historicalCoins <= 0)
                return;

            historicalCoins = trackManager.HistoricalCoins;
               
            m_StringBuilder.AppendLine($"Best coin score: {historicalCoins}");
        }

        void DisplayHistoricalTotalCoins()
        {
            int historicalTotalCoins = trackManager.HistoricalTotalCoins;
            if (historicalTotalCoins <= 0)
                return;

            historicalTotalCoins = trackManager.HistoricalTotalCoins;

            m_StringBuilder.AppendLine($"Total coins: {historicalTotalCoins}");
        }

        /// <summary>
        /// Call this function to change what information is currently being displayed.  This causes a GCAlloc.
        /// </summary>
        /// <param name="newDisplay">A collection of the new options for the display.</param>
        /// <exception cref="ArgumentOutOfRangeException">One or more of the display options is not valid.</exception>
        public void RebindDisplayOptions(List<DisplayOptions> newDisplay)
        {
            m_DisplayCalls.Clear();

            for (int i = 0; i < newDisplay.Count; i++)
            {
                switch (newDisplay[i])
                {
                    case DisplayOptions.SessionCoins:
                        m_DisplayCalls.Add(DisplaySessionCoins);
                        break;
                    case DisplayOptions.HistoricalCoins:
                        m_DisplayCalls.Add(DisplayHistoricalCoins);
                        break;
                    case DisplayOptions.HistoricalTotalCoins:
                        m_DisplayCalls.Add(DisplayHistoricalTotalCoins);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Call this function to change the racer about which the information is being displayed.
        /// </summary>
        public void RebindRacer(IRacer newRacer)
        {
            m_Racer = newRacer;
        }
    }
}