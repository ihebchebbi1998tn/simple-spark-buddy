namespace Main.ViewModels
{
	using System.Runtime.Serialization;

	[DataContract]
	public class CrmModelItem<T> : CrmModel, ICrmModelItem
	{
		[DataMember]
		public T Item { get; set; }
		[DataMember]
		object ICrmModelItem.Item
		{
			get { return Item; }
			set { Item = (T) value; }
		}
	}

	public interface ICrmModelItem
	{
		[DataMember]
		object Item { get; set; }
	}
}
