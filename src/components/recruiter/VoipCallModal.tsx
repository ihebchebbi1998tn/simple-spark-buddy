import { useState, useEffect, useCallback } from 'react';
import {
  Dialog, DialogContent,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Phone, PhoneOff, Mic, MicOff, Pause, Play,
  Volume2, VolumeX, User, Clock, PhoneCall,
  PhoneForwarded, MessageSquare
} from 'lucide-react';

interface VoipCallModalProps {
  isOpen: boolean;
  onClose: () => void;
  leadName: string;
  leadPhone: string;
  leadId: string;
  onCallEnd?: (duration: number, outcome: string) => void;
}

type CallStatus = 'ringing' | 'connected' | 'on-hold' | 'ended';

export function VoipCallModal({ isOpen, onClose, leadName, leadPhone, leadId, onCallEnd }: VoipCallModalProps) {
  const [callStatus, setCallStatus] = useState<CallStatus>('ringing');
  const [elapsedSeconds, setElapsedSeconds] = useState(0);
  const [isMuted, setIsMuted] = useState(false);
  const [isSpeakerOff, setIsSpeakerOff] = useState(false);
  const [callOutcome, setCallOutcome] = useState('');
  const [showOutcome, setShowOutcome] = useState(false);

  // Reset on open
  useEffect(() => {
    if (isOpen) {
      setCallStatus('ringing');
      setElapsedSeconds(0);
      setIsMuted(false);
      setIsSpeakerOff(false);
      setCallOutcome('');
      setShowOutcome(false);
    }
  }, [isOpen]);

  // Simulate ringing -> connected after 2.5s
  useEffect(() => {
    if (!isOpen || callStatus !== 'ringing') return;
    const t = setTimeout(() => setCallStatus('connected'), 2500);
    return () => clearTimeout(t);
  }, [isOpen, callStatus]);

  // Timer
  useEffect(() => {
    if (!isOpen || (callStatus !== 'connected' && callStatus !== 'on-hold')) return;
    const interval = setInterval(() => setElapsedSeconds(s => s + 1), 1000);
    return () => clearInterval(interval);
  }, [isOpen, callStatus]);

  const formatTime = (s: number) => {
    const m = Math.floor(s / 60);
    const sec = s % 60;
    return `${m.toString().padStart(2, '0')}:${sec.toString().padStart(2, '0')}`;
  };

  const handleHangup = useCallback(() => {
    setCallStatus('ended');
    setShowOutcome(true);
  }, []);

  const handleEndCall = useCallback(() => {
    onCallEnd?.(elapsedSeconds, callOutcome || 'completed');
    onClose();
  }, [elapsedSeconds, callOutcome, onCallEnd, onClose]);

  const toggleHold = () => {
    setCallStatus(prev => prev === 'on-hold' ? 'connected' : 'on-hold');
  };

  const getStatusColor = () => {
    switch (callStatus) {
      case 'ringing': return 'text-amber-500';
      case 'connected': return 'text-emerald-500';
      case 'on-hold': return 'text-violet-500';
      case 'ended': return 'text-muted-foreground';
    }
  };

  const getStatusLabel = () => {
    switch (callStatus) {
      case 'ringing': return 'Appel en cours...';
      case 'connected': return 'En communication';
      case 'on-hold': return 'En attente';
      case 'ended': return 'Appel terminé';
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={() => { if (callStatus === 'ended') handleEndCall(); else handleHangup(); }}>
      <DialogContent className="max-w-sm p-0 gap-0 overflow-hidden rounded-2xl w-[92vw] sm:w-full border-0 shadow-2xl">
        {/* Header gradient */}
        <div className={`relative p-6 pb-8 text-center ${
          callStatus === 'ringing' ? 'bg-gradient-to-b from-amber-600 to-amber-700' :
          callStatus === 'connected' ? 'bg-gradient-to-b from-emerald-600 to-emerald-700' :
          callStatus === 'on-hold' ? 'bg-gradient-to-b from-violet-600 to-violet-700' :
          'bg-gradient-to-b from-muted-foreground/80 to-muted-foreground/60'
        }`}>
          {/* Pulse ring animation for ringing */}
          {callStatus === 'ringing' && (
            <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
              <div className="w-24 h-24 rounded-full border-2 border-white/20 animate-ping" />
            </div>
          )}

          {/* Avatar */}
          <div className="relative mx-auto w-20 h-20 rounded-full bg-white/20 backdrop-blur-sm flex items-center justify-center mb-4 border-2 border-white/30">
            <User className="w-10 h-10 text-white" />
          </div>

          <h3 className="text-lg font-bold text-white truncate">{leadName}</h3>
          <p className="text-white/70 text-sm font-mono mt-1">{leadPhone}</p>
          <Badge className="mt-2 bg-white/20 text-white border-white/30 text-xs">
            ID: {leadId}
          </Badge>

          {/* Status */}
          <div className="flex items-center justify-center gap-2 mt-4">
            <div className={`w-2 h-2 rounded-full ${
              callStatus === 'ringing' ? 'bg-amber-300 animate-pulse' :
              callStatus === 'connected' ? 'bg-emerald-300 animate-pulse' :
              callStatus === 'on-hold' ? 'bg-violet-300 animate-pulse' :
              'bg-muted-foreground/50'
            }`} />
            <span className="text-white/90 text-sm font-medium">{getStatusLabel()}</span>
          </div>

          {/* Timer */}
          {(callStatus === 'connected' || callStatus === 'on-hold' || callStatus === 'ended') && (
            <div className="flex items-center justify-center gap-1.5 mt-2">
              <Clock className="w-3.5 h-3.5 text-white/60" />
              <span className="text-white font-mono text-lg font-bold">{formatTime(elapsedSeconds)}</span>
            </div>
          )}
        </div>

        {/* Controls */}
        <div className="bg-background p-5">
          {!showOutcome ? (
            <>
              {/* Active call controls */}
              {(callStatus === 'connected' || callStatus === 'on-hold') && (
                <div className="grid grid-cols-4 gap-3 mb-5">
                  <button
                    className={`flex flex-col items-center gap-1.5 p-3 rounded-xl transition-all ${isMuted ? 'bg-destructive/10 text-destructive' : 'bg-muted hover:bg-muted/80 text-foreground'}`}
                    onClick={() => setIsMuted(!isMuted)}
                  >
                    {isMuted ? <MicOff className="w-5 h-5" /> : <Mic className="w-5 h-5" />}
                    <span className="text-[10px] font-medium">{isMuted ? 'Activé' : 'Muet'}</span>
                  </button>
                  <button
                    className={`flex flex-col items-center gap-1.5 p-3 rounded-xl transition-all ${callStatus === 'on-hold' ? 'bg-violet-500/10 text-violet-600' : 'bg-muted hover:bg-muted/80 text-foreground'}`}
                    onClick={toggleHold}
                  >
                    {callStatus === 'on-hold' ? <Play className="w-5 h-5" /> : <Pause className="w-5 h-5" />}
                    <span className="text-[10px] font-medium">{callStatus === 'on-hold' ? 'Reprendre' : 'Attente'}</span>
                  </button>
                  <button
                    className={`flex flex-col items-center gap-1.5 p-3 rounded-xl transition-all ${isSpeakerOff ? 'bg-destructive/10 text-destructive' : 'bg-muted hover:bg-muted/80 text-foreground'}`}
                    onClick={() => setIsSpeakerOff(!isSpeakerOff)}
                  >
                    {isSpeakerOff ? <VolumeX className="w-5 h-5" /> : <Volume2 className="w-5 h-5" />}
                    <span className="text-[10px] font-medium">{isSpeakerOff ? 'Son off' : 'Volume'}</span>
                  </button>
                  <button className="flex flex-col items-center gap-1.5 p-3 rounded-xl bg-muted hover:bg-muted/80 text-foreground transition-all">
                    <PhoneForwarded className="w-5 h-5" />
                    <span className="text-[10px] font-medium">Transférer</span>
                  </button>
                </div>
              )}

              {/* Ringing indicator */}
              {callStatus === 'ringing' && (
                <div className="text-center py-4 mb-4">
                  <div className="flex items-center justify-center gap-1.5">
                    <PhoneCall className="w-4 h-4 text-amber-500 animate-bounce" />
                    <span className="text-sm text-muted-foreground">Connexion en cours...</span>
                  </div>
                </div>
              )}

              {/* Hang up button */}
              <Button
                onClick={handleHangup}
                className="w-full h-14 rounded-xl bg-destructive hover:bg-destructive/90 text-destructive-foreground font-semibold text-base gap-2 shadow-lg shadow-destructive/20"
              >
                <PhoneOff className="w-5 h-5" />
                Raccrocher
              </Button>
            </>
          ) : (
            /* Post-call outcome */
            <div className="space-y-4">
              <div className="text-center mb-2">
                <p className="text-sm font-medium text-foreground">Résultat de l'appel</p>
                <p className="text-xs text-muted-foreground">Durée: {formatTime(elapsedSeconds)}</p>
              </div>
              <div className="grid grid-cols-2 gap-2">
                {[
                  { value: 'joignable', label: '✅ Joignable', active: 'bg-emerald-500/10 border-emerald-500/30 text-emerald-700' },
                  { value: 'non_joignable', label: '❌ Non joignable', active: 'bg-destructive/10 border-destructive/30 text-destructive' },
                  { value: 'rappeler', label: '🔄 À rappeler', active: 'bg-amber-500/10 border-amber-500/30 text-amber-700' },
                  { value: 'interesse', label: '⭐ Intéressé', active: 'bg-primary/10 border-primary/30 text-primary' },
                ].map(o => (
                  <button
                    key={o.value}
                    className={`p-3 rounded-xl border text-sm font-medium transition-all ${
                      callOutcome === o.value ? o.active : 'border-border hover:bg-muted text-foreground'
                    }`}
                    onClick={() => setCallOutcome(o.value)}
                  >
                    {o.label}
                  </button>
                ))}
              </div>
              <Button onClick={handleEndCall} className="w-full h-12 rounded-xl font-semibold" disabled={!callOutcome}>
                <MessageSquare className="w-4 h-4 mr-2" />
                Terminer & Enregistrer
              </Button>
            </div>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}
