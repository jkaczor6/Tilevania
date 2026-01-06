using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            FindAnyObjectByType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            FindAnyObjectByType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(0);
        }
    }
}
