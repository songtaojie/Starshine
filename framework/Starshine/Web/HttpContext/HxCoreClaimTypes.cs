using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine
{
    /// <summary>
    /// 自定义声明
    /// </summary>
    public static class HxClaimTypes
    {
        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4    
        /// </summary>
        public const string Jti = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/jti";
        /// <summary>
        /// IdentityServer4的name claim
        /// </summary>
        public const string OrgId = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/oid";
    }

    /// <summary>
    /// Claim的常量值
    /// </summary>
    internal static class HxClaimValues
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string SuperAdmin = "SuperAdmin";

        /// <summary>
        /// 管理员的值
        /// </summary>
        public const string Admin = "Admin";
    }
}
