using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AppUtility.AppIO
{    
    public interface IDirectoryFileService
    {
        bool CreateDirectoryIfNotExist(string DirectoryPath);
        bool CreateFileFromBase64String(string FileContentsBase64, string FilePath);
    }

    public class DirectoryFileService : IDirectoryFileService
    {
        public DirectoryFileService()
        {
        }

        public bool CreateDirectoryIfNotExist(string DirectoryPath)
        {
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateFileFromBase64String(string FileContentsBase64, string FilePath)
        {
            try
            {
                string TempFileContentBase64 = Regex.Replace(FileContentsBase64, "^data:image/[a-zA-Z]+;base64,", string.Empty);
                Byte[] bytes = Convert.FromBase64String(TempFileContentBase64);
                System.IO.File.WriteAllBytes(FilePath, bytes);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
