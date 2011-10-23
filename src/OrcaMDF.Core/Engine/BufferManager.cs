using System;
using System.Collections.Generic;
using System.IO;

namespace OrcaMDF.Core.Engine
{
	/// <summary>
	/// The buffer manager caches 8kb byte arrays in memory and reads from disk if necessary. It does not know about about
	/// pages themselves, just 8kb data chunks.
	/// </summary>
	public class BufferManager : IDisposable
	{
		private readonly Database database;
		private readonly Dictionary<short, Dictionary<int, byte[]>> buffer = new Dictionary<short, Dictionary<int, byte[]>>();
		private readonly Dictionary<short, FileStream> fileStreams = new Dictionary<short, FileStream>();

		internal BufferManager(Database db)
		{
			database = db;

			// For each file, instantiate a buffer
			foreach(var file in db.Files.Values)
			{
				buffer.Add(file.FileID, new Dictionary<int, byte[]>());
				fileStreams.Add(file.FileID, new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read));
			}
		}

		internal byte[] GetPageBytes(short fileID, int pageID)
		{
			// Ensure file is part of the database
			if (!buffer.ContainsKey(fileID))
				throw new ArgumentOutOfRangeException("File with ID " + fileID + " is not part of this database.");

			// Read bytes from file if not already in buffer
			if (!buffer[fileID].ContainsKey(pageID))
			{
				lock (buffer[fileID])
				{
					if (buffer[fileID].ContainsKey(pageID))
						return buffer[fileID][pageID];

					buffer[fileID].Add(pageID, getPageFromDisk(fileID, pageID));
				}
			}

			// Return result from buffer
			return buffer[fileID][pageID];
		}

		/// <summary>
		/// Reads the 8kb data chunk from the specified file on disk.
		/// </summary>
		private byte[] getPageFromDisk(short fileID, int pageID)
		{
			var fs = fileStreams[fileID];

			var bytes = new byte[8192];
			fs.Seek((long)pageID * 8192, SeekOrigin.Begin);
			fs.Read(bytes, 0, 8192);

			return bytes;
		}

		public void Dispose()
		{
			// Make sure to close all open file streams
			foreach (var fs in fileStreams.Values)
				fs.Dispose();
		}
	}
}