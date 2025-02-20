using Data.Enum;

namespace Data.DTOs.SystemAccountDTOs
{
    public class PostSystemAccountDTO : BaseSystemAccountDTO
    {
        public EnumRole Role { get; set; }
        public string AccountPassword { get; set; } = string.Empty;
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
