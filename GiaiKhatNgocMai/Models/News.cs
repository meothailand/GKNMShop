//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GiaiKhatNgocMai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] NewsAvatar { get; set; }
        public bool IsHotNew { get; set; }
        public bool IsPublic { get; set; }
        public System.DateTime DatePosted { get; set; }
        public int PostedBy { get; set; }
        public string UrlTitle { get; set; }
    
        public virtual User User { get; set; }
    }
}