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

        public void Import(Binary binary, string sourcePath)
        {
            try
            {
                Add(binary);

                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

                if (sourceDirectory.Exists)
                    sourceDirectory.CopyTo(binary.Directory.FullName);
                else
                    ZipFile.ExtractToDirectory(sourcePath, binary.Directory.FullName);

                _masterListService.CreateMasterListItems(binary.Directory);
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
