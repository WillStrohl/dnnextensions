/*
' Copyright (c) 2012 DotNetNuke Corporation
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections.Generic;
//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Search;

namespace DNNCommunity.Modules.MyGroups.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for MyGroups
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

        public const string SETTINGKEY_PROFILETABID = "MyGroups.ProfileTabId";

        public const string FILEID_MATCH_PATTERN = @"^FileID=(\d+)$";

        #endregion

        #region Image Helpers

        public IFileInfo GetImageFromProvider(int PortalId, string FolderName, string FileName)
        {
            var oFolder = FolderManager.Instance.GetFolder(PortalId, FolderName);
            var oImage = FileManager.Instance.GetFile(oFolder, FileName);

            return oImage;
        }

        public IFileInfo GetImageFromProvider(int PortalId , IFolderInfo Folder , string FileName )
        {
            var oImage = FileManager.Instance.GetFile(Folder, FileName);
            return oImage;
        }

        /// <summary>
        /// GetImageFileUrl - this method returns the valid URL for any file, regardless to folder or folder provider in use
        /// </summary>
        /// <param name="Image">Fully loaded IFileInfo object</param>
        /// <returns></returns>
        /// <remarks>
        /// WARNING!!! This method can return exceptions. They should be caught and processed in the UI though.
        /// </remarks>
        public string GetImageFileUrl(IFileInfo Image)
        {
            /*******************************************************'
            ' WARNING!!!
            ' This method can return exceptions. They should be 
            ' caught and processed in the UI though.
            '*******************************************************/
            var mapFolder = FolderMappingController.Instance.GetFolderMapping(Image.FolderMappingID);
            return FolderProvider.Instance(mapFolder.FolderProviderType).GetFileUrl(Image);
        }

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

        //List<MyGroupsInfo> colMyGroupss = GetMyGroupss(ModuleID);
        //if (colMyGroupss.Count != 0)
        //{
        //    strXML += "<MyGroupss>";

        //    foreach (MyGroupsInfo objMyGroups in colMyGroupss)
        //    {
        //        strXML += "<MyGroups>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objMyGroups.Content) + "</content>";
        //        strXML += "</MyGroups>";
        //    }
        //    strXML += "</MyGroupss>";
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
        //XmlNode xmlMyGroupss = DotNetNuke.Common.Globals.GetContent(Content, "MyGroupss");
        //foreach (XmlNode xmlMyGroups in xmlMyGroupss.SelectNodes("MyGroups"))
        //{
        //    MyGroupsInfo objMyGroups = new MyGroupsInfo();
        //    objMyGroups.ModuleId = ModuleID;
        //    objMyGroups.Content = xmlMyGroups.SelectSingleNode("content").InnerText;
        //    objMyGroups.CreatedByUser = UserID;
        //    AddMyGroups(objMyGroups);
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

        //List<MyGroupsInfo> colMyGroupss = GetMyGroupss(ModInfo.ModuleID);

        //foreach (MyGroupsInfo objMyGroups in colMyGroupss)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objMyGroups.Content, objMyGroups.CreatedByUser, objMyGroups.CreatedDate, ModInfo.ModuleID, objMyGroups.ItemId.ToString(), objMyGroups.Content, "ItemId=" + objMyGroups.ItemId.ToString());
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
