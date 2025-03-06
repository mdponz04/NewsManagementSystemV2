namespace BusinessLogic.DTOs.SystemAccountDTOs
{
    public class GetSystemAccountDTO : BaseSystemAccountDTO
    {
        public short AccountId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
