import { useState, useEffect } from "react";
import DetailedInscriptionForm from "@/components/DetailedInscriptionForm";
import LoadingTransition from "@/components/shared/LoadingTransition";

const DetailedInscription = () => {
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Simulate loading time for the detailed inscription page
    const timer = setTimeout(() => {
      setIsLoading(false);
    }, 100);

    return () => clearTimeout(timer);
  }, []);

  if (isLoading) {
    return <LoadingTransition onComplete={() => setIsLoading(false)} />;
  }

  return <DetailedInscriptionForm />;
};

export default DetailedInscription;
