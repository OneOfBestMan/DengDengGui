using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeneralBusinessRepository;
using System.Reflection;

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

        #region User �û�����
        /// <summary>
        /// �û�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult Users()
        {
            return View(_businessRepository.GetUsers());
        }
        /// <summary>
        /// ��ѯ�û�
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        [HttpGet("queryusers")]
        public ActionResult QueryUser(string queryName)
        {
            return new JsonResult(_businessRepository.GetUsers(queryName));
        }
        /// <summary>
        /// ����û�
        /// </summary>
        /// <param name="userName">�û���</param>
        /// <param name="password">����</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public bool UserAdd(string userName, string password, string name)
        {
            try
            {
                _businessRepository.AddUser(userName, password, name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="userName">�û���</param>
        /// <param name="password">����</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpPost("modifyuser")]
        public bool UserModify(int ID, string userName, string password, string name)
        {
            try
            {

                _businessRepository.ModifyUser(ID, userName, password, name);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("deleteuser")]
        public bool UserDelete(int ID)
        {

            try
            {
                _businessRepository.RemoveUser(ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Role ��ɫ����
        /// <summary>
        /// ��ӽ�ɫ 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpGet("roles")]
        public IActionResult Roles()
        {
            var list = _businessRepository.GetRoles();
            return View(list);
        }
        [HttpPost("roles")]
        public IActionResult GetRoles()
        {
            var list = _businessRepository.GetRoles();
            return new JsonResult(list, new Newtonsoft.Json.JsonSerializerSettings());
        }
        /// <summary>
        /// ��ӽ�ɫ
        /// </summary>
        /// <param name="rolename">��ɫ����</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public bool AddRole(string rolename)
        {
            return _businessRepository.AddRole(rolename) > 0 ? true : false;
        }
        /// <summary>
        /// �޸Ľ�ɫ 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="rolename">��ɫ����</param>
        /// <returns></returns>
        [HttpPost("modifyrole")]
        public bool ModifyRole(int id, string rolename)
        {
            return _businessRepository.ModifyRole(id, rolename) > 0 ? true : false;
        }
        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deleterole")]
        public bool DeleteRole(int id)
        {
            return _businessRepository.RemoveRole(id) > 0 ? true : false;
        }
        #endregion

        #region PermissionȨ�޹���
        [HttpGet("permissions")]
        public IActionResult Permissions()
        {
            return View();
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
        /// ��ȡ����action
        /// </summary>
        /// <returns></returns>
        public IActionResult GetActions()
        {
            var actions = Common.ActionHandle.GetActions();
            return new JsonResult(actions);
        }
        #endregion
    }
}