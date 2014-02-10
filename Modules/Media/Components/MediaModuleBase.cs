//
// DNN Corp - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

//INSTANT C# NOTE: Formerly VB project-level imports:
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace DotNetNuke.Modules.Media
{

	public abstract class MediaModuleBase : DotNetNuke.Entities.Modules.PortalModuleBase
    {

        #region Private Members

        private bool _PostToJournal = false;
        private bool _PostToJournalSiteWide = false;
        private bool _OverrideJournalSetting = false;
        private bool _NotifyOnUpdate = false;

        #endregion

        #region Properties

        /// <summary>
        /// PostToJournal - If true, the module can post a message to the journal
        /// </summary>
        protected bool PostToJournal{
            get{
                if (PostToJournalSiteWide && OverrideJournalSetting == false)
                {
                    string strSettingValue = PortalController.GetPortalSetting(MediaController.SETTING_POSTTOJOURNAL, PortalId, string.Empty);

                    if (!string.IsNullOrEmpty(strSettingValue))
                    {
                        _PostToJournal = bool.Parse(strSettingValue);
                    }
                }
                else
                {
                    if (Settings[MediaController.SETTING_POSTTOJOURNAL] != null)
                    {
                        _PostToJournal = bool.Parse(Settings[MediaController.SETTING_POSTTOJOURNAL].ToString());
                    }
                }

                return _PostToJournal;
            }
            private set
            {
                _PostToJournal = value;
            }
        }

        /// <summary>
        /// PostToJournalSiteWide - if true, all instances of the media module will post to the journal when updated
        /// </summary>
        protected bool PostToJournalSiteWide
        {
            get
            {
                string strSettingValue = PortalController.GetPortalSetting(MediaController.SETTING_POSTTOJOURNALSITEWIDE, PortalId, string.Empty);

                if (!string.IsNullOrEmpty(strSettingValue))
                {
                    _PostToJournalSiteWide = bool.Parse(strSettingValue);
                }
                return _PostToJournalSiteWide;
            }
            private set
            {
                _PostToJournalSiteWide = value;
            }
        }

        /// <summary>
        /// OverrideJournalSetting - if true, the module will override the site setting (if it exists)
        /// </summary>
        protected bool OverrideJournalSetting
        {
            get
            {
                if (Settings[MediaController.SETTING_OVERRIDEJOURNALSETTING] != null)
                {
                    _OverrideJournalSetting = bool.Parse(Settings[MediaController.SETTING_OVERRIDEJOURNALSETTING].ToString());
                }
                return _OverrideJournalSetting;
            }
            private set
            {
                _OverrideJournalSetting = value;
            }
        }

        /// <summary>
        /// NotifyOnUpdate - if true, the module will notify people in the message center when an update is performed
        /// </summary>
        protected bool NotifyOnUpdate
        {
            get
            {
                string strSettingValue = PortalController.GetPortalSetting(MediaController.SETTING_NOTIFYONUPDATE, PortalId, string.Empty);

                if (!string.IsNullOrEmpty(strSettingValue))
                {
                    _NotifyOnUpdate = bool.Parse(strSettingValue);
                }

                return _NotifyOnUpdate;
            }
            private set
            {
                _NotifyOnUpdate = value;
            }
        }

        #endregion

        #region Localization 

        protected string GetLocalizedString(string Key)
		{
			return GetLocalizedString(Key, this.LocalResourceFile);
		}

		protected string GetLocalizedString(string Key, string LocalizationFilePath)
		    {
			    return Localization.GetString(Key, LocalizationFilePath);
            }

        #endregion

    }

}