using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _193064
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator StartGame(string levelName)
        {
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene(levelName);
        }

        public void OnLevel1ButtonPressed()
        {
            StartCoroutine(StartGame("193064-Level1"));
        }

        public void OnLevel2ButtonPressed()
        {
            StartCoroutine(StartGame("193064-Level2"));
        }


        public void OnExitToDekstopButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}