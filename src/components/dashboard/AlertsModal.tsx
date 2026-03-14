import { Bell, CheckCircle2, AlertTriangle, Info, X, Clock, AlertCircle } from 'lucide-react';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';
import { candidateService } from '@/services/candidateService';
import { useEffect, useState } from 'react';
import { useToast } from '@/hooks/use-toast';
import { formatDistanceToNow } from 'date-fns';
import { fr } from 'date-fns/locale';

interface AlertsModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

interface Alert {
  _id: string;
  type: 'success' | 'warning' | 'info' | 'error';
  title: string;
  message: string;
  action_type: string;
  read: boolean;
  created_at: string;
}

export const AlertsModal = ({ open, onOpenChange }: AlertsModalProps) => {
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [loading, setLoading] = useState(false);
  const { toast } = useToast();

  useEffect(() => {
    if (open) {
      fetchAlerts();
    }
  }, [open]);

  const fetchAlerts = async () => {
    setLoading(true);
    try {
      const response = await candidateService.getAlerts();
      if (response.success) {
        setAlerts(response.data.alerts);
        setUnreadCount(response.data.unreadCount);
      }
    } catch (error) {
      console.error('Error fetching alerts:', error);
      toast({
        title: 'Erreur',
        description: 'Impossible de charger les alertes',
        variant: 'destructive',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      const response = await candidateService.markAllAlertsAsRead();
      if (response.success) {
        setAlerts(alerts.map(alert => ({ ...alert, read: true })));
        setUnreadCount(0);
        toast({
          title: 'Succès',
          description: 'Toutes les alertes ont été marquées comme lues',
        });
      }
    } catch (error) {
      console.error('Error marking alerts as read:', error);
      toast({
        title: 'Erreur',
        description: 'Impossible de marquer les alertes comme lues',
        variant: 'destructive',
      });
    }
  };

  const getAlertIcon = (type: Alert['type']) => {
    switch (type) {
      case 'success':
        return <CheckCircle2 className="h-4 w-4 text-green-600" />;
      case 'warning':
        return <AlertTriangle className="h-4 w-4 text-orange-600" />;
      case 'info':
        return <Info className="h-4 w-4 text-blue-600" />;
      case 'error':
        return <AlertCircle className="h-4 w-4 text-red-600" />;
    }
  };

  const formatRelativeTime = (dateString: string) => {
    try {
      return formatDistanceToNow(new Date(dateString), { 
        addSuffix: true, 
        locale: fr 
      });
    } catch {
      return 'Date inconnue';
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg max-h-[90vh] overflow-hidden flex flex-col p-0">
        <DialogHeader className="p-4 sm:p-6 pb-3 border-b">
          <DialogTitle className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-2">
              <Bell className="h-5 w-5" />
              <span className="text-base sm:text-lg">Alertes</span>
              {unreadCount > 0 && (
                <Badge variant="destructive" className="h-5 px-1.5 text-xs">
                  {unreadCount}
                </Badge>
              )}
            </div>
          </DialogTitle>
        </DialogHeader>

        <ScrollArea className="flex-1 px-4 sm:px-6">
          <div className="space-y-3 py-4">
            {loading ? (
              <div className="text-center py-12">
                <p className="text-sm text-muted-foreground">Chargement...</p>
              </div>
            ) : alerts.length === 0 ? (
              <div className="text-center py-12">
                <Bell className="h-12 w-12 mx-auto mb-3 text-muted-foreground/40" />
                <p className="text-sm text-muted-foreground">
                  Aucune alerte pour le moment
                </p>
              </div>
            ) : (
              alerts.map((alert) => (
                <div
                  key={alert._id}
                  className={`relative p-3 sm:p-4 rounded-lg border bg-card hover:bg-accent/5 transition-colors ${
                    !alert.read ? 'border-primary/40' : ''
                  }`}
                >
                  {!alert.read && (
                    <div className="absolute top-3 right-3 w-2 h-2 bg-primary rounded-full" />
                  )}
                  
                  <div className="flex gap-3">
                    <div className="shrink-0 mt-0.5">
                      {getAlertIcon(alert.type)}
                    </div>
                    
                    <div className="flex-1 min-w-0 space-y-1">
                      <h4 className="font-medium text-sm">
                        {alert.title}
                      </h4>
                      
                      <p className="text-sm text-muted-foreground">
                        {alert.message}
                      </p>
                      
                      <div className="flex items-center gap-1.5 text-xs text-muted-foreground pt-1">
                        <Clock className="h-3 w-3" />
                        <span>{formatRelativeTime(alert.created_at)}</span>
                      </div>
                    </div>
                  </div>
                </div>
              ))
            )}
          </div>
        </ScrollArea>

        <div className="border-t p-4 bg-muted/30">
          <Button 
            variant="ghost" 
            size="sm"
            className="w-full text-xs"
            onClick={handleMarkAllAsRead}
            disabled={unreadCount === 0 || loading}
          >
            Tout marquer comme lu
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
};