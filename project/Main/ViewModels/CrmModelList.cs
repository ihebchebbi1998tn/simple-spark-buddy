namespace Main.ViewModels
{
	using System.Collections;
	using System.Collections.Generic;

	public class CrmModelList<T> : CrmModel, ICrmModelList
	{
		public IList<T> List { get; set; }

		IList ICrmModelList.List
		{
			get { return (IList)List; }
			set { List = (IList<T>)value; }
		}
	}
	public interface ICrmModelList
	{
		IList List { get; set; }
	}
}
