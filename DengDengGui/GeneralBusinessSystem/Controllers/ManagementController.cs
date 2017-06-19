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
    public class ManagementController : BaseController
    {
        /// <summary>
        /// ����ģ��
        /// </summary>
        IBillModuleRepository _billModule;
        /// <summary>
        /// ��ѯģ��
        /// </summary>
        IQueryModuleRepository _queryModule;
        /// <summary>
        /// ͼ��ģ��
        /// </summary>
        IChartModuleRepository _chartModule;
        /// <summary>
        /// ʵ����ManagementController
        /// </summary>
        /// <param name="businessRepository">ҵ��ִ���</param>
        public ManagementController(IBusinessRepository business, IBillModuleRepository billModule, IQueryModuleRepository queryModule, IChartModuleRepository chartModule) : base(business)
        {
            _billModule = billModule;
            _queryModule = queryModule;
            _chartModule = chartModule;
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
            try
            {
                var list = _businessRepository.GetMenus();
                return new JsonResult(new { result = 1, message = $"��ѯȫ���˵��ɹ�",data= list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ѯȫ���˵���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ѯȫ���˵���{exc.Message}" });
            }
        }

        /// <summary>
        /// ��Ӳ˵�
        /// </summary>
        /// <param name="rolename">����</param>
        /// <returns></returns>
        [HttpPost("addmenu")]
        public IActionResult AddMenu(string name)
        {
            try
            {
               // _businessRepository.AddMenu(name, CompanyID);
                _businessRepository.AddMenu(name, 1);   
                return new JsonResult(new { result = 1, message = $"��Ӳ˵��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��Ӳ˵���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��Ӳ˵���{exc.Message}" });
            }
        }
        /// <summary>
        /// �޸Ĳ˵� 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpPost("modifymenu")]
        public IActionResult ModifyMenu(int id, string name)
        {
            try
            {
                _businessRepository.ModifyMenu(id, name);
                return new JsonResult(new { result = 1, message = $"�޸Ĳ˵��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"�޸Ĳ˵���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"�޸Ĳ˵���{exc.Message}" });
            }
        }
        /// <summary>
        /// ɾ���˵�
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deletemenu")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _businessRepository.RemoveMenu(id);
                return new JsonResult(new { result = 1, message = $"ɾ���˵��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"ɾ���˵���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"ɾ���˵���{exc.Message}" });

            }
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
            var billModules = _billModule.GetBillModules();
            var queryModules = _queryModule.GetQueryModules();
            var chartModules = _chartModule.GetChartModules();
            return new JsonResult(new { menus = menus, bills = billModules, queries = queryModules, charts = chartModules }, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }

        #endregion


    }
}