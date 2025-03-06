namespace BusinessLogic.DTOs.CategoryDTOs
{
    public class BaseCategoryDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
