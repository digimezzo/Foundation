namespace Digimezzo.Foundation.Core.Win32
{
    internal enum SHFileOperationReturnCode : ulong
    {
        /// <summary>
        /// Operation completed successfully.
        /// </summary>
        SUCCESSFUL = 0L,

        /// <summary>
        /// The process cannot access the file because it is being used by another process.
        /// </summary>
        ERROR_SHARING_VIOLATION = 32L,

        /// <summary>
        /// MAX_PATH was exceeded during the operation.
        /// </summary>
        DE_ERROR_MAX = 0xB7,

        /// <summary>
        /// An unspecified error occurred on the destination.
        /// </summary>
        ERRORONDEST = 0x10000
    }
}