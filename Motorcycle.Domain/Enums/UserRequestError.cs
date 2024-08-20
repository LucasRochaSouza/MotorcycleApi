using System.ComponentModel;

namespace Domain.Enums
{
    public enum UserRequestError
    {
        [Description("None")]
        None = 0,

        [Description("First Name should be provided and not contain numbers")]
        FirstName = 1,

        [Description("Last Name should be provided and not contain numbers")]
        LastName = 2,

        [Description("Email format is not valid")]
        EmailFormat = 3,

        [Description("Email address must be unique")]
        EmailMustBeUnique = 4
    }
}