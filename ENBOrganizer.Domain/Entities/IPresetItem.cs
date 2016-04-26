namespace ENBOrganizer.Domain.Entities
{
    public interface IPresetItem : IEntity
    {
        string Path { get; }

        void Rename(string newName);
        void Delete();
    }
}
