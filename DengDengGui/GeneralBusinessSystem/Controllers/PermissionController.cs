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
    public class PermissionController : BaseController
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
            try
            {
                var permissions = _permissionRepository.GetPermissions();
                var actions = Common.ActionHandle.GetActions("basecontroller");
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
                return new JsonResult(new { result = 1, message = "��ȡ����action�ɹ�", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ȡ����action��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ȡ����action��{exc.Message}" });
            }
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
        /// ��ѯȫ���û�
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public ActionResult GetUsers(string queryName)
        {
            try
            {
                if (string.IsNullOrEmpty(queryName))
                {
                    var list = _permissionRepository.GetUsers();
                    return new JsonResult(new { result = 1, message = "��ѯȫ���û��ɹ�", data = list }, new JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
                else
                {
                    var list = _permissionRepository.GetUsers(queryName);
                    return new JsonResult(new { result = 1, message = $"��{queryName}��ѯ�û��ɹ�", data = list }, new JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ѯȫ���û���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ѯȫ���û���{exc.Message}" });
            }
        }

        /// <summary>
        /// ����û�
        /// </summary>
        /// <param name="userName">�û���</param>
        /// <param name="password">����</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public IActionResult UserAdd(string userName, string password, string name)
        {
            try
            {
                // _permissionRepository.AddUser(userName, password, name, CompanyID);
                _permissionRepository.AddUser(userName, password, name, 1);
                return new JsonResult(new { result = 1, message = "����û��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"����û���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"����û���{exc.Message}" });
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
        [HttpPut("modifyuser")]
        public IActionResult UserModify(int ID, string userName, string password, string name)
        {
            try
            {
                _permissionRepository.ModifyUser(ID, userName, password, name);
                return new JsonResult(new { result = 1, message = "�޸��û��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"�޸��û���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"�޸��û���{exc.Message}" });
            }
        }
        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("deleteuser")]
        public IActionResult UserDelete(int ID)
        {

            try
            {
                _permissionRepository.RemoveUser(ID);
                return new JsonResult(new { result = 1, message = "ɾ���û��ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"ɾ���û���{exc.Message}");
                return new JsonResult(new { result = 0, message = $"ɾ���û���{exc.Message}" });
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
            try
            {
                var list = _permissionRepository.GetRoles();
                return new JsonResult(new { result = 1, message = $"��ѯȫ����ɫ", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ѯȫ����ɫ��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ѯȫ����ɫ��{exc.Message}" });
            }
        }

        /// <summary>
        /// ��ӽ�ɫ
        /// </summary>
        /// <param name="rolename">��ɫ����</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public IActionResult AddRole(string name)
        {
            try
            {
                //_permissionRepository.AddRole(name, CompanyID);
                _permissionRepository.AddRole(name, 1);
                return new JsonResult(new { result = 1, message = "��ӽ�ɫ�ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ӽ�ɫ��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ӽ�ɫ��{exc.Message}" });
            }
        }
        /// <summary>
        /// �޸Ľ�ɫ 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="rolename">��ɫ����</param>
        /// <returns></returns>
        [HttpPut("modifyrole")]
        public IActionResult ModifyRole(int id, string name)
        {
            try
            {
                _permissionRepository.ModifyRole(id, name);
                return new JsonResult(new { result = 1, message = "�޸Ľ�ɫ�ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"�޸Ľ�ɫ��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"�޸Ľ�ɫ��{exc.Message}" });
            }
        }
        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpDelete("deleterole")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _permissionRepository.RemoveRole(id);
                return new JsonResult(new { result = 1, message = "ɾ����ɫ�ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"ɾ����ɫ��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"ɾ����ɫ��{exc.Message}" });
            }
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
            try
            {
                var list = _permissionRepository.GetPermissions();
                return new JsonResult(new { result = 1, message = $"��ȡȫ��Ȩ�޳ɹ�", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"��ȡȫ��Ȩ�ޣ�{exc.Message}");
                return new JsonResult(new { result = 0, message = $"��ȡȫ��Ȩ�ޣ�{exc.Message}" });
            }
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
                return new JsonResult(new { result = 1, message = "ɾ��Ȩ�޳ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"ɾ��Ȩ�ޣ�{exc.Message}");
                return new JsonResult(new { result = 0, message = $"ɾ��Ȩ�ޣ�{exc.Message}" });
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
                _permissionRepository.AddPermission(action, actiondescription, controllername, predicate, 1);
                // _permissionRepository.AddPermission(action, actiondescription, controllername, predicate, CompanyID);
                return new JsonResult(new { result = 1, message = "���Ȩ�޳ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"���Ȩ�ޣ�{exc.Message}");
                return new JsonResult(new { result = 0, message = $"���Ȩ�ޣ�{exc.Message}" });
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
            try
            {
                var list = _permissionRepository.GetPermissionsByRoleID(roleID);
                return new JsonResult(new { result = 1, message = $"����ɫID:{roleID}��ѯȨ�޳ɹ�", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"����ɫID:{roleID}��ѯȨ�ޣ�{ exc.Message}");
                return new JsonResult(new { result = 0, message = $"����ɫID:{roleID}��ѯȨ�ޣ�{ exc.Message}" });
            }
        }
        /// <summary>
        /// ���������ɫȨ��
        /// </summary>
        /// <param name="roleID">��ɫID</param>
        /// <param name="rolePermissions">��ɫȨ��</param>
        /// <returns></returns>
        [HttpPost("savarolepermissons")]
        public IActionResult SavaRolePermissions(int roleid, List<Model.ViewModel.RolePermission> rolepermissions)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(rolepermissions);
                _permissionRepository.SavaRolePermissions(roleid, list);
                ReLoadPermissions();
                return new JsonResult(new { result = 1, message = "���������ɫȨ�޳ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"���������ɫȨ�ޣ�{exc.Message}");
                return new JsonResult(new { result = 0, message = $"���������ɫȨ�ޣ�{exc.Message}" });
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
            try
            {
                var list = _permissionRepository.GetRoleByUserID(userID);
                return new JsonResult(new { result = 1, message = $"���û�ID:{userID}��ѯ��ɫ�ɹ�", data = list }, new JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"���û�ID:{userID}��ѯ��ɫ��{ exc.Message}");
                return new JsonResult(new { result = 0, message = $"���û�ID:{userID}��ѯ��ɫ��{ exc.Message}" });
            }
        }
        /// <summary>
        /// ���������û���ɫ
        /// </summary>
        /// <param name="userID">�û�ID</param>
        /// <param name="userroles">�û���ɫ</param>
        /// <returns></returns>
        [HttpPost("savauserroles")]
        public IActionResult SavaUserRoles(int userid, List<Model.ViewModel.UserRole> userroles)
        {
            try
            {
                var list = new List<dynamic>();
                list.AddRange(userroles);
                _permissionRepository.SavaUserRoles(userid, list);
                return new JsonResult(new { result = 1, message = "���������û���ɫ�ɹ�" });
            }
            catch (Exception exc)
            {
                _log.Log(NLog.LogLevel.Error, $"���������û���ɫ��{exc.Message}");
                return new JsonResult(new { result = 0, message = $"���������û���ɫ��{exc.Message}" });
            }
        }
        #endregion

    }
}