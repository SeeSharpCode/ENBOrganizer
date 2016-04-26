namespace ENBOrganizer.Model.Entities
{
    public interface IPresetItem : IEntity
    {
        string Path { get; set; }

        void Rename(string newName);
        void Delete();
    }
}
