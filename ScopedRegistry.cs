//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
namespace DREditorInstaller
{
    /// <summary>
    /// Serializable class to hold the data for the manifest.json's Scoped Registries.
    /// </summary>
    public class ScopedRegistry
    {
        /// <summary>
        /// The Display Name for the registry in the package manager.
        /// </summary>
        public string name;

        /// <summary>
        /// The URL the registry is located at for download.
        /// </summary>
        public string url;

        /// <summary>
        /// The scopes for the registry.
        /// </summary>
        public string[] scopes;


        public ScopedRegistry() { }

        public ScopedRegistry(string name, string url, string[] scopes)
        {
            this.name = name;

            this.url = url;

            this.scopes = scopes;
        }
    }
}