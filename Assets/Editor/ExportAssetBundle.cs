using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build Shader Bundles")]
	static void BuildAllAssetBundles ()
	{
		// Bring up save panel
		string path = EditorUtility.SaveFilePanel("Save Shader to Asset Bundles", "", "kopernicusshaders", "unity3d");

		if (path != string.Empty) {
			const BuildAssetBundleOptions opts = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.CollectDependencies;
			BuildTarget[] platforms = { BuildTarget.StandaloneWindows, BuildTarget.StandaloneOSXUniversal, BuildTarget.StandaloneLinux };
			string[] platformExts = { "-windows", "-macosx", "-linux" };

			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
			var activeObj = Selection.activeObject;
			for (var i = 0; i < platforms.Length; ++i)
			{
				// Build the resource file from the active selection.
				BuildPipeline.BuildAssetBundle(activeObj, selection, path.Replace(".unity3d", platformExts[i] + ".unity3d"), opts, platforms[i]);
			}
		}
	}
}
