using System.Collections.Generic;

namespace OrcaMDF.Core.Engine
{
	public class DataScanner
	{
		private readonly MdfFile file;

		public DataScanner(MdfFile file)
		{
			this.file = file;
		}

		public IEnumerable<T> ScanLinkedPages<T>(PageLocation firstPage) where T : new()
		{
			while(firstPage.FileID != 0 && firstPage.PageID != 0)
			{
				var page = file.GetDataPage(firstPage.PageID);

				foreach (var entity in page.GetEntities<T>())
					yield return entity;

				firstPage = page.Header.NextPage;
			}
		}
	}
}