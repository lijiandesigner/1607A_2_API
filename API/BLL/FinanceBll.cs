﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
namespace BLL
{
    public class FinanceBll
    {
        FinanceDal FinanceDal = new FinanceDal();
        ManagerDal Managerdal = new ManagerDal();
        /// <summary>
        /// 显示所有员工工资
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TempPaymessage> GetAllMoney()
        {
            return FinanceDal.GetAllMoney();
        }

        /// <summary>
        /// 请假方法
        /// </summary>
        /// <returns></returns>
        public int Vacatefinance(Vacate vacate)
        {
            return FinanceDal.Vacatefinance(vacate);
        }

        /// <summary>
        /// 提交离职
        /// </summary>
        /// <param name="dimission"></param>
        /// <returns></returns>
        public int Dimission(Dimission dimission)
        {
            int result=FinanceDal.Dimission(dimission);
            if (result>0)
            {
                Managerdal.DeleteEmp(dimission.EmpsId);
            }
            return result;
        }
    }
}
