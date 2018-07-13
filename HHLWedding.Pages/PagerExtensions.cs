using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace HHLWedding.Pages
{
    public static class PagerExtensions
    {
        public static string Pager<T>(PagedList<T> model)
        {
            if (model.TotalRecord == 0)
            {
                return null;
            }

            string link = "";
            link += ("<div class='col-lg-12'>");

            //显示分页信息
            link += (string.Format("<div class='full-left'>&nbsp;&nbsp;第{0}/{1}页 每页<input type='text' id='txtpagesize' style='width:50px;' value='{2}'/>条数据<a href='#' onclick='MCurrentPage()' class='btn btn-primary btn-mini'>确认</a> 共{3}条数据 </div>", model.CurrentPage, model.TotalPage, model.PageSize, model.TotalRecord));

            //计算页码
            int start = model.CurrentPage - 5;
            start = start <= 0 ? 1 : start;
            int end = start + 9;
            end = end >= model.TotalPage ? model.TotalPage : end;


            //页数开始
            link += ("<div class='full-right'><ul class='pagination'>");
            //首页
            link += (string.Format("<li class='previous'><a href='?page={0}' title='首页'>〈</a></li>", 1));
            //往上页数跳转
            if (start >= 2)
            {
                link += (string.Format("<li class='previous'><a href='?page={0}'  title=''>…</a></li>", (model.CurrentPage - 5) > 0 ? (model.CurrentPage - 5) : 1));
            }
            else
            {
                link += ("<li class='previous disabled'><a>…</a></li>");
            }
            //上一页
            if (model.HasPrePage)
            {
                link += (string.Format("<li class='previous'><a href='?page={0}'  title='上一页'>《</a></li>", model.CurrentPage - 1));
            }
            else
            {
                link += ("<li class='previous disabled'><a>《</a></li>");
            }
            //中间页码
            for (int i = start; i <= end; i++)
            {
                if (i == model.CurrentPage)
                {
                    link += (string.Format("<li class='active'><a>{0}</a></li>", i));
                }
                else
                {
                    link += (string.Format("<li class='previous'><a href='?page={0}'  title='{1}'>{0}</a></li>", i, "第" + i + "页"));
                }
            }


            //下一页
            if (model.HasNextPage)
            {
                link += (string.Format("<li class='next'><a href='?page={0}'  title='下一页'>》</a></li>", model.CurrentPage + 1));
            }
            else
            {
                link += ("<li class='next disabled'><a>》</a></li>");
            }
            //往下页数跳转
            if (model.TotalPage >= model.CurrentPage + 5)
            {
                link += (string.Format("<li class='previous'><a href='?page={0}'  title=''>…</a></li>", (model.CurrentPage + 6) >= model.TotalPage ? model.TotalPage : (model.CurrentPage + 6)));
            }
            else
            {
                link += ("<li class='previous disabled'><a>…</a></li>");
            }
            //末页
            link += (string.Format("<li class='previous'><a href='?page={0}'  title='末页'>〉</a></li>", model.TotalPage));
            link += (string.Format("<li class='previous'><input type='text' class='txtpage' id='txtPages' value='{0}' /><li><li  class='previous'><a href='#' onclick='ChangePage()'>跳转</a></li>", model.CurrentPage));
            link += ("</ul></div></div>");
            link += (@"<script type='text/javascript'></script>");
            //return MvcHtmlString.Create(link.ToString());
            return link;

        }

        #region Ajax分页
        /// <summary>
        ///  分页 从1开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="model">已分页的PagedList</param>
        /// <param name="formId">搜索条件form表单id</param>
        /// <param name="contentId">添加分布视图的容器Id</param>
        /// <returns></returns>
        public static string PagerAjax<T>(PagedList<T> model, string formId = "formPageSearch", string contentId = "div_pager")
        {

            if (model.TotalRecord == 0)
                return null;
            //开始拼分页字符串
            string links = "";
            links += ("<div class='row'>");
            links += ("<div class='col-sm-12'>");

            //显示分页信息
            links += (string.Format("<div class='pull-left'>&nbsp;&nbsp;第{0}页/{1}页 共{2}条数据</div>", model.CurrentPage, model.TotalPage, model.TotalRecord));

            // model.CurrentPage  从1开始

            //计算页码
            int start = model.CurrentPage - 2;
            start = start < 1 ? 1 : start;

            int end = model.CurrentPage + 2;
            end = end > model.TotalPage ? model.TotalPage : end;


            //页数开始
            links += ("<ul class='pagination pull-right' style='margin:0; margin-top:-5px;'>");

            //前一页
            if (model.HasPrePage && model.CurrentPage > 1)
            {
                links += (string.Format("<li class='prev'><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='上一页'>«</a></li>", model.CurrentPage - 1, formId, contentId));
            }
            else
            {
                links += ("<li class='prev disabled'><a>«</a></li>");
            }
            //总页码小于五页 直接输出
            if (model.TotalPage <= 5)
            {
                for (int i = 1; i <= model.TotalPage; i++)
                {
                    if (i == model.CurrentPage)
                    {
                        links += (string.Format("<li class='active'><a>{0}</a></li>", i));
                    }
                    else
                    {
                        links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")'>{0}</li>", i, formId, contentId));
                    }
                }
            }
            else
            {
                //最多显示五页
                if (model.CurrentPage <= 3)
                {
                    for (int i = start; i <= 5; i++)
                    {
                        if (i == model.CurrentPage)
                        {
                            links += (string.Format("<li class='active'><a>{0}</a></li>", i));
                        }
                        else
                        {
                            links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")'>{0}</li>", i, formId, contentId));
                        }
                    }
                }
                else
                {
                    //显示首页
                    if (model.CurrentPage > 4)
                    {
                        links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='首页'>{0}</li>", 1, formId, contentId));
                    }

                    int _start = start;
                    if ((end - start) <= 5)
                    {
                        //最后不足五页的补全五页
                        _start = end - 4;
                    }

                    //显示前面...
                    links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='第{0}页'>...</li>", _start - 1, formId, contentId));

                    for (int i = _start; i <= end; i++)
                    {
                        if (i == model.CurrentPage)
                        {
                            links += (string.Format("<li class='active'><a>{0}</a></li>", i));
                        }
                        else
                        {
                            links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")'>{0}</li>", i, formId, contentId));
                        }
                    }
                }
                //显示后面...

                if (model.CurrentPage <= model.TotalPage - 3 && model.CurrentPage < 3)
                {
                    links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='第{0}页'>...</li>", end + 4 - model.CurrentPage, formId, contentId));
                }
                else if (model.CurrentPage <= model.TotalPage - 3)
                {
                    links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='第{0}页'>...</li>", end + 1, formId, contentId));
                }
                //显示尾页
                if (model.TotalPage > 5 && model.CurrentPage <= model.TotalPage - 3)
                {
                    links += (string.Format("<li><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='尾页'>{0}</li>", model.TotalPage, formId, contentId));
                }
            }

            //显示下一页
            if (model.HasNextPage)
            {
                links += (string.Format("<li class='next'><a href='javascript:void(0);' onclick='hhl.ajaxPartial({0},\"{1}\",\"{2}\")' data-toggle='tooltip' title='下一页'>»</a></li>", model.CurrentPage + 1, formId, contentId));
            }
            else
            {
                links += ("<li class='next disabled'><a>»</a></li>");
            }

            //页数结束
            links += ("</ul></div></div>");

            //return MvcHtmlString.Create(links.ToString());
            return links;
        }

        #endregion

    }
}
