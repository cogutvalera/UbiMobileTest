using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ubisoft.Racing.Test
{
    public class LevelPause : MonoBehaviour, IPointerClickHandler
    {
        public Sprite m_pauseSprite, m_resumeSprite;
        public Image m_image;

        public void OnEnable()
        {
            Time.timeScale = 1.0f;
        }

        public void OnPointerClick(PointerEventData data)
        {
            Time.timeScale = 1.0f - Time.timeScale;

            StateUpdate();
        }

        private void StateUpdate()
        {
            if (Time.timeScale < 1.0f)
            {
                if (0.0f != Time.timeScale)
                    Time.timeScale = 0.0f;

                m_image.overrideSprite = m_resumeSprite;

                CarAudio.Instance.StopSound();
                CarController.Instance.StopWheelsAudio();
            }
            else
            {
                if (1.0f != Time.timeScale)
                    Time.timeScale = 1.0f;

                m_image.overrideSprite = m_pauseSprite;
            }
        }
    }
}
