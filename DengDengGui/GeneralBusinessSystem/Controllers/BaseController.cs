using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using Microsoft.Extensions.Logging;
using NLog;
using Microsoft.AspNetCore.Http;

namespace GeneralBusinessSystem.Controllers
{
    /// <summary>
    /// ƽ̨���Ʋ����
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// ҵ��ִ���
        /// </summary>
        protected IBusinessRepository _businessRepository;
        /// <summary>
        /// ��־��
        /// </summary>
        protected Logger _log;
        /// <summary>
        /// ͨ����Ŀƽ̨���Ʋ�ʵ��
        /// </summary>
        /// <param name="businessRepository">ҵ��ִ���</param>
        public BaseController(IBusinessRepository businessRepository)
        {
            _log = LogManager.GetCurrentClassLogger();
            _businessRepository = businessRepository;
        }

        /// <summary>
        /// ͨ����Ŀƽ̨���Ʋ�ʵ��
        /// </summary>
        public BaseController()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public int CompanyID
        {
            get
            {               
                var companyID = GetSessionValue("companyid");
                return Convert.ToInt32(companyID);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string  Name
        {
            get
            {
                return GetSessionValue("name");
            }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public string  UserName
        {
            get
            {              
                return GetSessionValue("username");
            }
        }
        /// <summary>
        /// ��ȡsession�е�����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        string GetSessionValue(string key)
        {
            var cookie = Request.Cookies["browseweb"];
            var userJson = HttpContext.Session.GetString(cookie + HttpContext.Connection.RemoteIpAddress.ToString());
            var userObj = Newtonsoft.Json.JsonConvert.DeserializeObject(userJson);
            var value = (userObj as Newtonsoft.Json.Linq.JObject).GetValue(key).First.ToString();
            return value;
        }
    }
}