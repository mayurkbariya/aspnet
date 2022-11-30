﻿using System;

 namespace FBDropshipper.Domain.Interfaces
{
    public interface IBase
    {
        /// <summary>
        /// The Date it was created
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// The Date it was Updated
        /// </summary>
        DateTime UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}