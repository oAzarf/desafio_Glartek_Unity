using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToScene : MonoBehaviour
{
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadInProgress(sceneIndex));
    }


    IEnumerator LoadInProgress(int sceneIndex)
    {
        Debug.Log("loading scene: "+ sceneIndex);
        var operation = SceneManager.LoadSceneAsync(sceneIndex);
        //operation.allowSceneActivation = false;

        while (!operation.isDone)
        {

            
            yield return null;
        }

        operation.allowSceneActivation = true;


    }


    public void ExitScene()
    {
        if (SceneManager.GetActiveScene().name== SceneManager.GetSceneByBuildIndex(0).name)
        {
            Application.Quit();
            return;
        }
        StartCoroutine(LoadInProgress(0));

    }
}
