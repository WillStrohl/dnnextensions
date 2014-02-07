using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.WillStrohlDisqus
{
    public class DisqusThreadInfo
    {
        public string Category { get; set; }
        public int Reactions { get; set; }
        public string[] Identifiers { get; set; }
        public string Forum { get; set; }
        public string Title { get; set; }
        public int Dislikes { get; set; }
        public bool IsDeleted { get; set; }
        public string Author { get; set; }
        public int UserScore { get; set; }
        public string Id { get; set; }
        public bool IsClosed { get; set; }
        public int Posts { get; set; }
        public bool UserSubscription { get; set; }
        public string Link { get; set; }
        public int Likes { get; set; }
        public string Message { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}