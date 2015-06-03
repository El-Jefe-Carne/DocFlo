using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocFlow.Services.Interfaces;

namespace DocFlow.Services.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Determines whether this instance [can find file] the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">filePath</exception>
        public bool FileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            return File.Exists(filePath);            
        }

        /// <summary>
        /// Moves the file.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        /// <exception cref="ArgumentNullException">
        /// oldPath
        /// or
        /// newPath
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">Could not find directory:  + newDirectoryName</exception>
        /// <exception cref="FileNotFoundException">Could not find file at location:  + oldPath</exception>
        /// <exception cref="Exception">Cannot move file because a file with the name ' + Path.GetFileName(newPath) + ' already exists in directory ' + Path.GetDirectoryName(newPath) + '</exception>
        public void MoveFile(string oldPath, string newPath)
        {
            //ensure the old path variable exists
            if (string.IsNullOrEmpty(oldPath))
                throw new ArgumentNullException("oldPath");

            //ensure the newPath variable exists
            if (string.IsNullOrEmpty(newPath))
                throw new ArgumentNullException("newPath");

            //if the new file name does not exist in the directory 
            if (!FileExists(newPath))
            {                
                //if the file exists at the old path
                if (FileExists(oldPath))
                {
                    //get the new directory's name
                    var newDirectoryName = Path.GetDirectoryName(newPath);

                    //if the directory of the new path is NOT null or empty
                    if (!string.IsNullOrEmpty(newDirectoryName))
                    {
                        //move the old file to the new path
                        File.Move(oldPath, newPath);
                    }
                    else
                        throw new DirectoryNotFoundException("Could not find directory: " + newDirectoryName);
                }
                else
                    throw new FileNotFoundException("Could not find file at location: " + oldPath);
            }
            else
                throw new Exception("Unable to move file because a file with the name '" + Path.GetFileName(newPath) + "' already exists in directory '" + Path.GetDirectoryName(newPath) + "'");                         
        }

        /// <summary>
        /// Renames the file.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        /// <exception cref="ArgumentNullException">
        /// oldPath
        /// or
        /// newPath
        /// </exception>
        /// <exception cref="FileNotFoundException">Unable to locate file:  + oldPath</exception>
        /// <exception cref="Exception">Unable to rename file because a file with the name ' + Path.GetFileName(newPath) + ' already exists in directory ' + Path.GetDirectoryName(newPath) + '</exception>
        public void RenameFile(string oldPath, string newPath)
        {
            if (string.IsNullOrEmpty(oldPath))
                throw new ArgumentNullException("oldPath");

            if (string.IsNullOrEmpty(newPath))
                throw new ArgumentNullException("newPath");

            //if the new file name does not exist in the directory 
            if (!FileExists(newPath))
            {
                //if the old file exists
                if (FileExists(oldPath))
                {
                    //File.Move is how C# handles renaming as well.
                    File.Move(oldPath, newPath);
                }
                else
                {
                    throw new FileNotFoundException("Unable to locate file: " + oldPath);
                } 
            }
            else
                throw new Exception("Unable to rename file because a file with the name '" + Path.GetFileName(newPath) + "' already exists in directory '" + Path.GetDirectoryName(newPath) + "'");                        
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException">path</exception>
        /// <exception cref="Exception">The file could not be found.</exception>
        public void DeleteFile(string path)
        {
            //ensure the old path exists
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            //ensure that the file at the old path exists
            if (FileExists(path))
                File.Delete(path);
            else
                throw new FileNotFoundException("Unable to find file at location: " + path);            
        }


        public IEnumerable<string> GetFilesFromDirectory(string directory)
        {
            //select all file names in the specified directory
            return Directory.EnumerateFiles(directory).Where(fn =>
                    Path.GetExtension(fn) == ".pdf"
                    || Path.GetExtension(fn) == ".jpeg"
                    || Path.GetExtension(fn) == ".jpg"
                    || Path.GetExtension(fn) == ".gif"
                    || Path.GetExtension(fn) == ".tiff"
                    || Path.GetExtension(fn) == ".png")
                .Select(p => Path.GetFileName(p));                        
        }
    }
}
