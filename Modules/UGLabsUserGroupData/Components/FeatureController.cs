/*
' Copyright (c) 2013 DNN Corp.
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

namespace DNNCommunity.Modules.UGLabsUserGroupData.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for UGLabsUserGroupData
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {

        #region Constants

        public const string PATTERN_WEBSITE_URL = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?";
        public const string PATTERN_FACEBOOK_URL = @"http(s)?://www\.facebook\.com/.*";
        public const string PATTERN_TWITTER_URL = @"http(s)?://(www)?\.?twitter\.com.*";
        public const string PATTERN_LINKEDIN_URL = @"http(s)*://www\.linkedin\.com/.*";
        public const string PATTERN_GOOGLEPLUS_URL = @"http(s)?://plus\.google\.com/.*";
        public const string PATTERN_MEETUP_URL = @"http(s)?://www\.meetup\.com/.*";
        public const string PATTERN_YOUTUBE_URL = @"http(s)?://.*\.youtube\.com/.*";

        public const string KEY_COUNTRY = "ugCountry";
        public const string KEY_COUNTRYFULL = "ugCountryFull";
        public const string KEY_REGION = "ugRegion";
        public const string KEY_REGIONFULL = "ugRegionFull";
        public const string KEY_CITY = "ugCity";
        public const string KEY_WEBSITEURL = "ugWebsiteUrl";
        public const string KEY_LATITUDE = "ugLatitude";
        public const string KEY_LONGITUDE = "ugLongitude";
        public const string KEY_TWITTERURL = "ugTwitterUrl";
        public const string KEY_LINKEDINURL = "ugLinkedInUrl";
        public const string KEY_FACEBOOKURL = "ugFacebookUrl";
        public const string KEY_DEFAULTLANGUAGE = "ugDefaultLanguage";
        public const string KEY_MEETUPURL = "ugMeetUpUrl";
        public const string KEY_YOUTUBEURL = "ugYouTubeUrl";
        public const string KEY_GOOGLEPLUSURL = "ugGooglePlusUrl";

        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<UGLabsMetaDataInfo> colUGLabsMetaDatas = GetUGLabsMetaDatas(ModuleID);
        //if (colUGLabsMetaDatas.Count != 0)
        //{
        //    strXML += "<UGLabsMetaDatas>";

        //    foreach (UGLabsMetaDataInfo objUGLabsMetaData in colUGLabsMetaDatas)
        //    {
        //        strXML += "<UGLabsMetaData>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objUGLabsMetaData.Content) + "</content>";
        //        strXML += "</UGLabsMetaData>";
        //    }
        //    strXML += "</UGLabsMetaDatas>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlUGLabsMetaDatas = DotNetNuke.Common.Globals.GetContent(Content, "UGLabsMetaDatas");
        //foreach (XmlNode xmlUGLabsMetaData in xmlUGLabsMetaDatas.SelectNodes("UGLabsMetaData"))
        //{
        //    UGLabsMetaDataInfo objUGLabsMetaData = new UGLabsMetaDataInfo();
        //    objUGLabsMetaData.ModuleId = ModuleID;
        //    objUGLabsMetaData.Content = xmlUGLabsMetaData.SelectSingleNode("content").InnerText;
        //    objUGLabsMetaData.CreatedByUser = UserID;
        //    AddUGLabsMetaData(objUGLabsMetaData);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<UGLabsMetaDataInfo> colUGLabsMetaDatas = GetUGLabsMetaDatas(ModInfo.ModuleID);

        //foreach (UGLabsMetaDataInfo objUGLabsMetaData in colUGLabsMetaDatas)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objUGLabsMetaData.Content, objUGLabsMetaData.CreatedByUser, objUGLabsMetaData.CreatedDate, ModInfo.ModuleID, objUGLabsMetaData.ItemId.ToString(), objUGLabsMetaData.Content, "ItemId=" + objUGLabsMetaData.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
