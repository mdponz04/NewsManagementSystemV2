namespace Repositories.DTOs.SystemAccountDTOs
{
    public class GetSystemAccountDTO : BaseSystemAccountDTO
    {
        public int AccountId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
