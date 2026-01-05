using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(collision.CompareTag("Player"))
        {
            if(sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(sceneIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
