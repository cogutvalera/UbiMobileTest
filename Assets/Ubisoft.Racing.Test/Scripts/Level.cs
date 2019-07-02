using UnityEngine;
using System.Collections.Generic;

namespace Ubisoft.Racing.Test
{
    public class Level : MonoBehaviour
    {
        public int m_totalLaps = 3;

        [System.Serializable]
        public class Rating
        {
            public int seconds;
            public int stars;
        }
        public List<Rating> m_ratings;

        public List<Checkpoint> m_checkpoints;

        private Checkpoint m_prevCheckpoint;
        public Checkpoint PrevCheckpoint
        {
            get
            {
                return m_prevCheckpoint;
            }

            set
            {
                m_prevCheckpoint = value;
            }
        }

        private static Level m_instance;
        public static Level Instance
        {
            get
            {
                return m_instance;
            }
        }

        private int m_lapsCounter;
        public int LapsCounter
        {
            get
            {
                return m_lapsCounter;
            }

            set
            {
                m_lapsCounter = value;
            }
        }

        private void Awake()
        {
            m_instance = this;

            m_lapsCounter = 0;
        }

        public bool IsLevelFinished()
        {
            return m_totalLaps == m_lapsCounter;
        }

        public void RatingsFeedback()
        {
            if (false == IsLevelFinished())
                return;

            StartCoroutine(WebServer.Instance.SendScore(SystemInfo.deviceName, (int)HUD.Instance.TimeDuration));

            HUD.Instance.Invoke("GetHistory", 1.5f);

            HUD.Instance.m_feedbackLabel.text = "0 Stars";
            HUD.Instance.ActivateFeedback(true);

            for (int i = 0; i < m_ratings.Count; i++)
            {
                Rating r = m_ratings[i];

                if (HUD.Instance.TimeDuration < r.seconds * m_totalLaps)
                {
                    HUD.Instance.m_feedbackLabel.text = r.stars.ToString() + " Star";
                    if (1 != r.stars)
                        HUD.Instance.m_feedbackLabel.text += "s";

                    return;
                }
            }
        }
    }
}
