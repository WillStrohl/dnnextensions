/*
 * Copyright (c) 2011-2012, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this 
 * list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * Neither the name of Will Strohl, Disqus Module, nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior 
 * written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace DotNetNuke.Modules.WillStrohlDisqus
{
    [Serializable()]
    public class DisqusResponseInfo
    {
        public int Dislikes { get; set; }
        public int NumReports { get; set; }
        public int Likes { get; set; }
        public string Message { get; set; }
        public bool IsSpam { get; set; }
        public DateTime CreatedAt { get; set; }
        public DisqusAuthorInfo Author { get; set; }
        public string[] Media { get; set; }
        public int UserScore { get; set; }
        public string Id { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsJuliaFlagged { get; set; }
        public string Parent { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFlagged { get; set; }
        public string RawMessage { get; set; }
        public DisqusThreadInfo Thread { get; set; }
        public string Url { get; set; }
        public int Points { get; set; }
        public bool IsEdited { get; set; }
    }
}