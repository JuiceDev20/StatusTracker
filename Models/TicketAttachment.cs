﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static StatusTracker.Extensions.CustomAttributes;

namespace StatusTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string UserId { get; set; }
        public virtual STUser User { get; set; }
    
        [Display(Name = "Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2* 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })]
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; } 
        public string ContentType { get; set; }  //Added to store what type of file it was.


    }
}