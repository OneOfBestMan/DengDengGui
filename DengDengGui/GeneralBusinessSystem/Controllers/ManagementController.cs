using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;

namespace GeneralBusinessSystem.Controllers
{
    public class ManagementController : GBController
    {
        /// <summary>
        /// ʵ����ManagementController
        /// </summary>
        /// <param name="businessRepository">ҵ��ִ���</param>
        public ManagementController(IBusinessRepository businessRepository) : base(businessRepository)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}