using System;
using System.IO;
using System.Runtime.InteropServices;
using Alphaleonis.Win32.Vss;

namespace OrcaMDF.Adhoc
{
	class VssHelper
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, int lpData, ref int pbCancel, uint dwCopyFlags);
		private delegate uint CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, uint dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

		public static void CopyFile(string source, string destination)
		{
			var oVSSImpl = VssUtils.LoadImplementation();

			using (var vss = oVSSImpl.CreateVssBackupComponents())
			{
				vss.InitializeForBackup(null);

				vss.SetBackupState(false, true, VssBackupType.Full, false);

				using (var async = vss.GatherWriterMetadata())
					async.Wait();

				Guid snapshotSet = vss.StartSnapshotSet();
				string volume = new FileInfo(source).Directory.Root.Name;
				var snapshot = vss.AddToSnapshotSet(volume, Guid.Empty);

				using (var async = vss.PrepareForBackup())
					async.Wait();

				using (var async = vss.DoSnapshotSet())
					async.Wait();

				var props = vss.GetSnapshotProperties(snapshot);
				string vssFile = source.Replace(volume, props.SnapshotDeviceObject + @"\");

				int cancel = 0;
				CopyFileEx(vssFile, destination, null, 0, ref cancel, 0);
			}
		}
	}
}