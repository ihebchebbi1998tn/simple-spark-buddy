namespace Main.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;

	public interface IInformationSchemaCache : ISingletonDependency
	{
		IEnumerable<InformationSchema> GetAll();
	}
}
