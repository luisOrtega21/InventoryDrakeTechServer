using INVENTORY.SHARED.Dto;

namespace INVENTORY.SERVER.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
