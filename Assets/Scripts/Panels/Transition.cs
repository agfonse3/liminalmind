using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
