import Header from "@/components/shared/Header"
import Footer from "@/components/shared/Footer"
import CandidateInscriptionForm from "@/components/CandidateInscriptionForm"

const PreInscription: React.FC = () => {
  return (
    <div className="min-h-screen flex flex-col">
      <Header />
      <main className="flex-1 flex items-center justify-center py-12">
        <CandidateInscriptionForm />
      </main>
      <Footer />
    </div>
  )
}

export default PreInscription
