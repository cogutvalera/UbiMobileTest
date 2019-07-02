using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Ubisoft.Racing.Test
{
    public class LevelReset : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData data)
        {
            // reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
