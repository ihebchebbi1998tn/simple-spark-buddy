namespace Main.ViewModels
{
	using System;
	using System.Reflection;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;
	using Crm.Library.Model.Site;

	public class SiteViewModel : ITransientDependency
    {
        public virtual Guid ID { get; protected set; }
        public virtual string DisplayName { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string DefaultLanguageKey { get; set; }
		public virtual string DefaultLocale { get; set; }
		public virtual string DefaultCountryKey { get; set; }
        public virtual string Theme { get; set; }
        public virtual bool IsProfilingActive { get; set; }
        public virtual string MaterialTheme { get; set; }
        public virtual Uri Host { get; set; }
        public virtual byte[] ReportLogo { get; set; }
        public virtual string ReportLogoContentType { get; set; }
        public virtual string ReportFooterCol1 { get; set; }
				public virtual string ReportFooterCol2 { get; set; }
				public virtual string ReportFooterCol3 { get; set; }
				public virtual byte[] ReportFooterImage { get; set; }
				public virtual string ReportFooterImageContentType { get; set; }
				public virtual byte[] SiteLogo { get; set; }
				public virtual string CompanyName { get; set; }
				public virtual string ResponsibleAddress { get; set; }
				public virtual string DataProtectionOfficer { get; set; }
        public virtual string ApplicationVersion { get; set; }

        // Constructor
        public SiteViewModel()
        {
            // Constructor for Testing purposes
        }

        public SiteViewModel(Site currentSite)
        {
            ID = currentSite.UId;
            DisplayName = currentSite.Name ?? "L-mobile CRM";
            MaterialTheme = currentSite.GetExtension<DomainExtension>().MaterialTheme;
            DefaultLanguageKey = currentSite.GetExtension<DomainExtension>().DefaultLanguageKey;
            DefaultCountryKey = currentSite.GetExtension<DomainExtension>().DefaultCountryKey;
            DefaultLocale = currentSite.GetExtension<DomainExtension>().DefaultLocale;
            Theme = currentSite.GetExtension<DomainExtension>().Theme;
            Host = currentSite.GetExtension<DomainExtension>().HostUri ?? new Uri("http://localhost/");
            ReportLogo = currentSite.GetExtension<DomainExtension>().ReportLogo;
            ReportLogoContentType = currentSite.GetExtension<DomainExtension>().ReportLogoContentType;
						ReportFooterCol1 = currentSite.GetExtension<DomainExtension>().ReportFooterCol1;
						ReportFooterCol2 = currentSite.GetExtension<DomainExtension>().ReportFooterCol2;
						ReportFooterCol3 = currentSite.GetExtension<DomainExtension>().ReportFooterCol3;
						ReportFooterImage = currentSite.GetExtension<DomainExtension>().ReportFooterImage;
						ReportFooterImageContentType = currentSite.GetExtension<DomainExtension>().ReportFooterImageContentType;
						SiteLogo = currentSite.GetExtension<DomainExtension>().SiteLogo;
						CompanyName = currentSite.GetExtension<GDPRDomainExtension>().CompanyName;
						ResponsibleAddress = currentSite.GetExtension<GDPRDomainExtension>().ResponsibleAddress;
						DataProtectionOfficer = currentSite.GetExtension<GDPRDomainExtension>().DataProtectionOfficer;
						ApplicationVersion = Assembly.GetAssembly(typeof(User)).GetName().Version.ToString();
        }
    }
}
