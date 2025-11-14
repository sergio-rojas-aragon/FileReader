
using FileReader.Core.DTO;


namespace FileReader.Core.Interfaces
{
    public abstract class FolderResolverBase : IFolderResolver
    {
        public readonly IFolderPath folderPath;

        protected FolderResolverBase(IFolderPath folderPath)
        {
            // get the names of the file folders
            this.folderPath = folderPath;
        }

        public abstract bool CreateAllFolderIfNoExists(string path);

        // add obligatory
        protected abstract void SetDirectoryPaths(string path);
        protected abstract void tryCreateDirectory(string path);

    }
}
