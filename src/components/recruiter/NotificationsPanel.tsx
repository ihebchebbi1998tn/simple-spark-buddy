import { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  X, 
  Bell, 
  ShoppingCart, 
  AlertTriangle, 
  CheckCircle, 
  Clock,
  Trash2
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { ScrollArea } from '@/components/ui/scroll-area';

interface Notification {
  id: string;
  type: 'success' | 'warning' | 'info' | 'order';
  title: string;
  message: string;
  time: string;
  read: boolean;
}

interface NotificationsPanelProps {
  isOpen: boolean;
  onClose: () => void;
}

const mockNotifications: Notification[] = [
  {
    id: '1',
    type: 'success',
    title: 'Nouveau record',
    message: 'Taux de conversion à 82% cette semaine',
    time: 'Il y a 2 heures',
    read: false,
  },
  {
    id: '2',
    type: 'warning',
    title: 'Attention nécessaire',
    message: 'Commande CMD-2024-003 en retard de livraison',
    time: 'Il y a 5 heures',
    read: false,
  },
  {
    id: '3',
    type: 'info',
    title: 'Nouveau rapport disponible',
    message: 'Rapport de performance mensuel prêt à consulter',
    time: 'Il y a 1 jour',
    read: false,
  },
  {
    id: '4',
    type: 'order',
    title: 'Nouvelle commande',
    message: '50 leads livrés pour la campagne Premium',
    time: 'Il y a 2 jours',
    read: true,
  },
  {
    id: '5',
    type: 'success',
    title: 'Candidat intégré',
    message: 'Ahmed Benali a été intégré avec succès',
    time: 'Il y a 3 jours',
    read: true,
  },
  {
    id: '6',
    type: 'info',
    title: 'Mise à jour système',
    message: 'Nouvelles fonctionnalités disponibles dans le simulateur',
    time: 'Il y a 4 jours',
    read: true,
  },
];

export function NotificationsPanel({ isOpen, onClose }: NotificationsPanelProps) {
  const [notifications, setNotifications] = useState<Notification[]>(mockNotifications);
  
  const unreadCount = notifications.filter(n => !n.read).length;

  const getNotificationIcon = (type: string) => {
    switch (type) {
      case 'success':
        return (
          <div className="w-9 h-9 rounded-xl bg-emerald-500/10 flex items-center justify-center shrink-0">
            <CheckCircle className="w-4 h-4 text-emerald-600" />
          </div>
        );
      case 'warning':
        return (
          <div className="w-9 h-9 rounded-xl bg-amber-500/10 flex items-center justify-center shrink-0">
            <AlertTriangle className="w-4 h-4 text-amber-600" />
          </div>
        );
      case 'order':
        return (
          <div className="w-9 h-9 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
            <ShoppingCart className="w-4 h-4 text-primary" />
          </div>
        );
      default:
        return (
          <div className="w-9 h-9 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
            <Bell className="w-4 h-4 text-primary" />
          </div>
        );
    }
  };

  const markAsRead = (id: string) => {
    setNotifications(prev =>
      prev.map(n => (n.id === id ? { ...n, read: true } : n))
    );
  };

  const markAllAsRead = () => {
    setNotifications(prev => prev.map(n => ({ ...n, read: true })));
  };

  const deleteNotification = (id: string) => {
    setNotifications(prev => prev.filter(n => n.id !== id));
  };

  const clearAll = () => {
    setNotifications([]);
  };

  return (
    <AnimatePresence>
      {isOpen && (
        <>
          {/* Backdrop */}
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className="fixed inset-0 bg-black/30 backdrop-blur-sm z-40"
            onClick={onClose}
          />
          
          {/* Panel */}
          <motion.div
            initial={{ x: '100%' }}
            animate={{ x: 0 }}
            exit={{ x: '100%' }}
            transition={{ type: 'spring', damping: 25, stiffness: 200 }}
            className="fixed top-0 right-0 h-full w-full sm:w-[420px] bg-background border-l border-border shadow-2xl z-50 flex flex-col"
          >
            {/* Header */}
            <div className="flex items-center justify-between p-4 sm:p-5 border-b border-border bg-muted/30 shrink-0">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-xl bg-primary/10 flex items-center justify-center">
                  <Bell className="w-5 h-5 text-primary" />
                </div>
                <div>
                  <h2 className="font-semibold text-foreground text-base sm:text-lg">Notifications</h2>
                  <p className="text-xs text-muted-foreground">
                    {unreadCount > 0 ? `${unreadCount} non lue${unreadCount > 1 ? 's' : ''}` : 'Toutes lues'}
                  </p>
                </div>
              </div>
              <Button variant="ghost" size="icon" onClick={onClose} className="h-9 w-9">
                <X className="w-5 h-5" />
              </Button>
            </div>

            {/* Actions Bar */}
            {notifications.length > 0 && (
              <div className="flex items-center justify-between px-4 sm:px-5 py-2.5 border-b border-border shrink-0 bg-background">
                <Button 
                  variant="ghost" 
                  size="sm" 
                  onClick={markAllAsRead}
                  disabled={unreadCount === 0}
                  className="text-xs h-8 px-2.5"
                >
                  Tout marquer comme lu
                </Button>
                <Button 
                  variant="ghost" 
                  size="sm" 
                  onClick={clearAll}
                  className="text-xs text-destructive hover:text-destructive h-8 px-2.5"
                >
                  Tout effacer
                </Button>
              </div>
            )}

            {/* Notifications List */}
            <ScrollArea className="flex-1">
              {notifications.length === 0 ? (
                <div className="flex flex-col items-center justify-center h-64 text-muted-foreground px-4">
                  <div className="w-16 h-16 rounded-full bg-muted/50 flex items-center justify-center mb-4">
                    <Bell className="w-8 h-8 opacity-40" />
                  </div>
                  <p className="text-sm font-medium">Aucune notification</p>
                  <p className="text-xs text-muted-foreground mt-1">Vous êtes à jour !</p>
                </div>
              ) : (
                <div className="p-3 sm:p-4 space-y-2">
                  {notifications.map((notification, index) => (
                    <motion.div
                      key={notification.id}
                      initial={{ opacity: 0, y: 10 }}
                      animate={{ opacity: 1, y: 0 }}
                      transition={{ delay: index * 0.03 }}
                      className={`group relative p-3.5 sm:p-4 rounded-xl cursor-pointer transition-all ${
                        !notification.read 
                          ? 'bg-primary/5 border border-primary/20 hover:bg-primary/10' 
                          : 'bg-muted/30 hover:bg-muted/50'
                      }`}
                      onClick={() => markAsRead(notification.id)}
                    >
                      <div className="flex gap-3">
                        {getNotificationIcon(notification.type)}
                        <div className="flex-1 min-w-0">
                          <div className="flex items-start justify-between gap-2 mb-1">
                            <h3 className={`text-sm font-medium line-clamp-1 ${!notification.read ? 'text-foreground' : 'text-muted-foreground'}`}>
                              {notification.title}
                            </h3>
                            {!notification.read && (
                              <span className="shrink-0 w-2 h-2 rounded-full bg-primary mt-1.5" />
                            )}
                          </div>
                          <p className="text-sm text-muted-foreground line-clamp-2 mb-2">
                            {notification.message}
                          </p>
                          <div className="flex items-center justify-between">
                            <span className="text-xs text-muted-foreground flex items-center gap-1.5">
                              <Clock className="w-3 h-3" />
                              {notification.time}
                            </span>
                            <Button
                              variant="ghost"
                              size="icon"
                              className="opacity-0 group-hover:opacity-100 h-7 w-7 text-muted-foreground hover:text-destructive transition-opacity"
                              onClick={(e) => {
                                e.stopPropagation();
                                deleteNotification(notification.id);
                              }}
                            >
                              <Trash2 className="w-3.5 h-3.5" />
                            </Button>
                          </div>
                        </div>
                      </div>
                    </motion.div>
                  ))}
                </div>
              )}
            </ScrollArea>
          </motion.div>
        </>
      )}
    </AnimatePresence>
  );
}
