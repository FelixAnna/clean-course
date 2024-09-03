using Entities.Entities;

namespace Services.Kids
{
    public class KidModel(KidEntity? entity)
    {
        public int Id { get; set; } = entity?.KidId ?? 0;

        public string Name { get; set; } = entity?.Name ?? string.Empty;

        public int StartSchoolYear { get; set; } = entity?.StartSchoolYear ?? 0;

        public bool Selected { get; set; } = entity?.Selected ?? false;
    }
}
