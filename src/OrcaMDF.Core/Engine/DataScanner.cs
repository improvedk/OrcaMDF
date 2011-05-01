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
		/// Scans a heap beginning from the provided IAM page and onwards.
		/// </summary>
		public IEnumerable<TEntity> ScanHeap<TEntity>(PagePointer loc) where TEntity : new()
		{
			while (loc != PagePointer.Zero)
			{
				var iamPage = file.GetIamPage(loc);

				// Gather results from header slots and yield them afterwards
				var slotResults = new List<TEntity>();
				scanIamSlotPage(iamPage.Slot0, slotResults);
				scanIamSlotPage(iamPage.Slot1, slotResults);
				scanIamSlotPage(iamPage.Slot2, slotResults);
				scanIamSlotPage(iamPage.Slot3, slotResults);
				scanIamSlotPage(iamPage.Slot4, slotResults);
				scanIamSlotPage(iamPage.Slot5, slotResults);
				scanIamSlotPage(iamPage.Slot6, slotResults);
				scanIamSlotPage(iamPage.Slot7, slotResults);

				foreach (var entity in slotResults)
					yield return entity;

				// Then loop through extents and yield results
				foreach (var extent in iamPage.GetAllocatedExtents())
					foreach (var page in extent.GetPagePointers())
					{
						var dataPage = file.GetDataPage(page);

						foreach (var entity in dataPage.GetEntities<TEntity>())
							yield return entity;
					}

				loc = iamPage.Header.NextPage;
			}
		}

		private void scanIamSlotPage<TEntity>(PagePointer loc, List<TEntity> result) where TEntity : new()
		{
			if (loc != PagePointer.Zero)
			{
				var dataPage = file.GetDataPage(loc);
				result.AddRange(dataPage.GetEntities<TEntity>());
			}
		}

		/// <summary>
		/// Starts at the clustered index page (loc) and uses the b-tree structure to find all leaf level data pages. Not as fast as ScanClusteredIndex.
		/// </summary>
		public IEnumerable<TEntity> ScanClusteredIndexUsingIndexStructure<TIndex, TEntity>(PagePointer loc) where TIndex : ClusteredIndexEntity, new() where TEntity : new()
		{
			while (loc != PagePointer.Zero)
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