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
using Microsoft.AspNetCore.Http;
using GeneralBusinessSystem.Middleware;

namespace GeneralBusinessSystem.Controllers
{
    public class PermissionController : GBController
    {
        /// <summary>
        /// Ȩ�޲ִ�����
        /// </summary>
        IPermissionRepository _permissionRepository;

        /// <summary>
        /// ʵ����ManagementController
        /// </summary>
        /// <param name="businessRepository">ҵ��ִ���</param>

        public PermissionController(IBusinessRepository businessRepository, IPermissionRepository permissionRepository) : base(businessRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ��ȡ����action,Ϊ���οؼ�������Դ
        /// </summary>
        /// <returns></returns>
        [HttpGet("allaction")]
        public IActionResult GetActions()
        {
            var permissions = _permissionRepository.GetPermissions();
            var actions = Common.ActionHandle.GetActions();
            var list = new List<dynamic>();
            foreach (var groupItem in actions.GroupBy(s => s.ControllerName))
            {
                var node = new { name = groupItem.Key, open = true, children = new List<dynamic>() };
                foreach (var action in actions)
                {
                    if (groupItem.Key == action.ControllerName)
                    {
                        var permission = permissions.SingleOrDefault(s => s["Action"] == action.ActionName && s["ControllerName"] == action.ControllerName && s["Predicate"] == action.Predicate.ToString());
                        if (permission == null)
                        {
                            node.children.Add(new { name = $"{action.ActionName}��{action.Predicate}��" });
                        }
                        else
                        {
                            node.children.Add(new { name = $"{action.ActionName}��{action.Predicate}��", chkDisabled = true });
                        }
                    }
                }
                list.Add(node);
            }
            return new JsonResult(list);
        }

        #region User �û�����
        /// <summary>
        /// �û�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// ��ȡȫ���û�
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public ActionResult GetUsers()
        {
            var result = _permissionRepository.GetUsers();
            return new JsonResult(result, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }
        /// <summary>
        /// ��ѯ�û�
        /// </summary>
        /// <param name="queryName">��ѯ����</param>
        /// <returns></returns>
        [HttpGet("queryusers")]
        public ActionResult QueryUser(string queryName)
        {
            var list = _permissionRepository.GetUsers(queryName);
            return new JsonResult(list, new JsonSerializerSettings()
            {

                ContractResolver = new LowercaseContractResolver()
            });
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
                _permissionRepository.AddUser(userName, password, name, CompanyID);

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

                _permissionRepository.ModifyUser(ID, userName, password, name);
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
                _permissionRepository.RemoveUser(ID);
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
        /// ��ɫ����
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public IActionResult Roles()
        {
            return View();
        }
        /// <summary>
        /// ��ѯȫ����ɫ
        /// </summary>
        /// <returns></returns>
        [HttpGet("getroles")]
        public IActionResult GetRoles()
        {
            var list = _permissionRepository.GetRoles();
            return new JsonResult(list, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }

        /// <summary>
        /// ��ӽ�ɫ
        /// </summary>
        /// <param name="rolename">��ɫ����</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public bool AddRole(string rolename)
        {
            return _permissionRepository.AddRole(rolename,CompanyID) > 0 ? true : false;
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
            return _permissionRepository.ModifyRole(id, rolename) > 0 ? true : false;
        }
        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost("deleterole")]
        public bool DeleteRole(int id)
        {
            return _permissionRepository.RemoveRole(id) > 0 ? true : false;
        }
        #endregion

        #region PermissionȨ�޹���
        [HttpGet("permissions")]
        public IActionResult Permissions()
        {
            return View();
        }
        /// <summary>
        /// ��ȡȫ��Ȩ��
        /// </summary>
        /// <returns></returns>
        [HttpGet("getpermissions")]
        public IActionResult GetPermissions()
        {
            var permissions = _permissionRepository.GetPermissions();
            return new JsonResult(permissions, new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            });
        }
        /// <summary>
        /// ɾ��Ȩ��
        /// </summary>
        /// <param name="id">Ȩ��ID</param>
        /// <returns></returns>
        [HttpDelete("removepermission")]
        public IActionResult RemovePermission(int id)
        {
            try
            {
                var result = _permissionRepository.RemovePermission(id);
                return new JsonResult(new { result = result > 0 ? true : false });
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = false, message = exc.Message });
            }
        }

