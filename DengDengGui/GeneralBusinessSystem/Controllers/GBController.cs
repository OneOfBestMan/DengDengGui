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
    /// ͨ����Ŀƽ̨���Ʋ���
    /// </summary>
    public class GBController : Controller
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
        public GBController(IBusinessRepository businessRepository)
        {
            _log = LogManager.GetCurrentClassLogger();
            _businessRepository = businessRepository;
        }

        /// <summary>
        /// ͨ����Ŀƽ̨���Ʋ�ʵ��
        /// </summary>
        public GBController()
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
                var cookie = Request.Cookies["browseweb"];
                var userJson = HttpContext.Session.GetString(cookie + HttpContext.Connection.RemoteIpAddress.ToString());
                var userObj = Newtonsoft.Json.JsonConvert.DeserializeObject(userJson);
                var companyID = (userObj as Newtonsoft.Json.Linq.JObject).GetValue("companyid").First.ToString();
                return Convert.ToInt32(companyID);
            }
        }
    }
}