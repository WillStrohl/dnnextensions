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

using DotNetNuke.Entities.Portals;
using DotNetNuke.Modules.WillStrohlDisqus.Components;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace DotNetNuke.Modules.WillStrohlDisqus
{
    public class ImportDisqusComments : SchedulerClient
    {

        #region Constants

        private const string APPLICATION_NAME = "wnsDisqusApplicationName";
        private const string API_SECRET = "wnsDisqusApiSecret";
        private const string MESSAGE_PROCESSING = "<br />Processing Portal ({0}): {1}";
        private const string MESSAGE_CALLED = "<br />Called: {0}";
        private const string MESSAGE_DISQUS_COMMENTS = "<br />Disqus returned {0} comments.";
        private const string MESSAGE_DELETE_COMMENTS = "<br />Deleting original comments.";
        private const string MESSAGE_LSTRESPONSE_COMMENTS = "<br />lstResponse has {0} comments";
        private const string MESSAGE_OPOST_COMMENTS = "<br />oPosts has {0} comments";
        private const string MESSAGE_OHEADER_COMMENTS = "<br />oHeader has {0} comments";
        private const string MESSAGE_SAVING_COMMENT = "<br />Saving comment #{0}";
        private const string MESSAGE_IDENTIFIER = "<br />Indentifier == {0}";
        private const string MESSAGE_COMMENT_SAVED = "<br />Comment #{0} saved";
        private const string MESSAGE_COMMENT_SUMMARY = "<br />{0} had {1} comments processed.";
        private const string MESSAGE_UNEXPECTED_ERROR = "<br />UNEXPECTED EXCEPTION OCCURRED!!! {0}<br />{1}";
        private const string ERROR_MESSAGE_1 = "<br />ERROR occured saving comment #{0}";
        private const string ERROR_MESSAGE_2 = "<br />PortalId: {0} TabId: {1} TabModuleId: {2} RawUrl: {3} CreatedAt: {4}";
        private const string ERROR_MESSAGE_3 = "<br />ERROR DETAILS {0}<br />{1}";

        #endregion

        public ImportDisqusComments(ScheduleHistoryItem oItem) : base()
        {
            ScheduleHistoryItem = oItem;
        }

        public override void DoWork()
        {
            try
            {
                //Perform required items for logging
                Progressing();

                ImportCommentsFromDisqus();

                //To log note
                //this.ScheduleHistoryItem.AddLogNote("note");

                //Show success
                ScheduleHistoryItem.Succeeded = true;
            }
            catch (Exception ex)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote("Exception= " + ex.ToString());
                Errored(ref ex);
                Exceptions.LogException(ex);
            }
        }

        private void ImportCommentsFromDisqus()
        {
            PortalController ctlPortal = new PortalController();
            ArrayList lstPortal = ctlPortal.GetPortals();

            // cycle through each portal
            foreach (PortalInfo oPortal in lstPortal)
            {
                ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_PROCESSING, oPortal.PortalID, oPortal.PortalName));
                try
                {
                    // if the portal has the site settings, import its comments
                    string DisqusApplicationName = PortalController.GetPortalSetting(APPLICATION_NAME, oPortal.PortalID, string.Empty);

                    if (string.IsNullOrEmpty(DisqusApplicationName) == false)
                    {
                        var ctlModule = new FeatureController();
                        string DisqusApiSecret = PortalController.GetPortalSetting(API_SECRET, oPortal.PortalID, string.Empty);
                        string strResponse =
                            ctlModule.GetContent(
                                string.Format(ctlModule.GenericDisqusUrl(DisqusApplicationName, DisqusApiSecret),
                                              FeatureController.DISQUS_POST_LIST));
                        DisqusInfo oPosts = JsonConvert.DeserializeObject<DisqusInfo>(strResponse);

                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_CALLED, 
                            string.Format(ctlModule.GenericDisqusUrl(DisqusApplicationName, DisqusApiSecret), FeatureController.DISQUS_POST_LIST)));
                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_DISQUS_COMMENTS, oPosts.Response.Length));

                        /*
                         * TODO: Finish the scheduler item
                         * 1. Delete all posts from the DB for the Portal
                         * 1. Save all responses to memory
                         * 2. Slice up 1 per record
                         *      a. Be mindful of TabId, TabModuleId, PortalId
                         *      b. PortalId will require parsing to figure out
                         * 3. Save to DB
                         * 4. Determine if subsequent requests need to be made
                         *      a. Use the cursor to plan subsequent requests
                         *      b. Request repeatedly until the
                         *          I. API refuses the request
                         *          II. Cursor reveals no further records
                         */

                        // delete all saved Disqus records for the site
                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + MESSAGE_DELETE_COMMENTS);
                        ctlModule.DeleteDisqusbyPortal(oPortal.PortalID);

                        ArrayList lstResponse = new ArrayList();
                        foreach (DisqusResponseInfo oResponse in oPosts.Response)
                        {
                            lstResponse.Add(oResponse);
                        }
                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_LSTRESPONSE_COMMENTS, lstResponse.Count));

                        // build a header object
                        DisqusInfo oHeader = oPosts;
                        oHeader.Response = new DisqusResponseInfo[1];
                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_OPOST_COMMENTS, oPosts.Response.Length));
                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_OHEADER_COMMENTS, oHeader.Response.Length));
                        int i = 0;

                        // iterate through all responses (comments)
                        foreach (DisqusResponseInfo oResponse in lstResponse)
                        {
                            ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_SAVING_COMMENT, i));
                            
                            SaveResponse(ctlModule, oPortal.PortalID, oHeader, oResponse, i);

                            ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_COMMENT_SAVED, i));
                            i++;
                        }

                        ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_COMMENT_SUMMARY, oPortal.PortalName, i));
                    }
                }
                catch(Exception ex)
                {
                    Exceptions.LogException(ex);
                    ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_UNEXPECTED_ERROR, ex.Message, ex.StackTrace));
                    continue;
                }
            }
            
        }

        private void SaveResponse(FeatureController ctlModule, int PortalId, DisqusInfo oHeader, DisqusResponseInfo oResponse, int ItemId)
        {
            // assign the response to the header record
            oHeader.Response[0] = oResponse;

            // get a identity reference
            ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(MESSAGE_IDENTIFIER, oHeader.Response[0].Thread.Identifiers[0]));
            DisqusIdentifierInfo oIdentity = ctlModule.GetIdentifier(oHeader.Response[0].Thread.Identifiers[0]);

            try
            {
                // save the response object
                ctlModule.AddDisqus(PortalId,
                                    oIdentity.TabId,
                                    oIdentity.TabModuleId,
                                    oIdentity.RawUrl,
                                    SerializeToXML(oHeader),
                                    oHeader.Response[0].CreatedAt.ToString());
            }
            catch (Exception exSave)
            {
                ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(ERROR_MESSAGE_1, ItemId));
                ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(ERROR_MESSAGE_2, PortalId, oIdentity.TabId, oIdentity.TabModuleId, oIdentity.RawUrl, oHeader.Response[0].CreatedAt.ToString()));
                ScheduleHistoryItem.AddLogNote(Environment.NewLine + string.Format(ERROR_MESSAGE_3, exSave.Message, exSave.StackTrace));
            }
        }

        public string SerializeToXML(DisqusInfo oDisqus)
        {
            StringWriter outStream = new StringWriter();
            XmlSerializer oSerializer = new XmlSerializer(typeof(DisqusInfo));
            oSerializer.Serialize(outStream, oDisqus);
            return outStream.ToString();
        }

    }
}