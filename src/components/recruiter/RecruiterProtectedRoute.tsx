import { Navigate } from 'react-router-dom';
import { recruiterAuthService } from '@/services/recruiterAuthService';

interface Props {
  children: React.ReactNode;
}

export function RecruiterProtectedRoute({ children }: Props) {
  if (!recruiterAuthService.isAuthenticated()) {
    return <Navigate to="/recruteurs/login" replace />;
  }
  return <>{children}</>;
}
