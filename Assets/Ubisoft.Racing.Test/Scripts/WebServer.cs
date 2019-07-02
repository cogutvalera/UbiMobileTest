using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace Ubisoft.Racing.Test
{
    public class WebServer : MonoBehaviour
    {
        public string addScoreURL = "http://localhost/addscore.php";
        public string highscoreURL = "http://localhost/display.php";
        public string secret = "mySecretKey";

        public class PlayerScore
        {
            public string name;
            public int score;
        }

        private static WebServer m_instance;
        public static WebServer Instance
        {
            get
            {
                return m_instance;
            }
        }

        private void Awake()
        {
            m_instance = this;
        }

        public IEnumerator SendScore(string userName, int userScore)
        {
            string hash = Sha256(userName + userScore + secret);
            string url = addScoreURL + 
                "?name=" + userName + "&score=" + userScore + "&hash=" + hash + "&level_name=" + SceneManager.GetActiveScene().name;
            Debug.Log(url);

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
            }
        }

        public IEnumerator GetScore(System.Action<List<PlayerScore>> callback)
        {
            string url = highscoreURL + "?level_name=" + SceneManager.GetActiveScene().name;
            Debug.Log(url);

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    List<PlayerScore> history = JsonConvert.DeserializeObject<List<PlayerScore>>(www.downloadHandler.text);
                    if (null != callback)
                        callback.Invoke(history);
                }
            }
        }

        static string Sha256(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
