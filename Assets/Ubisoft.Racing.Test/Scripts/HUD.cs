using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Ubisoft.Racing.Test
{
    public class HUD : MonoBehaviour
    {
        public Text m_speedLabel, m_timeDurationLabel, m_lapsLabel, m_feedbackLabel;

        public GameObject m_historyPanel;
        public List<Text> m_historyRowsText;

        public void GetHistory()
        {
            StartCoroutine(WebServer.Instance.GetScore(
                (history) =>
                {
                    if (null == history)
                        return;

                    if (history.Count > 0)
                    {
                        m_historyPanel.gameObject.SetActive(true);

                        for (int i = 0; i < Mathf.Max(m_historyRowsText.Count, history.Count); i++)
                        {
                            if (i < Mathf.Min(m_historyRowsText.Count, history.Count))
                            {
                                m_historyRowsText[i].gameObject.SetActive(true);
                                m_historyRowsText[i].text = (i + 1).ToString() + ". " + history[i].name +
                                    " <size=31><color=green>" + GetTimeFormatted(history[i].score) + "</color></size>";
                            }
                            else
                            {
                                m_historyRowsText[i].gameObject.SetActive(false);
                            }
                        }
                    }
                })
            );

            Invoke("LoadNextScene", 5.0f);
        }

        private void LoadNextScene()
        {
            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        private float m_timeDuration;
        public float TimeDuration
        {
            get
            {
                return m_timeDuration;
            }
        }

        private int m_hours, m_minutes, m_seconds;

        private static HUD m_instance;
        public static HUD Instance
        {
            get
            {
                return m_instance;
            }
        }

        private void Awake()
        {
            m_instance = this;
            m_timeDuration = 0;
            m_feedbackLabel.gameObject.SetActive(false);
        }

        public void FixedUpdate()
        {
            SetSpeedLabel();
            SetLapsLabel();

            if (Level.Instance.IsLevelFinished())
                return;

            m_timeDuration += Time.fixedDeltaTime;
            
            SetTimeLabel();
        }

        private void SetSpeedLabel()
        {
            m_speedLabel.text = CarController.Instance.CurrentSpeedString;
        }

        private string GetTimeFormatted(float time)
        {
            m_hours = (int)(Mathf.Floor(time / 3600)) % 60;
            m_minutes = (int)(Mathf.Floor(time / 60)) % 60;
            m_seconds = (int)(time) % 60;

            return m_hours.ToString("00") + ":" + m_minutes.ToString("00") + ":" + m_seconds.ToString("00");
        }

        private void SetTimeLabel()
        {
            m_timeDurationLabel.text = GetTimeFormatted(m_timeDuration);
        }

        private void SetLapsLabel()
        {
            m_lapsLabel.text = Level.Instance.LapsCounter.ToString() + "/" + Level.Instance.m_totalLaps.ToString() + " Laps";
        }

        public void ActivateFeedback(bool flag, float sec = 0.0f)
        {
            if (sec <= 0.0f)
            {
                m_feedbackLabel.gameObject.SetActive(flag);
                return;
            }

            if (flag)
                Invoke("EnableFeedback", sec);
            else
                Invoke("DisableFeedback", sec);
        }

        private void EnableFeedback()
        {
            m_feedbackLabel.gameObject.SetActive(true);
        }

        private void DisableFeedback()
        {
            m_feedbackLabel.gameObject.SetActive(false);
        }
    }
}
