//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
using System.IO;
#else
using UnityEditor.PackageManager;
#endif

namespace DREditorInstaller
{
    /// <summary>
    /// The installer class that auto installs the DREditor Registry to let users download DREditor tools through the unity package manager.
    /// </summary>
    [InitializeOnLoad]
    public static class DRERegistryInstaller
    {
        /// <summary>
        /// The display name for the DREditor Registry on the unity package manager.
        /// </summary>
        private static readonly string RegistryName = "DREditor";
#if NEWTONSOFT_JSON
        /// <summary>
        /// The link to the registry DREditor is located at. (Official npm Registry)
        /// </summary>
        private static readonly string URL = "https://registry.npmjs.org";

        /// <summary>
        /// The domain scopes of DREditor Packages.
        /// </summary>
        private static readonly string[] Scopes = new string[]
        {
            "net.dreditor"
        };

        /// <summary>
        /// The initialized serializable scoped registry that the system will add to the unity project if it hasn't already.
        /// </summary>
        private static readonly ScopedRegistry DREditorRegistry = new(RegistryName, URL, Scopes);
#else
        /// <summary>
        /// The name of the Newtonsoft.Json package this script will need to easily add registries.
        /// </summary>
        private static readonly string NewtonsoftJsonPackage = "com.unity.nuget.newtonsoft-json";
#endif

        static DRERegistryInstaller()
        {
#if !NEWTONSOFT_JSON
            EditorPrefs.DeleteKey(GetProjectName() + RegistryName);

            Client.Add(NewtonsoftJsonPackage);

            CompilationPipeline.RequestScriptCompilation();
#else
            if (!EditorPrefs.GetBool(GetProjectName() + RegistryName))
                AddScopedRegistry();
#endif
        }


        /// <summary>
        /// Gets the name of the project to set a unique editor preference setting that a registry has been added.
        /// </summary>
        /// <returns>The name of the project if it could be found, otherwise an empty string.</returns>
        private static string GetProjectName()
        {
            string path = Application.dataPath;

            int assetsIndex = path.LastIndexOf("/Assets");

            if (assetsIndex == -1)
                assetsIndex = path.LastIndexOf("\\Assets");

            if (assetsIndex == -1)
            {
                Debug.LogError("ALERT: Assets directory not found in path.");

                return string.Empty;
            }

            string projectPath = path[..assetsIndex];

            int lastSeparatorIndex = projectPath.LastIndexOf('/');

            if (lastSeparatorIndex == -1)
                lastSeparatorIndex = projectPath.LastIndexOf('\\');

            if (lastSeparatorIndex == -1)
            {
                Debug.LogError("ALERT: Invalid project path.");

                return string.Empty;
            }

            string projectName = projectPath[(lastSeparatorIndex + 1)..];

            return projectName;
        }
#if NEWTONSOFT_JSON
        /// <summary>
        /// Adds a scoped registry to the package manager to allow for new libraries of packages to be easily downloadable.
        /// </summary>
        /// <param name="registry">If left empty adds the DREditor Registry.</param>
        public static void AddScopedRegistry(ScopedRegistry registry = null)
        {
            registry ??= DREditorRegistry;

            var manifestPath = Path.Combine(Application.dataPath, "..", "Packages/manifest.json");

            var manifestJson = File.ReadAllText(manifestPath);

            var manifest = JsonConvert.DeserializeObject<ManifestJson>(manifestJson);

            foreach(var existingRegistry in manifest.scopedRegistries)
                if (existingRegistry.name == RegistryName)
                {
                    EditorPrefs.SetBool(GetProjectName() + registry.name, true);

                    return;
                }

            manifest.scopedRegistries.Add(registry);

            File.WriteAllText(manifestPath, JsonConvert.SerializeObject(manifest, Formatting.Indented));

            EditorPrefs.SetBool(GetProjectName() + registry.name, true);

            CompilationPipeline.RequestScriptCompilation();
        }
#endif
    }
}