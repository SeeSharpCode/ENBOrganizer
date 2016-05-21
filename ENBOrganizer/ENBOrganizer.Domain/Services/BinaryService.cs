using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;
using System.IO;
using System.IO.Compression;

namespace ENBOrganizer.Domain.Services
{
    public class BinaryService : DataService<Binary>
    {
        private readonly MasterListService _masterListService;

        public BinaryService(MasterListService masterListService)
            : base("Binaries.xml")
        {
            _masterListService = masterListService;
        }

        public new void Delete(Binary binary)
        {
            binary.Directory.Delete(true);

            base.Delete(binary);
        }

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
    }
}
