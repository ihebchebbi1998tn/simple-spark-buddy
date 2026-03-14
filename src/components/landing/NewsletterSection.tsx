import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Facebook, Linkedin, Instagram } from "lucide-react";
import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import { useTranslation } from "react-i18next";

const NewsletterSection = () => {
  const { ref, isVisible } = useScrollAnimation();
  const { t } = useTranslation('hero');
  const [email, setEmail] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Newsletter signup:", email);
    setEmail("");
  };

  return (
    <motion.section 
      ref={ref}
      className="py-20 bg-gradient-to-br from-primary/5 to-blue-50"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-4xl">
        <div className="flex flex-col lg:flex-row items-center justify-between gap-8">
          <motion.div 
            className="text-center lg:text-left"
            initial={{ opacity: 0, x: -50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -50 }}
            transition={{ duration: 0.8, delay: 0.2 }}
          >
            <div className="flex items-center justify-center lg:justify-start mb-4">
              <img src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png" alt="Call Center Match Logo" className="h-16 w-auto" />
            </div>
            <div className="flex space-x-4 justify-center mt-4">
              <a href="#" className="text-muted-foreground hover:text-primary transition-colors" aria-label="Facebook"><Facebook className="w-6 h-6" /></a>
              <a href="#" className="text-muted-foreground hover:text-primary transition-colors" aria-label="LinkedIn"><Linkedin className="w-6 h-6" /></a>
              <a href="#" className="text-muted-foreground hover:text-primary transition-colors" aria-label="Instagram"><Instagram className="w-6 h-6" /></a>
            </div>
          </motion.div>

          <motion.div 
            className="flex-1 max-w-lg"
            initial={{ opacity: 0, x: 50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 50 }}
            transition={{ duration: 0.8, delay: 0.3 }}
          >
            <p className="text-muted-foreground mb-6 text-center lg:text-right">
              {t('newsletter.description')}
            </p>
            
            <form onSubmit={handleSubmit} className="flex gap-3">
              <Input type="email" placeholder={t('newsletter.placeholder')} value={email} onChange={e => setEmail(e.target.value)} className="flex-1 bg-white border-gray-200" required />
              <Button type="submit" className="whitespace-nowrap">{t('newsletter.subscribe')}</Button>
            </form>
          </motion.div>
        </div>
      </div>
    </motion.section>
  );
};

export default NewsletterSection;
