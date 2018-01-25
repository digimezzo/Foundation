using Digimezzo.Foundation.Core.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// Gets the directory name for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The directory name</returns>
        public static string DirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Gets the file name with extension for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The file name with extension</returns>
        public static string FileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Gets the file name without extension for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The file name without extension</returns>
        public static string FileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Gets the size in bytes for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The size in bytes</returns>
        public static long SizeInBytes(string path)
        {
            return new FileInfo(path).Length;
        }

        /// <summary>
        /// Gets the date created in DateTime format for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The date created in DateTime format</returns>
        public static DateTime DateCreated(string path)
        {
            return new FileInfo(path).CreationTime;
        }

        /// <summary>
        /// Gets the date created in Ticks for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The date created in Ticks</returns>
        public static long DateCreatedTicks(string path)
        {
            return new System.IO.FileInfo(path).CreationTime.Ticks;
        }

        /// <summary>
        /// Gets the date modified in DateTime format for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The date modified in DateTime format</returns>
        public static DateTime DateModified(string path)
        {
            return new System.IO.FileInfo(path).LastWriteTime;
        }

        /// <summary>
        /// Gets the date modified in Ticks for a given path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>The date modified in Ticks</returns>
        public static long DateModifiedTicks(string path)
        {
            return new System.IO.FileInfo(path).LastWriteTime.Ticks;
        }

        /// <summary>
        /// Gets if a given path is too long
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>True if the path is too long</returns>
        public static bool IsPathTooLong(string path)
        {
            // The fully qualified file name must be less than 260 characters, 
            // and the directory name must be less than 248 characters. This 
            // simple method just checks for a limit of 248 characters.
            return path.Length >= 248;
        }

        /// <summary>
        /// Gets if a given path is absolute
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>True if the path is absolute</returns>
        public static bool IsAbsolutePath(string path)
        {
            Regex regex = new Regex("^(([a-zA-Z]:\\\\)|(//)).*");
            Match match = regex.Match(path);

            return match.Success;
        }

        /// <summary>
        /// Cleans a given file name from forbidden characters for storage on disk
        /// </summary>
        /// <param name="filename">The given file name</param>
        /// <returns>A file name without forbidden characters</returns>
        public static string SanitizeFilename(string filename)
        {
            string retVal = string.Empty;
            string replaceStr = string.Empty;

            // Invalid characters for filenames: \ / : * ? " < > |
            retVal = filename.Replace("\\", replaceStr);
            retVal = retVal.Replace("/", replaceStr);
            retVal = retVal.Replace(":", replaceStr);
            retVal = retVal.Replace("*", replaceStr);
            retVal = retVal.Replace("?", replaceStr);
            retVal = retVal.Replace("\"", replaceStr);
            retVal = retVal.Replace("<", replaceStr);
            retVal = retVal.Replace(">", replaceStr);
            retVal = retVal.Replace("|", replaceStr);

            return retVal;
        }

        /// <summary>
        /// Sends a file to the Recycle Bin
        /// </summary>
        /// <param name="path">The path of the directory or file to send to the Recycle Bin</param>
        /// <param name="flags">FileOperationFlags to add in addition to FOF_ALLOWUNDO</param>
        private static bool SendToRecycleBin(string path, FileOperationFlags flags)
        {
            var fs = new SHFILEOPSTRUCT
            {
                wFunc = FileOperationType.FO_DELETE,
                pFrom = path + '\0' + '\0',
                fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
            };
            var returnCode = (SHFileOperationReturnCode)NativeMethods.SHFileOperation(ref fs);

            switch (returnCode)
            {
                case SHFileOperationReturnCode.SUCCESSFUL: return true;
                case SHFileOperationReturnCode.ERROR_SHARING_VIOLATION:
                    throw new IOException($"The process cannot access the file '{Path.GetFullPath(path)}' because it is being used by another process.");
                case SHFileOperationReturnCode.DE_ERROR_MAX: throw new IOException($"The length of target path '{Path.GetFullPath(path)}' is over MAX_PATH");
                case SHFileOperationReturnCode.ERRORONDEST:
                    throw new IOException("An unspecified error occurred on the destination.");
                default: throw new NotImplementedException("Not supported SHFileOperation return code: " + returnCode);
            }
        }

        /// <summary>
        /// Sends a path to the Recycle Bin, and displays a warning if the path is being permanently  
        /// destroyed during the delete operation rather than recycled because it is too large.
        /// </summary>
        /// <param name="path">The path to recycle</param>
        public static bool SendToRecycleBinInteractive(string path)
        {
            return SendToRecycleBin(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING);
        }

        /// <summary>
        /// Sends a path to the Recycle Bin silently. It suppresses the warning the path is being permanently destroyed.
        /// </summary>
        /// <param name="path">The path to recycle</param>
        public static bool SendToRecycleBinSilent(string path)
        {
            return SendToRecycleBin(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_SILENT);
        }
    }
}
