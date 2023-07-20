using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class LoadDll : MonoBehaviour
{
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        LoadMetadataForAOTAssemblies();
        LoadHotFixDll();
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private void LoadMetadataForAOTAssemblies()
    {
        List<string> aotMetaAssemblyFiles = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
        };
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 

        for (int i = 0; i < aotMetaAssemblyFiles.Count; i++)
        {
            if (i < aotMetaAssemblyFiles.Count)
            {
                StartCoroutine(LoadAssembly(Application.streamingAssetsPath + "/" + aotMetaAssemblyFiles[i] + ".bytes", "aot"));
            }
        }
    }

    /// <summary>
    /// 加载热更dll
    /// </summary>
    private void LoadHotFixDll()
    {
        string filePath = Application.streamingAssetsPath + "/" + "HotUpdate.dll.bytes";
        StartCoroutine(LoadAssembly(filePath, "dll"));
    }


    IEnumerator LoadAssembly(string filePath, string type)
    {
        UnityWebRequest unityWebRequest;
        unityWebRequest = UnityWebRequest.Get(filePath);

        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                switch (type)
                {
                    case "dll":
                        Assembly asm = Assembly.Load(unityWebRequest.downloadHandler.data);
                        Type entryType = asm.GetType("Entry");
                        entryType.GetMethod("Start").Invoke(null, null);
                        break;

                    case "aot":
                        HomologousImageMode mode = HomologousImageMode.SuperSet;
                        byte[] dllBytes = unityWebRequest.downloadHandler.data;
                        // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                        LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                        Debug.Log($"LoadMetadataForAOTAssembly:{filePath.Substring(filePath.LastIndexOf("/"))}. mode:{mode} ret:{err}");
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
}
