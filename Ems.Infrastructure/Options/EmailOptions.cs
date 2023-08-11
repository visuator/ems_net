namespace Ems.Infrastructure.Options;

public class EmailOptions
{
    public Uri ConfirmationLink { get; set; }
    public Uri PasswordResetLink { get; set; }

    public string PasswordResetEmailSubject { get; set; }
    public string PasswordResetEmailTemplateFileName { get; set; }

    public string RegistrationEmailSubject { get; set; }
    public string RegistrationEmailTemplateFileName { get; set; }

    public string ReconfirmationEmailSubject { get; set; }
    public string ReconfirmationEmailTemplateFileName { get; set; }

    public string NewPasswordEmailSubject { get; set; }
    public string NewPasswordEmailTemplateFileName { get; set; }
}