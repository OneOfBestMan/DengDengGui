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

        #region User����
        [Route("users")]
        public ActionResult UserIndex()
        {
            return View(_businessRepository.GetUsers());
        }
        [Route("queryusers")]
        public ActionResult QueryUser(string queryName)
        {
            return new JsonResult(_businessRepository.GetUsers(queryName));
        }

        [HttpPost]
        [Route("adduser")]
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


        [HttpPost]
        [Route("modifyuser")]
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

        [HttpPost]
        [Route("deleteuser")]
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


        #region Role����
        /// <summary>
        /// ��ӽ�ɫ 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRole()
        {
            return View();
        }
        #endregion
    }
}