using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Web.Common;

namespace Sahiplendir
{
    public class Extensions
    {
        public static PagedListRenderOptions PagedListRenderOptions
        {
            get
            {
                return new PagedListRenderOptions
                {

                    ActiveLiElementClass = "active",
                    UlElementClasses = new[] { "pagination", "pagination-sm" },
                    LiElementClasses = new[] { "page-item" },
                    PageClasses = new[] { "page-link" },
                    ContainerDivClasses = new[] { "d-flex", "justify-content-center", "p-2" },
                };
            }
        }
    }
}
