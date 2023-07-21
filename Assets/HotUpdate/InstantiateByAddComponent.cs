using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InstantiateByAddComponent : MonoBehaviour
{
    string url = "https://test01-1253238815.cos.ap-guangzhou.myqcloud.com/HotUpdate/";
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
        StartCoroutine(StartLoadAssemblyAsset(url + "scenes"));
    }

    IEnumerator StartLoadAssemblyAsset(string filePath)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(filePath);

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(unityWebRequest);//获取ab包

                if (ab != null)
                    SceneManager.LoadScene("SampleSelector", LoadSceneMode.Single);
                SceneManager.sceneLoaded += CallBack;

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
        Debug.Log(scene.name + "is load complete!，热更新场景加载成功！");
    }
}
