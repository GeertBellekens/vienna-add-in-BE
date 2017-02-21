/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;

namespace VIENNAAddIn.validator
{
    internal class ValidationMessage
    {
        #region errorLevelTypes enum

        public enum errorLevelTypes
        {
            INFO,
            WARN,
            ERROR
        } ;

        #endregion

        private static int messageIDcounter;


        /// <summary>
        /// Create a new Validationmessage and generate an automatic messageID
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="messageDetail_"></param>
        /// <param name="affectedView_"></param>
        /// <param name="errorLevel_"></param>
        /// <param name="affectedPackageID_"></param>
        /// 
        public ValidationMessage(String message_, String messageDetail_, String affectedView_,
                                 errorLevelTypes errorLevel_, int affectedPackageID_)
        {
            Message = message_;
            MessageDetail = messageDetail_;
            AffectedView = affectedView_;
            ErrorLevel = errorLevel_;
            AffectedPackageID = affectedPackageID_;
            MessageID = ++messageIDcounter;
        }


        public int MessageID { get; set; }


        public errorLevelTypes ErrorLevel { get; set; }


        public String Message { get; set; }

        public String MessageDetail { get; set; }

        public String AffectedView { get; set; }

        public int AffectedPackageID { get; set; }
    }
}