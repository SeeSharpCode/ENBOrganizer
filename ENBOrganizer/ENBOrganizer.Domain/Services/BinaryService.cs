using ENBOrganizer.Domain.Entities;

namespace ENBOrganizer.Domain.Services
{
    public class BinaryService : DataService<Binary>
    {
        public void ImportDirectory(string sourceDirectoryPath, Game game)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            Preset preset = new Preset(sourceDirectory.Name, game);

            try
            {
                base.Add(preset);

                sourceDirectory.CopyTo(preset.Directory.FullName);

                CreateMasterListItemsFromPreset(preset);
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
