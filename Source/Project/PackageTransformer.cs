using Project.IO.Extensions;

namespace Project
{
	public class PackageTransformer
	{
		#region Methods

		protected internal virtual string? NormalizePath(string? path)
		{
			return path?.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).ToLowerInvariant();
		}

		protected internal virtual bool PathsAreEqual(string firstPath, string secondPath)
		{
			return string.Equals(this.NormalizePath(firstPath), this.NormalizePath(secondPath), StringComparison.OrdinalIgnoreCase);
		}

		public virtual void ValidateFilePath(string? action, string? directoryPath, string? filePath)
		{
			if(string.IsNullOrWhiteSpace(filePath))
				return;

			// If the file-path is a relative path.
			if(!PathExtension.IsPathFullyQualified(filePath!))
				return;

			if(directoryPath == null)
				throw new ArgumentNullException(nameof(directoryPath));

			if(!PathExtension.IsPathFullyQualified(directoryPath))
				throw new ArgumentException($"The directory-path can not be relative ({directoryPath}).", nameof(directoryPath));

			// We are not allowed to delete files outside the directory-path.
			// We can not transform files outside the directory-path because we can not resolve the destination for those files.
			var directory = new DirectoryInfo(directoryPath);
			var file = new FileInfo(filePath);
			var parentDirectory = file.Directory;

			while(parentDirectory != null)
			{
				if(this.PathsAreEqual(directory.FullName, parentDirectory.FullName))
					return;

				parentDirectory = parentDirectory.Parent;
			}

			throw new InvalidOperationException($"It is not allowed to {action} the file \"{filePath}\". The file is outside the directory-path \"{directoryPath}\".");
		}

		#endregion
	}
}