/*
  * Copyright (c) 2011-2016, Will Strohl
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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using System;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DotNetNuke.Modules.WillStrohlDisqus
{
    [Serializable()]
    public class DisqusInfo : IHydratable
    {

        #region Private Members 

        private DateTime _CreatedAt = Null.NullDate;

        #endregion

        #region Properties

        public int LocalDbId { get; set; }
        public int PortalId { get; set; }
        public int TabId { get; set; }
        public int ModuleId { get; set; }
        public DisqusCursorInfo Cursor { get; set; }
        public int Code { get; set; }
        public DisqusResponseInfo[] Response { get; set; }
        private string SavedResponseFromDatabase { get; set; }
        public DateTime CreatedAt { 
            get { 
                if (_CreatedAt == Null.NullDate && Response != null)
                {
                    _CreatedAt = Response[0].CreatedAt;
                }
                return _CreatedAt;
            }
            set { _CreatedAt = value; }
        }

        #endregion

        #region IHydratable Members

        public void Fill(IDataReader dr)
        {
            LocalDbId = Convert.ToInt32(Null.SetNullInteger(dr["LocalDbId"]));
            PortalId = Convert.ToInt32(Null.SetNullInteger(dr["PortalId"]));
            TabId = Convert.ToInt32(Null.SetNullInteger(dr["TabId"]));
            ModuleId = Convert.ToInt32(Null.SetNullInteger(dr["TabModuleId"]));
            //Code = Convert.ToInt32(Null.SetNull(dr["Code"], Code));
            SavedResponseFromDatabase = Convert.ToString(Null.SetNullString(dr["DisqusComment"]));
            CreatedAt = Convert.ToDateTime(Null.SetNullDateTime(dr["CreatedAt"]));

            LoadRemainingStruct();
        }

        public int KeyID
        {
            get { return LocalDbId; }
            set { LocalDbId = value; }
        }

        #endregion

        private void LoadRemainingStruct()
        {
            XDocument doc = XDocument.Parse(SavedResponseFromDatabase);

            if (doc.Element("Code") != null)
            {
                Code = (int) doc.Element("Code");
            }

            if (doc.Element("Cursor") != null)
            {
                Cursor = new DisqusCursorInfo();

                Cursor.HasNext = (bool)doc.Element("Cursor/HasNext");
                Cursor.HasPrev = (bool)doc.Element("Cursor/HasPrev");
                Cursor.Id = (string)doc.Element("Cursor/Id");
                Cursor.More = (bool)doc.Element("Cursor/More");
                Cursor.Next = (string)doc.Element("Cursor/Next");
                Cursor.Prev = (string)doc.Element("Cursor/Prev");
                Cursor.Total = (string)doc.Element("Cursor/Total");
            }

            if (doc.Element("Response") != null)
            {
                Response = new DisqusResponseInfo[doc.Elements("Response").Count() - 1];

                int i = 0;

                foreach(XElement oElement in doc.Elements("Response"))
                {
                    Response[i] = new DisqusResponseInfo();

                    if (oElement.Element("Author") != null)
                    {
                        Response[i].Author = new DisqusAuthorInfo();

                        Response[i].Author.About = (string) oElement.Element("Author/About");
                        if (oElement.Element("Author/Avatar") != null)
                        {
                            Response[i].Author.Avatar = new DisqusAvatarInfo();
                            Response[i].Author.Avatar.Cache = (string)oElement.Element("Author/Avatar/Cache");
                            Response[i].Author.Avatar.IsCustom = (bool)oElement.Element("Author/Avatar/IsCustom");
                            Response[i].Author.Avatar.Permalink = (string)oElement.Element("Author/Avatar/Permalink");
                        }
                        Response[i].Author.EmailHash = (string)oElement.Element("Author/EmailHash");
                        Response[i].Author.Id = (int)oElement.Element("Author/Id");
                        Response[i].Author.IsAnonymous = (bool)oElement.Element("Author/IsAnonymous");
                        Response[i].Author.IsPrimary = (bool)oElement.Element("Author/IsPrimary");
                        Response[i].Author.JoinedAt = (DateTime)oElement.Element("Author/JoinedAt");
                        Response[i].Author.Location = (string)oElement.Element("Author/Location");
                        Response[i].Author.Name = (string)oElement.Element("Author/Name");
                        Response[i].Author.ProfileUrl = (string)oElement.Element("Author/ProfileUrl");
                        Response[i].Author.Reputation = (long)oElement.Element("Author/Reputation");
                        Response[i].Author.Url = (string)oElement.Element("Author/Url");
                        Response[i].Author.Username = (string)oElement.Element("Author/Username");
                    }

                    Response[i].CreatedAt = (DateTime) oElement.Element("CreatedAt");
                    Response[i].Dislikes = (int) oElement.Element("Dislikes");
                    Response[i].Id = (string) oElement.Element("Id");
                    Response[i].IsApproved = (bool) oElement.Element("IsApproved");
                    Response[i].IsDeleted = (bool) oElement.Element("IsDeleted");
                    Response[i].IsEdited = (bool) oElement.Element("IsEdited");
                    Response[i].IsFlagged = (bool) oElement.Element("IsFlagged");
                    Response[i].IsHighlighted = (bool) oElement.Element("IsHighlighted");
                    Response[i].IsJuliaFlagged = (bool) oElement.Element("IsJuliaFlagged");
                    Response[i].IsSpam = (bool) oElement.Element("IsSpam");
                    Response[i].Likes = (int) oElement.Element("Likes");
                    if(oElement.Element("Media") != null)
                    {
                        Response[i].Media = new string[oElement.Elements("Media").Descendants().Count() - 1];
                        int iMedia = 0;
                        foreach (XElement oMedia in oElement.Element("Media").Descendants())
                        {
                            Response[i].Media[iMedia] = (string) oMedia;
                            iMedia++;
                        }
                    }
                    Response[i].Message = (string) oElement.Element("Message");
                    Response[i].NumReports = (int) oElement.Element("NumReports");
                    Response[i].Parent = (string) oElement.Element("Parent");
                    Response[i].Points = (int) oElement.Element("Points");
                    Response[i].RawMessage = (string) oElement.Element("RawMessage");
                    if (oElement.Element("Thread") != null)
                    {
                        Response[i].Thread = new DisqusThreadInfo();
                        Response[i].Thread.Author = (string) oElement.Element("Thread/Author");
                        Response[i].Thread.Category = (string) oElement.Element("Thread/Category");
                        Response[i].Thread.CreatedAt = (DateTime) oElement.Element("Thread/CreatedAt");
                        Response[i].Thread.Dislikes = (int)oElement.Element("Thread/Dislikes");
                        Response[i].Thread.Forum = (string)oElement.Element("Thread/Forum");
                        Response[i].Thread.Id = (string)oElement.Element("Thread/Id");
                        if(oElement.Element("Thread/Identifiers") != null)
                        {
                            Response[i].Thread.Identifiers = new string[oElement.Elements("Thread/Identifiers").Descendants().Count() - 1];

                            int iIdent = 0;
                            foreach (XElement oIdent in oElement.Element("Thread/Identifiers").Descendants())
                            {
                                Response[i].Thread.Identifiers[iIdent] = (string) oIdent;
                                iIdent++;
                            }
                        }
                        Response[i].Thread.IsClosed = (bool) oElement.Element("IsClosed");
                        Response[i].Thread.IsDeleted = (bool) oElement.Element("IsDeleted");
                        Response[i].Thread.Likes = (int) oElement.Element("Likes");
                        Response[i].Thread.Link = (string) oElement.Element("Link");
                        Response[i].Thread.Message = (string) oElement.Element("Message");
                        Response[i].Thread.Posts = (int) oElement.Element("Posts");
                        Response[i].Thread.Reactions = (int) oElement.Element("Reactions");
                        Response[i].Thread.Slug = (string) oElement.Element("Slug");
                        Response[i].Thread.Title = (string) oElement.Element("Title");
                        Response[i].Thread.UserScore = (int) oElement.Element("UserScore");
                        Response[i].Thread.UserSubscription = (bool) oElement.Element("UserSubscription");
                    }
                    Response[i].Url = (string) oElement.Element("Url");
                    Response[i].UserScore = (int) oElement.Element("UserScore");

                    i++;
                }
            }
        }
    }
}