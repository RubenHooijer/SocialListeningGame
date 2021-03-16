using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;

namespace UnityEditor.Localization.Samples.Google
{
    /// <summary>
    /// These examples show various ways to sync a String Table Collection with Google Sheets.
    /// These examples are illustrative and will not work as they are without correct Google Sheets credentials and String Table data.
    /// </summary>
    public class GoogleSheetsExamples
    {

        /// <summary>
        /// This example shows how we can push every String Table Collection that contains a Google Sheets extension.
        /// </summary>
        [MenuItem("Tools/Localization/Pull All Google Sheets Extensions")]
        public static void PullAllExtensions()
        {
            // Get every String Table Collection
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                // Its possible a String Table Collection may have more than one GoogleSheetsExtension.
                // For example if each Locale we pushed/pulled from a different sheet.
                foreach (var extension in collection.Extensions)
                {
                    if (extension is GoogleSheetsExtension googleExtension)
                    {
                        PullExtension(googleExtension);
                    }
                }
            }
        }

        private static void PullExtension(GoogleSheetsExtension googleExtension)
        {
            // Setup the connection to Google
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
            googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;

            // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
            googleSheets.PullIntoStringTableCollection(googleExtension.SheetId, googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns, reporter: new ProgressBarReporter());
        }
    }
}
