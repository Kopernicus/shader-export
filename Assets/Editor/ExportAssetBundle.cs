using UnityEditor;
using UnityEngine;
using System.IO;

public class ExportSelected {

    private const string ext = "unity3d";

    static readonly string[] destinations = { "Windows", "Linux", "MacOSX" };

    [MenuItem("Export/Export Selected Assets As Bundle")]
    private static void BuildBundle()
    {
        string destination = EditorUtility.SaveFilePanel("Export Bundle", string.Empty, "New Asset Bundle", ext);
        string fileName;
        Split(ref destination, out fileName);

        // As of this version of unity, there is no need for 32 bit exporting.
        BuildTarget[] targets = { BuildTarget.StandaloneWindows64, BuildTarget.StandaloneLinux64, BuildTarget.StandaloneOSXIntel64 };
        string[] extensions = { fileName + "-windows", fileName + "-linux", fileName + "-macosx" };

        // Get selected assets
        var assets = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);

        // Build the plan.
        AssetBundleBuild[] builds = { BuildPlan(assets) };
        
        // Build all bundles
        for (int i = 0; i < 3; i++)
        {
            // Assign the proper bundle name
            builds[0].assetBundleName = extensions[i];

            // Create finalized filepath and create folders if necessary (because the operation fails otherwise)
            string dest = Path.Combine(destination, destinations[i]);
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            // Build bundle for this platform.
            BuildPipeline.BuildAssetBundles(dest, builds, BuildAssetBundleOptions.None, targets[i]);
        }

        AssetDatabase.Refresh();
    }

    static readonly char[] separator = { '/' };
    /// <summary>
    /// Preprocesses a full file path for later use by the plan builder.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    static void Split(ref string path, out string filename)
    {
        string[] parts = path.Split(separator);
        filename = parts[parts.Length - 1];
        path = path.Replace(filename, string.Empty);
        filename = filename.Replace(".unity3d", string.Empty);
    }

    /// <summary>
    /// Generates an AssetBundleBuild plan from the given array of assets.
    /// </summary>
    /// <param name="assets">The assets to be included in this bundle</param>
    /// <returns></returns>
    public static AssetBundleBuild BuildPlan(Object[] assets)
    {
        AssetBundleBuild build = new AssetBundleBuild()
        {
            addressableNames = new string[assets.Length],
            assetNames = new string[assets.Length],
            assetBundleVariant = ext
        };

        for (int i = 0; i < assets.Length; i++)
        {
            build.addressableNames[i] = assets[i].name;
            build.assetNames[i] = AssetDatabase.GetAssetPath(assets[i]);
        }

        return build;
    }
}
