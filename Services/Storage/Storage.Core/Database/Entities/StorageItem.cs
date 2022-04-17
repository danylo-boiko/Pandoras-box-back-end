using Storage.Core.Enums;

namespace Storage.Core.Database.Entities
{
    public class StorageItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string Location { get; set; }

        public FileCategory Category { get; set; }
    }
}
