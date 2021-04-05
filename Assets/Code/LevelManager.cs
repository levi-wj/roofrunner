using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float autoLoadLevelTime = 0f;
    [SerializeField] string autoLoadLevel = null;

    void Start()
    {
        if (autoLoadLevelTime > 0) {
            StartCoroutine("loadLevelWithWait");
        }
    }

    private IEnumerator loadLevelWithWait()
    {
        yield return new WaitForSeconds(autoLoadLevelTime);
        LoadLevel(autoLoadLevel);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
