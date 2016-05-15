using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ENBOrganizer.Domain.Services
{
    public class BinaryService : DataService<Binary>
    {
        public void ImportDirectory(string sourceDirectoryPath, Game game)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            Binary binary = new Binary(sourceDirectory.Name, game);

            try
            {
                Add(binary);

                sourceDirectory.CopyTo(binary.Directory.FullName);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                Delete(binary);

                throw;
            }
        }

        public void ImportArchive(string archivePath, Game game)
        {
            Binary binary = new Binary(Path.GetFileNameWithoutExtension(archivePath), game);

            try
            {
                Add(binary);

                ZipFile.ExtractToDirectory(archivePath, binary.Directory.FullName);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                Delete(binary);

                throw;
            }
        }

        public void ImportActiveFiles(Binary binary)
        {
            try
            {
                Add(binary);

                List<MasterListItem> masterListItems = _masterListService.GetAll();
                List<string> gameDirectories = Directory.GetDirectories(preset.Game.DirectoryPath).ToList();
                List<string> gameFiles = Directory.GetFiles(preset.Game.DirectoryPath).ToList();

                foreach (MasterListItem masterListItem in masterListItems)
                {
                    string installedPath = Path.Combine(preset.Game.DirectoryPath, masterListItem.Name);

                    if (masterListItem.Type.Equals(MasterListItemType.Directory) && gameDirectories.Contains(installedPath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(installedPath);
                        directory.CopyTo(Path.Combine(preset.Directory.FullName, directory.Name));
                    }
                    else if (gameFiles.Contains(installedPath))
                    {
                        FileInfo file = new FileInfo(installedPath);
                        file.CopyTo(Path.Combine(preset.Directory.FullName, file.Name));
                    }
                }
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                Delete(preset);

                throw;
            }
        }
    }
}
