﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using 上海燕洵物联网科技有限公司人事管理系统.Models;
namespace 上海燕洵物联网科技有限公司人事管理系统.Controllers
{
    [ShouQuan]
   [Authorize]
    public class FinanceController : Controller
    {
        // GET: Finance
        /// <summary>
        /// 获取所有员工的工资详细
        /// </summary>
        /// <returns>list集合</returns>
        public ActionResult GetAllMoney(int pageindex=1)
        {
            var result = HttpClientHelper.Seng("get", "api/Finance/GetAllMoney",null);
            var list = JsonConvert.DeserializeObject<List<PaymessageViewModel>>(result);
            ViewBag.currentindex = pageindex;
            ViewBag.totaldata = list.Count();
            ViewBag.totalpage = Math.Floor((list.Count() * 1.0) / 5)+1;
            return View(list.Skip((pageindex - 1) * 5).Take(5).ToList());
        }
        /// <summary>
        /// 获取图表信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChartsMoney()
        {
            var result = HttpClientHelper.Seng("get", "api/Finance/GetAllMoney", null);
            return Content(result);
        }
        /// <summary>
        /// 显示职工信息
        /// </summary>
        /// <returns>list集合</returns>

        public ActionResult ShowMoney()
        {
           var list = HttpClientHelper.Seng("get", "api/Finance/Emps", null);
           var result=  JsonConvert.DeserializeObject<List<TempFinanceViewModel>>(list);
            var theOne= result.Where(a=>a.Ename==Session["Name"].ToString()).FirstOrDefault();
            return View(theOne);
        }
        /// <summary>
        /// 请假页面显示
        /// </summary>
        /// <returns>int</returns>
        [HttpGet]
        public ActionResult Vacatefinance()
        {
            return View();
        }

        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="vacate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Vacatefinance(VacateViewModel vacate)
        {
            vacate.VacateState = 1;
            vacate.EmpsId = Convert.ToInt32(Session["EmpsId"]);
            string str = JsonConvert.SerializeObject(vacate);
            string result = HttpClientHelper.Seng("post", "api/Finance/Vacatefinance",str);
            if (result.Contains("成功"))
            {
                return Content("提交成功,等待审核");
            }
            else
            {
                return Content("提交失败,等待审核");
            }
        }

        /// <summary>
        /// 显示离职页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LiZhi()
        {

            return View();
        }
        /// <summary>
        /// 接收离职信息
        /// </summary>
        /// <param name="dimission"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LiZhi(DimissionViewModel dimission,string[] reason,string name)
        {
            var list = HttpClientHelper.Seng("get", "api/Finance/Emps", null);
            var result = JsonConvert.DeserializeObject<List<TempFinanceViewModel>>(list);
            dimission.EmpsId = result.Where(a=>a.Ename==name).FirstOrDefault().Id;
            var str= JsonConvert.SerializeObject(dimission);
            string result1 = HttpClientHelper.Seng("post", "api/Finance/Dimission",str);
            if (result1.Contains("成功"))
            {
                return Content("离职成功");
            }
            else
            {
                return Content("离职失败");
            }
        }
    }
}