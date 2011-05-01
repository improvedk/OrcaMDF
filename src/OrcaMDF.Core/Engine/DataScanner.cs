using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine
{
	public class DataScanner
	{
		private readonly MdfFile file;

		public DataScanner(MdfFile file)
		{
			this.file = file;
		}

		/// <summary>
		/// Starts at the clustered index page (loc) and uses the b-tree structure to find all leaf level data pages. Not as fast as ScanClusteredIndex.
		/// </summary>
		public IEnumerable<TEntity> ScanClusteredIndexUsingIndexStructure<TIndex, TEntity>(PagePointer loc) where TIndex : ClusteredIndexEntity, new() where TEntity : new()
		{
			while (loc.FileID != 0 && loc.PageID != 0)
			{
				var page = file.GetClusteredIndexPage(loc);

				foreach (var indexRecord in page.GetEntities<TIndex>())
				{
					if (page.Header.Level > 1)
					{
						// Index level > 1 means next level is index as well. Follow chain.
						foreach (var entity in ScanClusteredIndex<TIndex, TEntity>(indexRecord.ChildPage))
							yield return entity;
					}
					else
					{
						// Index level == 0 means next level is leaf - return entities from data page.
						var dataPage = file.GetDataPage(indexRecord.ChildPage);

						foreach (var entity in dataPage.GetEntities<TEntity>())
							yield return entity;
					}
				}

				loc = page.Header.NextPage;
			}
		}

		/// <summary>
		/// Starts at the clustered index page (loc) and follows the b-tree structure till the first datapage, from where it'll do a linked page scan.
		/// </summary>
		public IEnumerable<TEntity> ScanClusteredIndex<TIndex, TEntity>(PagePointer loc) where TIndex : ClusteredIndexEntity, new() where TEntity : new()
		{
			var page = file.GetClusteredIndexPage(loc);
			var indexRecord = page.GetEntities<TIndex>().FirstOrDefault();
			
			if(indexRecord != null)
			{
				if(page.Header.Level > 1)
				{
					// Index level > 1 means next level is index as well. Follow chain.
					foreach(var entity in ScanClusteredIndex<TIndex, TEntity>(indexRecord.ChildPage))
						yield return entity;
				}
				else
				{
					// Index level == 1 means next level is leaf - scan linked data pages.
					foreach(var entity in ScanLinkedPages<TEntity>(indexRecord.ChildPage))
						yield return entity;
				}
			}
		}

		/// <summary>
		/// Starts at the data page (loc) and follows the NextPage pointer chain till the end.
		/// </summary>
		public IEnumerable<T> ScanLinkedPages<T>(PagePointer loc) where T : new()
		{
			while (loc != PagePointer.Zero)
			{
				var page = file.GetDataPage(loc);

				foreach (var entity in page.GetEntities<T>())
					yield return entity;
				
				loc = page.Header.NextPage;
			}
		}
	}
}