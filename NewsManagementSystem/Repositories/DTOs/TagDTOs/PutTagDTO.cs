namespace Repositories.DTOs.TagDTOs
{
    public class PutTagDTO : BaseTagDTO
    {
        public int TagId { get; set; }
        public string Note { get; set; }
    }
}
