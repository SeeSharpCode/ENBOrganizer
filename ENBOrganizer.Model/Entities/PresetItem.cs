namespace ENBOrganizer.Model.Entities
{
    public interface PresetItem : IEntity
    {
        string Path { get; set; }

        void Delete();
    }
}
