//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;

namespace DREditorInstaller
{
    /// <summary>
    /// Serializable class to hold the data for the manifest.json.
    /// </summary>
    public class ManifestJson
    {
        /// <summary>
        /// The extracted dependencies of the manifest.json.
        /// </summary>
        public Dictionary<string, string> dependencies = new();

        /// <summary>
        /// The scoped registries of the manifest.json.
        /// </summary>
        public List<ScopedRegistry> scopedRegistries = new();
    }
}