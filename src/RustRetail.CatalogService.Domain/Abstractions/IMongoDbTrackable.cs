namespace RustRetail.CatalogService.Domain.Abstractions
{
    public interface IMongoDbTrackable
    {
        public DateTime? CreatedDateTime { get; }
        public DateTime? UpdatedDateTime { get; }

        void SetCreatedDateTime(DateTime? createdDateTime);
        void SetUpdatedDateTime(DateTime? updatedDateTime);
    }
}
