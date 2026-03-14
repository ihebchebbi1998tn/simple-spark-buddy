import { motion, AnimatePresence } from 'framer-motion';

interface FormProgressProps {
  completedFields: number;
  totalFields: number;
  showCelebration?: boolean;
  className?: string;
}

export function FormProgress({
  completedFields,
  totalFields,
  showCelebration = true,
  className = ''
}: FormProgressProps) {
  const progressPercentage = (completedFields / totalFields) * 100;
  const isComplete = completedFields === totalFields;

  return (
    <AnimatePresence>
      {completedFields > 0 && (
        <motion.div
          className={`flex items-center justify-between p-4 rounded-lg bg-gradient-to-r from-primary/5 to-transparent border border-primary/10 ${className}`}
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          exit={{ opacity: 0, y: -20 }}
          transition={{ duration: 0.4 }}
        >
          <div className="flex items-center gap-3">
            <motion.div className="relative w-12 h-12">
              {/* Circular progress */}
              <svg className="w-12 h-12 transform -rotate-90">
                <circle
                  cx="24"
                  cy="24"
                  r="20"
                  stroke="currentColor"
                  strokeWidth="4"
                  fill="none"
                  className="text-muted/20"
                />
                <motion.circle
                  cx="24"
                  cy="24"
                  r="20"
                  stroke="currentColor"
                  strokeWidth="4"
                  fill="none"
                  className="text-primary"
                  strokeLinecap="round"
                  initial={{ strokeDasharray: '0 125.6' }}
                  animate={{ strokeDasharray: `${progressPercentage * 1.256} 125.6` }}
                  transition={{ duration: 0.5 }}
                />
              </svg>
              <div className="absolute inset-0 flex items-center justify-center">
                <motion.span
                  key={completedFields}
                  initial={{ scale: 0.5, opacity: 0 }}
                  animate={{ scale: 1, opacity: 1 }}
                  className="text-xs font-bold text-primary"
                >
                  {completedFields}
                </motion.span>
              </div>
            </motion.div>
            <div>
              <p className="text-xs sm:text-sm font-semibold text-foreground">
                {completedFields} / {totalFields} champs complétés
              </p>
              <p className="text-[10px] sm:text-xs text-muted-foreground">
                {totalFields - completedFields} restant{totalFields - completedFields > 1 ? 's' : ''}
              </p>
            </div>
          </div>
          
          <AnimatePresence>
            {showCelebration && isComplete && (
              <motion.div
                initial={{ opacity: 0, scale: 0.8, x: 20 }}
                animate={{ opacity: 1, scale: 1, x: 0 }}
                exit={{ opacity: 0, scale: 0.8 }}
                className="flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-green-500 to-emerald-500 text-white shadow-[var(--shadow-success)]"
              >
                <motion.span
                  animate={{ rotate: [0, 10, -10, 0] }}
                  transition={{ duration: 0.5, repeat: Infinity, repeatDelay: 2 }}
                >
                  🎉
                </motion.span>
                <span className="text-xs sm:text-sm font-semibold">Complet !</span>
              </motion.div>
            )}
          </AnimatePresence>
        </motion.div>
      )}
    </AnimatePresence>
  );
}
