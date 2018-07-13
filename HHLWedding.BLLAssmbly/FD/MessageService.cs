using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using HHLWedding.ToolsLibrary;
using EntityFramework.Extensions;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly.FD
{
    public class MessageService : ICRUDInterface<sm_Message>
    {
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-12
        /// @desc:消息
        /// </summary>
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        /// <summary>
        /// 根据主键获取消息
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public sm_Message GetByID(int? KeyID)
        {
            return ObjEntity.sm_Message.Find(KeyID);
        }

        public List<sm_Message> GetByAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 单独添加添加消息 
        /// </summary>
        /// <param name="ObjectT">消息实体类</param>
        /// <returns></returns>
        public int Insert(sm_Message ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.sm_Message.Add(ObjectT);
                return ObjEntity.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 批量添加消息
        /// </summary>
        /// <param name="list">消息集合</param>
        /// <returns></returns>
        public int InsertFroList(List<sm_Message> list)
        {
            if (list != null)
            {
                ObjEntity.sm_Message.AddRange(list);
                int result = ObjEntity.SaveChanges();
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 修改消息
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Update(sm_Message ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.SaveChanges();
            }
            return 0;
        }

        public int Delete(sm_Message ObjectT)
        {
            throw new NotImplementedException();
        }

        public List<sm_Message> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<sm_Message>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }

        /// <summary>
        /// 获取所有消息  type 1.收件箱  2.发件箱
        /// </summary>
        /// <returns></returns>
        public List<sm_Message> GetAllMessage(int EmployeeId, int type = 1, int isType = 0, string search = "")
        {
            if (type == 1)          //收件箱
            {
                var query = ObjEntity.sm_Message.Where(c => EmployeeId.ToString().Contains(c.ToEmployee) && c.SendType == type);
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.MessageTitle.Contains(search) || c.MessageContent.Contains(search));
                }
                //重要邮件
                if (isType == 1)
                {
                    return query.Where(c => c.IsGarbage == isType).OrderBy(c => c.SendDateTime).OrderByDescending(c => c.SendDateTime).ToList();
                }
                else
                {
                    return query.OrderBy(c => c.SendDateTime).OrderByDescending(c => c.SendDateTime).ToList();
                }
            }
            else
            {
                var querys = ObjEntity.sm_Message.Where(c => c.FromEmployee == EmployeeId && c.SendType == type);
                if (!string.IsNullOrEmpty(search))
                {
                    querys = querys.Where(c => c.MessageTitle.Contains(search) || c.MessageContent.Contains(search));
                }

                //草稿
                if (isType == 1)
                {
                    return querys.Where(c => c.IsDraft == isType).OrderBy(c => c.SendDateTime).OrderByDescending(c => c.SendDateTime).ToList();
                }
                else
                {
                    return querys.OrderBy(c => c.SendDateTime).OrderByDescending(c => c.SendDateTime).ToList();
                }


            }
        }

        /// <summary>
        /// 根据Id删除消息
        /// </summary>
        /// <param name="mid">拼接而成的消息Id</param>
        /// <returns></returns>
        public int DeleteMsg(string mid)
        {
            return ObjEntity.sm_Message.Where(c => mid.Contains(c.MessageId.ToString())).Delete();
        }


        /// <summary>
        /// 获取收件相对应的发件信息 同时修改状态
        /// </summary>
        /// <param name="sendDate"></param>
        /// <returns></returns>
        public sm_Message GetSendMsgByDate(DateTime? sendDate, int EmployeeId, int fromEmployee)
        {
            sm_Message query = ObjEntity.sm_Message.FirstOrDefault(c => c.SendDateTime == sendDate && c.ToEmployee == EmployeeId.ToString() && c.FromEmployee == fromEmployee && c.SendType == 2);
            return query;
        }


        /// <summary>
        /// 获取上一封邮件
        /// </summary>
        /// <param name="mid">邮箱ID</param>
        /// <param name="SendType">类型 1.收件  2.发件</param>
        /// <returns></returns>
        ///
        public sm_Message GetPreMsg(int mid, int EmployeeId, int sendType)
        {
            if (sendType == 1)
            {
                return ObjEntity.sm_Message.Where(c => c.MessageId > mid && c.ToEmployee == EmployeeId.ToString() && c.SendType == sendType).OrderBy(c => c.SendDateTime).OrderBy(c => c.MessageId).ToList().FirstOrDefault();
            }
            else
            {
                List<sm_Message> list = ObjEntity.sm_Message.Where(c => c.MessageId > mid && c.FromEmployee == EmployeeId && c.SendType == sendType).OrderBy(c => c.SendDateTime).OrderBy(c => c.MessageId).ToList();
                return list.FirstOrDefault();
            }
        }


        /// <summary>
        /// 获取下一封邮件
        /// </summary>
        /// <param name="mid">邮箱ID</param>
        /// <param name="SendType">类型 1.收件  2.发件</param>
        /// <returns></returns>
        ///
        public sm_Message GetNextMsg(int mid, int EmployeeId, int sendType)
        {
            if (sendType == 1)
            {
                return ObjEntity.sm_Message.Where(c => c.MessageId < mid && c.ToEmployee == EmployeeId.ToString() && c.SendType == sendType).OrderByDescending(c => c.SendDateTime).ThenByDescending(c => c.MessageId).ToList().FirstOrDefault();
            }
            else
            {
                return ObjEntity.sm_Message.Where(c => c.MessageId < mid && c.FromEmployee == EmployeeId && c.SendType == sendType).OrderByDescending(c => c.SendDateTime).ThenByDescending(c => c.MessageId).ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取未读邮件条数
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public int GetNoReadMsg(int EmployeeId)
        {
            var smList = ObjEntity.sm_Message.Where(c => c.ToEmployee == EmployeeId.ToString() && c.SendType == 1 && c.IsRead == 0);
            if (smList != null)
            {
                return smList.FutureCount();
            }
            return 0;
        }


        /// <summary>
        /// 获取草稿数量
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public int GetDraftCount(int EmployeeId)
        {
            var smList = ObjEntity.sm_Message.Where(c => c.FromEmployee == EmployeeId && c.SendType == 2 && c.IsDraft == 1);
            if (smList != null)
            {
                return smList.FutureCount();
            }
            return 0;
        }

        public List<sm_Message> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<sm_Message, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
