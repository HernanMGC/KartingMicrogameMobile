using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private GameObject gameManager;
    StringBuilder m_StringBuilder = new StringBuilder(0, 300);

    public TMPro.TextMeshProUGUI bestLapTMP;

    private void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameController");

        if (bestLapTMP != null)
        {
            
            if (PlayerPrefs.HasKey("BestTime"))
            {
                
                m_StringBuilder.Append("Best Lap Ever (PlayerPref): ");
                m_StringBuilder.Append(PlayerPrefs.GetFloat("BestTime").ToString(".##"));
                m_StringBuilder.Append('\n');

                this.bestLapTMP.text = m_StringBuilder.ToString();

            } else {
                this.bestLapTMP.text = "";
            }
        }
    }

    public void PlayGame()
    {
        this.gameManager.GetComponent<GameManager>().StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
