namespace OrcaMDF.Core.Engine.Pages
{
	public enum PageType : byte
	{
		Data = 1,
		Index = 2,
		TextMix = 3,
		TextTree = 4,
		Sort = 7,
		GAM = 8,
		SGAM = 9,
		IAM = 10,
		PFS = 11,
		Boot = 13,
		FileHeader = 15,
		DiffMap = 16,
		MLMap = 17
	}
}