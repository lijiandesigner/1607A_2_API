﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using 上海燕洵物联网科技有限公司人事管理系统.Models;

namespace 上海燕洵物联网科技有限公司人事管理系统.Controllers
{
    [ShouQuan]
   [Authorize]
    public class AttendanceController : Controller
    {
        // GET: Attendance
        /// <summary>
        /// 获取所有的打卡信息
        /// </summary>
        /// <returns>list集合</returns>
        [Authorize(Users = "华家铮")]
        public ActionResult GetAllAttend(int pageindex=1)
        {
            string json=HttpClientHelper.Seng("get", "api/AttendanceAPI/GetAllAttendance",null);
            var pun = JsonConvert.DeserializeObject<List<PunchcardViewModel>>(json);
            ViewBag.currentindex = pageindex;
            ViewBag.totaldata = pun.Count();
            ViewBag.totalpage = Math.Floor((pun.Count() * 1.0) / 5)+1;
            return View(pun.Skip((pageindex - 1) * 5).Take(5).ToList());
        }
        /// <summary>
        /// 打卡
        /// </summary>
        /// <returns>int</returns>
        [HttpGet]
        public ActionResult Punchcard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Punchcard(int Id)
        {
            string result;
            string Showjson = HttpClientHelper.Seng("get", "api/AttendanceAPI/GetAllAttendance", null);
            List<PunchcardViewModel> punchcards = JsonConvert.DeserializeObject<List<PunchcardViewModel>>(Showjson);
            var pun = punchcards.Where(c => c.EmpsId == Id&&(Convert.ToDateTime(c.Signindate).ToShortDateString()==DateTime.Now.ToShortDateString()||Convert.ToDateTime(c.Signoutdate).ToShortDateString()==DateTime.Now.ToShortDateString())).FirstOrDefault();
            if (pun == null)
            {
                PunchcardViewModel punchcard = new PunchcardViewModel() { EmpsId = Id, Signindate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string json = JsonConvert.SerializeObject(punchcard);
                result = HttpClientHelper.Seng("post", "api/AttendanceAPI/Punchcard", json);
                if (result.Contains("成功"))
                {
                    return Content("打卡成功");
                }
                else
                {
                    return Content("打卡失败");
                }
            }
            else if(pun.Signoutdate==null)
            {
                pun.Signoutdate = DateTime.Now.ToString();
                string json = JsonConvert.SerializeObject(pun);
                result = HttpClientHelper.Seng("put", "api/AttendanceAPI/UptPunchcard", json);
                if (result.Contains("成功"))
                {
                    return Content("打卡成功");
                }
                else
                {
                    return Content("打卡失败");
                }
            }
            else
            {
                return Content("你已打过卡,不能重新打卡");
            }            
        }
        public ActionResult GetChartsAttend()
        {
            string json = HttpClientHelper.Seng("get", "api/AttendanceAPI/GetAllAttendance", null);
            return Content(json);
        }
        /// <summary>
        /// 显示个人工资方法
        /// </summary>
        /// <returns>list集合</returns>
        public ActionResult ShowMoney(int Id)
        {
            string json = HttpClientHelper.Seng("get", "api/AttendanceAPIController/ShowMoney?Id="+Id,null);
            return View(JsonConvert.DeserializeObject<PaymessageViewModel>(json));
        }
        /// <summary>
        /// 请假方法
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult VacateAttendance()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VacateAttendance(VacateViewModel vacate)
        {
            vacate.VacateState = 1;
            string json = JsonConvert.SerializeObject(vacate);
            string result = HttpClientHelper.Seng("post","api/AttendanceAPIController/VacateAttendance",json);
            if (result.Contains("成功"))
	        {
                return Content("提交成功");
	        }
            else
	        {
                return Content("提交失败");
	        }
        }
    }
}