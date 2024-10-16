
namespace Mmicovic.RPSSL.Service.Models
{
    public class User
    {
        public required string UserName { get; set; }

        public required byte[] Salt { get; set; }
        public required byte[] PassphraseHash { get; set; }
    }
}
