import Header from "@/components/shared/Header"
import { HeroCarousel, ComparisonSection, HowItWorksSection, TestimonialsSection, FAQSection, CTABlock } from "@/components/landing"
import Footer from "@/components/shared/Footer"

const Index = () => {
  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main>
        <HeroCarousel />
        <ComparisonSection />
        <HowItWorksSection />
        <TestimonialsSection />
        <FAQSection />
        <CTABlock />
      </main>
      <Footer />
    </div>
  )
}

export default Index
