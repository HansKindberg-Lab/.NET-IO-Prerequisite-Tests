namespace Project
{
	public class Example
	{
		#region Methods

		protected internal virtual string? NormalizePath(string? path)
		{
			return path?.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).ToUpperInvariant();
		}

		protected internal virtual bool PathsAreEqual(string? firstPath, string? secondPath)
		{
			return string.Equals(this.NormalizePath(firstPath), this.NormalizePath(secondPath), StringComparison.OrdinalIgnoreCase);
		}

		protected internal virtual void ValidateFilePath(string? directoryPath, string? filePath)
		{
			if(string.IsNullOrWhiteSpace(filePath))
				return;

			if(!Path.IsPathRooted(filePath))
				return;

			if(!Uri.TryCreate(filePath, UriKind.RelativeOrAbsolute, out var fileUri))
				throw new ArgumentException($"Could neither create an absolute uri nor a relative uri from file-path \"{filePath}\".", nameof(filePath));

			if(!fileUri.IsAbsoluteUri)
				return;

			if(directoryPath == null)
				throw new ArgumentNullException(nameof(directoryPath));

			if(!Uri.TryCreate($"{directoryPath.TrimEnd('/', '\\')}\\", UriKind.Absolute, out var directoryUri))
				throw new ArgumentException($"Could not create an absolute uri from directory-path \"{directoryPath}\".", nameof(directoryPath));

			if(this.PathsAreEqual(directoryPath, filePath))
				throw new InvalidOperationException($"The directory-path and file-path can not be equal.");

			// We are not allowed to delete files outside the directory-path.
			// We can not transform files outside the directory-path because we can not resolve the destination for those files.
			if(!directoryUri.IsBaseOf(fileUri))
				throw new InvalidOperationException($"The file \"{filePath}\" is outside the directory-path \"{directoryPath}\".");
		}

		#endregion
	}
}