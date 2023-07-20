using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InstantiateByAddComponent : MonoBehaviour
{
    /// <summary>
    /// 加载prefab和场景
    /// </summary>
    void Start()
    {
        Debug.Log($"[InstantiateByAddComponent] 这个脚本通过AddComponent的方式实例化.");
        Run_InstantiateComponentByAssetBundle();
    }

    /// <summary>
    /// 从AB加载prefab和场景
    /// </summary>
    void Run_InstantiateComponentByAssetBundle()
    {
        StartCoroutine(StartLoadAssemblyAsset(Application.streamingAssetsPath + "/" + "prefabs", "prefab"));
        StartCoroutine(StartLoadAssemblyAsset(Application.streamingAssetsPath + "/" + "scenes", "scene"));
    }

    IEnumerator StartLoadAssemblyAsset(string filePath, string type)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(filePath);

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(unityWebRequest);//获取ab包
                switch (type)
                {
                    case "prefab":
                        GameObject obj = ab.LoadAsset<GameObject>("Cube");//加载Asset
                        GameObject.Instantiate(obj);//实例化 
                        break;

                    case "scene":
                        if (ab != null)
                            SceneManager.LoadScene("SampleSelector", LoadSceneMode.Single);
                        SceneManager.sceneLoaded += CallBack;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Debug.LogFormat($"filePath:{filePath} load error:{unityWebRequest.error}");
            }
        }
        unityWebRequest.Dispose();
    }

    void CallBack(Scene scene, LoadSceneMode sceneType)
    {
        Debug.Log(scene.name + "is load complete!，55555555555555555！");
    }
}
