import { Linkedin, Facebook, Twitter } from "lucide-react"
import { Link } from "react-router-dom"
import { useTranslation } from "react-i18next"

const Footer = () => {
  const { t } = useTranslation('footer')

  return (
    <footer className="pt-16 pb-10 border-t border-border/50 bg-muted/15">
      <div className="container mx-auto px-4 lg:px-8 max-w-[1100px]">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-10 lg:gap-12">
          {/* Column 1: Description */}
          <div>
            <h3 className="text-base font-bold text-foreground mb-4">CallCenterMatch.ai</h3>
            <p className="text-muted-foreground/80 mb-5 max-w-[280px] leading-relaxed text-[13px]">
              {t('description')}
            </p>
            <div className="flex gap-2.5">
              <a href="https://www.linkedin.com/company/callcentermatch/" target="_blank" rel="noopener noreferrer" aria-label="LinkedIn" className="w-9 h-9 rounded-xl bg-muted/80 flex items-center justify-center text-muted-foreground hover:text-primary hover:bg-primary/8 transition-all duration-300">
                <Linkedin size={16} />
              </a>
              <a href="#" aria-label="Twitter" className="w-9 h-9 rounded-xl bg-muted/80 flex items-center justify-center text-muted-foreground hover:text-primary hover:bg-primary/8 transition-all duration-300">
                <Twitter size={16} />
              </a>
              <a href="https://www.facebook.com/CCenterMatch/" target="_blank" rel="noopener noreferrer" aria-label="Facebook" className="w-9 h-9 rounded-xl bg-muted/80 flex items-center justify-center text-muted-foreground hover:text-primary hover:bg-primary/8 transition-all duration-300">
                <Facebook size={16} />
              </a>
            </div>
          </div>

          {/* Column 2: Plateforme */}
          <div>
            <h4 className="font-semibold text-foreground mb-4 text-sm">{t('platform')}</h4>
            <ul className="space-y-2.5">
              <li><Link to="/" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('home')}</Link></li>
              <li><Link to="/a-propos" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('about')}</Link></li>
              <li><Link to="/candidats" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('candidates')}</Link></li>
              <li><Link to="/test-langue" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('selfAssessment')}</Link></li>
              <li><Link to="/entreprises" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('recruiters')}</Link></li>
              <li><Link to="/modeles-performance" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('performanceModels')}</Link></li>
            </ul>
          </div>

          {/* Column 3: Contact */}
          <div>
            <h4 className="font-semibold text-foreground mb-4 text-sm">{t('contactTitle')}</h4>
            <ul className="space-y-2.5">
              <li><span className="text-muted-foreground/80 text-[13px]">{t('location')}</span></li>
              <li><span className="text-muted-foreground/80 text-[13px]">contact@callcentermatch.tn</span></li>
              <li><span className="text-muted-foreground/80 text-[13px]">+216 70 123 456</span></li>
            </ul>
          </div>

          {/* Column 4: Légal */}
          <div>
            <h4 className="font-semibold text-foreground mb-4 text-sm">{t('legal')}</h4>
            <ul className="space-y-2.5">
              <li><Link to="/privacy" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('privacy')}</Link></li>
              <li><Link to="/terms" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('terms')}</Link></li>
              <li><Link to="/contact" className="text-muted-foreground/80 hover:text-primary transition-colors duration-200 text-[13px]">{t('legalNotice')}</Link></li>
            </ul>
          </div>
        </div>

        {/* Divider */}
        <div className="h-px bg-border/50 my-10" />

        {/* Bottom */}
        <div className="flex flex-wrap justify-between items-center gap-4 text-[12px] text-muted-foreground/60">
          <span>{t('copyright')}</span>
          <span className="font-medium text-muted-foreground/50">{t('tagline')}</span>
        </div>
      </div>
    </footer>
  )
}

export default Footer
