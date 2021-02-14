﻿using System;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedAndSortedResultRequestByPageDto : PagedResultRequestByPageDto, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}
