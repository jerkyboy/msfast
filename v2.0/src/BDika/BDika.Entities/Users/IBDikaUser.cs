using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.StandardObjects;

namespace BDika.Entities.Users
{
    [Flags]
    public enum UserAttributes : uint
    {
        NoProperties = 0,
        IsRegisteredUser = 1
    }

    [Flags]
    public enum UserPermissions : uint
    {
        NoPermission = 0
    }

    public interface IBDikaUser : IUser 
    {
        new UserID UserID { get; set; }
        string FirstName { get; set; }

        UserAttributes UserAttributes { get; set; }
        UserPermissions UserPermissions { get; set; }

        bool HasAttribute(UserAttributes p);
        void SetAttribute(UserAttributes p, bool b);

        bool HasPermission(UserPermissions p);
        void SetPermission(UserPermissions p, bool b);

    }
}
