"use client"

import { useEffect } from "react"
import { useNavigate, useLocation } from "react-router-dom"
import LoadingTransition from "@/components/shared/LoadingTransition"

const ProcessingInscription = () => {
  const navigate = useNavigate()
  const location = useLocation()

  console.log('⏳ ProcessingInscription loaded:', {
    hasStep1Data: !!location.state?.step1Data,
    locationState: location.state
  });

  useEffect(() => {
    // If no state data, redirect back to home
    if (!location.state?.step1Data) {
      console.log('⚠️ No step1Data found, redirecting to home');
      navigate("/", { replace: true })
    } else {
      console.log('✅ Step1Data present, will proceed to detailed inscription');
    }
  }, [location.state, navigate])

  const handleLoadingComplete = () => {
    console.log('✨ Loading transition complete, navigating to /inscription-detaillee');
    
    // Navigate to detailed inscription with the form data and preserve language test data
    navigate("/inscription-detaillee", {
      state: { 
        step1Data: location.state?.step1Data,
        // Preserve language test data
        fromLanguageTest: location.state?.fromLanguageTest,
        testScore: location.state?.testScore,
        testLanguage: location.state?.testLanguage,
      },
      replace: true,
    })
  }

  return <LoadingTransition onComplete={handleLoadingComplete} />
}

export default ProcessingInscription
