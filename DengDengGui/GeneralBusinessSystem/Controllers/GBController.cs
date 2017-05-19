using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using Microsoft.Extensions.Logging;
using NLog;

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
        public GBController( )
        {
            _log = LogManager.GetCurrentClassLogger();
        }
    }
}