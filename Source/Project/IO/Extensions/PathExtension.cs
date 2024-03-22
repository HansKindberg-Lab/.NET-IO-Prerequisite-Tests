//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace Project.IO.Extensions
//{
//	/// <summary>
//	/// - https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.cs
//	/// - https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.Unix.cs
//	/// - https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.Windows.cs
//	/// </summary>
//	public static class PathExtension
//	{
//		private static bool IsPartiallyQualified(string? path)
//		{
//			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//			{

//			}
//			else
//			{
//				// https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.Unix.cs#L77
//				// This is much simpler than Windows where paths can be rooted, but not fully qualified (such as Drive Relative)
//				// As long as the path is rooted in Unix it doesn't use the current directory and therefore is fully qualified.
//				return !Path.IsPathRooted(path);
//			}

//			if(path.Length < 2)
//			{
//				// It isn't fixed, it must be relative.  There is no way to specify a fixed
//				// path with one character (or less).
//				return true;
//			}

//			if(IsDirectorySeparator(path[0]))
//			{
//				// There is no valid way to specify a relative path with two initial slashes or
//				// \? as ? isn't valid for drive relative paths and \??\ is equivalent to \\?\
//				return !(path[1] == '?' || IsDirectorySeparator(path[1]));
//			}

//			// The only way to specify a fixed path that doesn't begin with two slashes
//			// is the drive, colon, slash format- i.e. C:\
//			return !((path.Length >= 3)
//			         && (path[1] == VolumeSeparatorChar)
//			         && IsDirectorySeparator(path[2])
//			         // To match old behavior we'll check the drive character for validity as the path is technically
//			         // not qualified if you don't have a valid drive. "=:\" is the "=" file's default data stream.
//			         && IsValidDriveChar(path[0]));
//		}
//	}
//}

