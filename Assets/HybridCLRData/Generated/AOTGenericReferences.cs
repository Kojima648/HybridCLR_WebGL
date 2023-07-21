using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.List<Entry.MyVec3>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>
	// }}

	public void RefMethods()
	{
		// object[] System.Array.Empty<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
	}
}