using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Common;

namespace GeneralBusinessSystem.Controllers
{
    /// <summary>
    /// ��̨����controller
    /// </summary>
    public class ManagementController : GBController
    {
        /// <summary>
        /// ʵ����ManagementController
        /// </summary>
        /// <param name="businessRepository">ҵ��ִ���</param>
        public ManagementController(IBusinessRepository businessRepository) : base(businessRepository)
        {

        }
        /// <summary>
        /// ��̨��ҳ
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #region �˵�����
        /// <summary>-
        /// �˵�����
        /// </summary>
        /// <returns></returns>
        [HttpGet("menus")]
        public IActionResult Menus()
        {
            return View();
        }

        /// <summary>
        /// ��ѯȫ���˵�
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmenus")]
        public IActionResult GetMenus()
        {
            var list = _businessRepository.GetMenus();
            return new JsonResult(list, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }

        /// <summary>
        /// ��Ӳ˵�
        /// </summary>
        /// <param name="rolename">����</param>
        /// <returns></returns>
        [HttpPost("addmenu")]
        public bool AddMenu(string name)
        {
            return _businessRepository.AddMenu(name) > 0 ? true : false;
        }
        /// <summary>
        /// �޸Ĳ˵� 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpPost("modifymenu")]
        public bool ModifyMenu(int id, string name)
        {
            return _businessRepository.ModifyMenu(id, name) > 0 ? true : false;
        }
        /// <summary>
        /// ɾ���˵�
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deletemenu")]
        public bool DeleteRole(int id)
        {
            return _businessRepository.RemoveMenu(id) > 0 ? true : false;
        }
        #endregion
        #region �˵�ģ�����


        /// <summary>
        /// �˵�ģ�����
        /// </summary>
        /// <returns></returns>
        [HttpGet("menumodules")]
        public IActionResult MenuModules()
        {
            return View();
        }
        /// <summary>
        /// ��ȡ�˵�ģ���еĲ˵��б����ݣ���ѯ��ͼ��ģ���б�
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMenuModules()
        {
            var menus = _businessRepository.GetMenus();
            var billModules = _businessRepository.GetBillModules();
            var queryModules = _businessRepository.GetQueryModules();
            var chartModules = _businessRepository.GetChartModules();
            return new JsonResult(new { menus = menus, bills = billModules, queries = queryModules, charts = chartModules }, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }

        #endregion


    }
}