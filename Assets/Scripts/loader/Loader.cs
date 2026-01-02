using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System
    ;




public static class Loader
{
    private class LoadingMonoBehaviour : MonoBehaviour {}
    public enum Scene
    {
        Game,MapEditor,Menu,Loading
    }

    private static Action onLoaderCallback; //delegate that return void

    private static AsyncOperation loadingAsyncOperaion;

    public static void Load(Scene scene)
    {       
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));  
        };
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {

        yield return null;
        loadingAsyncOperaion = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperaion.isDone) {
            yield return null;
        
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperaion != null)
        {
            return loadingAsyncOperaion.progress;
        }
        else
        {
            return 1f;
        }
    }


    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
