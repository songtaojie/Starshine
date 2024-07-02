using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Starshine
{
    /// <summary>
    /// 用户上下文操作类
    /// </summary>
    public sealed class UserManager
    {
        /// <summary>
        /// 是否使用IdentityServer4
        /// </summary>
        private static bool _isUseIds4 = false;
        /// <summary>
        /// HttpContext访问器
        /// </summary>
        private IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Http上下文操作类
        /// </summary>
        public HttpContext? HttpContext
        {
            get
            {
                return _contextAccessor.HttpContext;
            }
        }

        /// <summary>
        /// 用户上下文操作类
        /// </summary>
        /// <param name="contextAccessor"></param>
        public UserManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _isUseIds4 = StarshineApp.Settings.UseIdentityServer4 == true;
        }
        /// <summary>
        /// 用户的名字
        /// </summary>
        public string? UserName
        {
            get
            { 
                string? name = HttpContext?.User?.Identity?.Name;
                if (!string.IsNullOrEmpty(name)) return name;
                //string getNameType = _isUseIds4 ? HxClaimTypes.Ids4Name : ClaimTypes.Name;
               return GetClaimValueByType(ClaimTypes.Name).FirstOrDefault();
            }
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuperAdmin
        {
            get
            {
                var claims = GetClaimsIdentity();
                var isAdmin = claims.Any(c => c.Type == ClaimTypes.Role && c.Value == HxClaimValues.SuperAdmin);
                return IsAuthenticated && isAdmin;
            }
        }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                var claims = GetClaimsIdentity();
                var isAdmin = claims.Any(c => c.Type == ClaimTypes.Role && c.Value == HxClaimValues.Admin);
                return IsAuthenticated && isAdmin;
            }
        }
        /// <summary>
        /// Jwt的id
        /// </summary>
        public string? JwtId => GetClaimValueByType(HxClaimTypes.Jti).FirstOrDefault();

        /// <summary>
        /// 用户的id
        /// </summary>
        public T? GetUserId<T>()
        {
            return GetClaimValueByType<T>(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// 获取机构id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetOrgId<T>()
        {
            return GetClaimValueByType<T>(HxClaimTypes.OrgId);
        }

        /// <summary>
        /// 用户的id
        /// </summary>
        public string? UserId
        {
            get
            {
                return GetClaimValueByType(ClaimTypes.NameIdentifier).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public string? GetCookieValue(string cookieName)
        {
            return HttpContext?.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 设置cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expires">过期时间</param>
        public void SetCookieValue(string cookieName, string value, DateTime? expires = null)
        {
            string? cookieValue = GetCookieValue(cookieName);
            if (!string.IsNullOrEmpty(cookieValue)) HttpContext?.Response?.Cookies.Delete(cookieName);
            if (expires.HasValue)
            {
                HttpContext?.Response.Cookies.Append(cookieName, value, new CookieOptions
                {
                    Expires = new DateTimeOffset(expires.Value)
                });
            }
            else
            {
                HttpContext?.Response.Cookies.Append(cookieName, value);
            }
        }
        /// <summary>
        /// 是否已经验证，即是否一登录
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated => HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var auth = HttpContext?.Request?.Headers["Authorization"];
            if (!auth.HasValue || Microsoft.Extensions.Primitives.StringValues.IsNullOrEmpty(auth.Value)) return string.Empty;
            return auth.Value.ToString().Replace("Bearer ", "");
        }
       
        /// <summary>
        /// 获取claims集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();
        }

        /// <summary>
        /// 根据claim获取相应的值
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        public List<string> GetClaimValueByType(string ClaimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();

        }

        /// <summary>
        /// 根据claimType获取ClaimValue值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public dynamic? GetClaimValueByType<T>(string claimType)
        {
            var claim = GetClaimValueByType(claimType).FirstOrDefault();
            if (claim == null) return default(T);
            var type = Type.GetTypeCode(typeof(T));
            return type switch
            {
                TypeCode.Object => JsonSerializer.Deserialize<T>(claim),
                TypeCode.Boolean => Convert.ToBoolean(claim),
                TypeCode.Char => Convert.ToChar(claim),
                TypeCode.SByte => Convert.ToSByte(claim),
                TypeCode.Byte => Convert.ToByte(claim),
                TypeCode.Int16 => Convert.ToInt16(claim),
                TypeCode.UInt16 => Convert.ToUInt16(claim),
                TypeCode.Int32 => Convert.ToInt32(claim),
                TypeCode.UInt32 => Convert.ToUInt32(claim),
                TypeCode.Int64 => Convert.ToInt64(claim),
                TypeCode.UInt64 => Convert.ToUInt64(claim),
                TypeCode.Single => Convert.ToSingle(claim),
                TypeCode.Double => Convert.ToDouble(claim),
                TypeCode.Decimal => Convert.ToDecimal(claim),
                TypeCode.DateTime => Convert.ToDateTime(claim),
                TypeCode.DBNull or TypeCode.Empty => claim,
                _ => claim,
            };
        }
    }
}
