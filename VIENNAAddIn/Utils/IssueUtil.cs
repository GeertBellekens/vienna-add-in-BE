/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;

namespace VIENNAAddIn.Utils
{
	/// <sUMM2ary>
	/// SUMM2ary description for IssueUtil.
	/// </sUMM2ary>
	internal class IssueUtil
	{	
		private EA.Repository repository;
		private const String DONT_ASK_FOR_INPUT_FILE = "DontAskForInputFile";
		internal IssueUtil(EA.Repository repository)
		{
			//
			// TODO: Add constructor logic here
			//
			this.repository = repository;
		}
		/// <sUMM2ary>
		/// sets a flag, that the user isnt asked for the input file for a 
		/// worksheet every time
		/// </sUMM2ary>
		internal void SetDontAskForInputWorksheet() {
			// set the issue to save the option as 'don't ask me again'
			EA.ProjectIssues pIssues = (EA.ProjectIssues)repository.Issues.AddNew(DONT_ASK_FOR_INPUT_FILE,"Issue");
			pIssues.Update();
			repository.Issues.Refresh();
		}
		/// <sUMM2ary>
		/// removes a flag, that the user isnt asked for the input file for a 
		/// worksheet every time
		/// </sUMM2ary>
		internal void UnSetDontAskForInputWorksheet() {
			for(short i = 0; i < repository.Issues.Count; i++) {
				EA.ProjectIssues issue = (EA.ProjectIssues)repository.Issues.GetAt(i); 
				if(issue.Name.Equals(DONT_ASK_FOR_INPUT_FILE)) {
					repository.Issues.Delete(i);
				}
			}
			repository.Issues.Refresh();
		}
		/// <sUMM2ary>
		/// Convience method to allow the setting/unsetting of the 'DontAskForInputWorksheet'
		/// based on a boolean parameter given as an argument of the method
		/// </sUMM2ary>
		/// <param name="setting">true if the 'DontAskForInputWorksheet' parameter
		/// should be set, false to unset</param>
		internal void SetDontAskForInputWorksheet(bool setting) {
			if(setting) {
				this.SetDontAskForInputWorksheet();
			}
			else {
				this.UnSetDontAskForInputWorksheet();
			}
		}
		/// <sUMM2ary>
		/// checks if the user is asked to determine the input file for a worksheet 
		/// every time
		/// </sUMM2ary>
		/// <returns>true if the user is not asked every time (= there is a default worksheet),
		/// false if he has to determine every time</returns>
		internal bool GetDontAskForInputWorksheet() {
			foreach(EA.ProjectIssues issue in repository.Issues) {
				if(issue.Name.Equals(DONT_ASK_FOR_INPUT_FILE)) {
					return true;
				}
			}
			return false;
		}

	}
}
