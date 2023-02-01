using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS.Host.Data;

public static class Constants
{
    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
    }

    public static class StandardScopes
    {
        public const string Roles = "roles";
        public const string CatalogApi = "catalog-api";
    }
}
