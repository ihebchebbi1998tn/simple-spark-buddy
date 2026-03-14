namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.Model.Site;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode.Conformist;

	public class DomainExtensionMapping : ComponentMapping<DomainExtension>, INHibernateMappingExtension<Domain, DomainExtension>
	{
		public DomainExtensionMapping()
		{
			Property(x => x.DefaultLanguageKey);
			Property(x => x.DefaultLocale);
			Property(x => x.DefaultCountryKey);
			Property(x => x.Host);
			Property(x => x.MaterialLogo, m => m.Length(Int32.MaxValue));
			Property(x => x.MaterialTheme);
			Property(x => x.ReportFooterCol1);
			Property(x => x.ReportFooterCol2);
			Property(x => x.ReportFooterCol3);
			Property(x => x.ReportFooterImage, m => m.Length(Int32.MaxValue));
			Property(x => x.ReportFooterImageContentType);
			Property(x => x.ReportLogo, m => m.Length(Int32.MaxValue));
			Property(x => x.ReportLogoContentType);
			Property(x => x.SiteLogo, m => m.Length(Int32.MaxValue));
			Property(x => x.Theme);
			Property(x => x.LegacyId);
			Property(x => x.ContractNo);
			Property(x => x.ProjectId);
			Property(x => x.LicenseKey);
			Property(x => x.ReportBarcodeShowOnServiceReport);
			Property(x => x.ReportBarcodeShowOnChecklistPdf);
			Property(x => x.ReportBarcodePrintOnly);
			Property(x => x.ReportBarcodePrefix);
			Property(x => x.ReportBarcodeRoot);
			Property(x => x.ReportBarcodeSuffix);
			Property(x => x.ReportBarcodeFormat);
			Property(x => x.MinPasswordStrength);
			Property(x => x.MultiFactorAuthenticationModeKey);
		}
	}
}
