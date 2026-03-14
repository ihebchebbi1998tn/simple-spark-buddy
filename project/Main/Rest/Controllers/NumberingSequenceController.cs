namespace Main.Rest.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;

	using Main.Model;
	using Main.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class NumberingSequenceController : RestController
	{
		private readonly INumberingService numberingService
			;
		private readonly IRepositoryWithTypedId<NumberingSequence, Guid> numberingSequenceRepository;
		private readonly IMapper mapper;

		public NumberingSequenceController(IRepositoryWithTypedId<NumberingSequence, Guid> numberingSequenceRepository, INumberingService numberingService, IMapper mapper, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.numberingSequenceRepository = numberingSequenceRepository;
			this.numberingService = numberingService;
			this.mapper = mapper;
		}

		public virtual ActionResult Get(string id)
		{
			var result = numberingService.GetNextFormattedNumber(id);
			return Rest(result);
		}

		public virtual ActionResult GetNextHighValue(string id)
		{
			var result = numberingService.GetNextHighValue(id);
			return Rest(result);
		}

		public virtual ActionResult List()
		{
			var numberingSequences = numberingSequenceRepository.GetAll().ToList();
			var result = mapper.Map<List<NumberingSequenceInfo>>(numberingSequences);
			return Rest(result);
		}

		internal class NumberingSequenceInfo
		{
			public string SequenceName { get; set; }
			public long? MaxLow { get; set; }
			public long LastNumber { get; set; }
			public string Prefix { get; set; }
			public string Format { get; set; }
			public string Suffix { get; set; }
		}
	}
}