        /// <summary>
        /// ���Ȩ��
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="actiondescription">action����</param>
        /// <param name="controllername">controller</param>
        /// <param name="predicate">ν��</param>
        /// <returns></returns>
        [HttpPost("addpermission")]
        public dynamic AddPermission(string action, string actiondescription, string controllername, string predicate)
        {
            try
            {
                var result = _permissionRepository.AddPermission(action, actiondescription, controllername, predicate,CompanyID);    
                return new { result = result > 0 ? true : false };
            }
            catch (Exception exc)
            {
                return new { result = false, message = exc.Message };
            }
        }




        #endregion

        #region ��ɫȨ�޹���
        /// <summary>
        /// ��ɫȨ������
        /// </summary>
        /// <returns></returns>
        [HttpGet("rolepermission")]
        public IActionResult RolePermission()
        {
            return View();
        }
        /// <summary>
        /// ����ɫID��ѯȨ��
        /// </summary>
        /// <param name="roleID">��ɫID</param>
        /// <returns></returns>
        [HttpGet("getpermission")]
        public IActionResult GetPermissionByRoleID(int roleID)
        {
            var list = _permissionRepository.GetPermissionsByRoleID(roleID);
            return new JsonResult(list, new JsonSerializerSettings()
            {

                ContractResolver = new LowercaseContractResolver()
            });
        }
        /// <summary>
        /// ���������ɫȨ��
        /// </summary>
        /// <param name="roleID">��ɫID</param>
        /// <param name="rolePermissions">��ɫȨ��</param>
        /// <returns></returns>
        [HttpPost("savarolepermissons")]
        public dynamic SavaRolePermissions(int roleid, List<Model.ViewModel.RolePermission> rolepermissions)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(rolepermissions);
                var result = _permissionRepository.SavaRolePermissions(roleid, list);
                ReLoadPermissions();
                return new { result = result };
            }
            catch (Exception exc)
            {
                return new { result = false, message = exc.Message };
            }
        }
        /// <summary>
        /// ���¼���Ȩ��
        /// </summary>
        void ReLoadPermissions()
        {
            PermissionMiddleware._userPermissions.Clear();
            PermissionMiddleware._userPermissions = new List<dynamic>();
            foreach (var dic in _permissionRepository.GetUserPermissions())
            {
                PermissionMiddleware._userPermissions.Add(new
                {
                    UserName = dic["UserName"],
                    Action = dic["Action"]
                });
            }
        }
        #endregion

        #region �û���ɫ����
        /// <summary>
        /// �û���ɫ����
        /// </summary>
        /// <returns></returns>
        [HttpGet("userrole")]
        public IActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// ���û�ID��ѯ��ɫ
        /// </summary>
        /// <param name="userID">�û�ID</param>
        /// <returns></returns>
        [HttpGet("getrole")]
        public IActionResult GetRoleByUserID(int userID)
        {
            var list = _permissionRepository.GetRoleByUserID(userID);
            return new JsonResult(list, new JsonSerializerSettings()
            {

                ContractResolver = new LowercaseContractResolver()
            });
        }
        /// <summary>
        /// ���������û���ɫ
        /// </summary>
        /// <param name="userID">�û�ID</param>
        /// <param name="userroles">�û���ɫ</param>
        /// <returns></returns>
        [HttpPost("savauserroles")]
        public dynamic SavaUserRoles(int userid, List<Model.ViewModel.UserRole> userroles)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(userroles);
                var result = _permissionRepository.SavaUserRoles(userid, list);
                return new { result = result };
            }
            catch (Exception exc)
            {
                return new { result = false, message = exc.Message };
            }
        }
        #endregion

    }
}