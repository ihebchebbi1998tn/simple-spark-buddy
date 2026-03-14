import { useState, useRef, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Progress } from '@/components/ui/progress';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import {
    Upload, AlertTriangle, CheckCircle2, ArrowLeft, Trash2, FileSpreadsheet,
    Download, Eye, EyeOff, AlertCircle, XCircle
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

// ============================================
// TYPES & INTERFACES
// ============================================

interface ExcelLead {
    // Row metadata
    rowNumber: number;

    // Facebook/Instagram metadata
    'Numéro'?: number;
    'id': string;
    'created_time': string;
    'ad_id'?: string;
    'ad_name'?: string;
    'adset_id'?: string;
    'adset_name'?: string;
    'campaign_id': string;
    'campaign_name': string;
    'form_id'?: string;
    'form_name'?: string;
    'is_organic': string;
    'platform': string;

    // Lead profile data
    'Expérience globale en centres d\'appels'?: string;
    'Langue étrangère maîtrisée'?: string;
    'Activité recherchée'?: string;
    'Expérience activité recherchée'?: string;
    'Opération recherchée'?: string;
    'Expérience dans l\'opération recherchée'?: string;
    'Type de travail recherché'?: string;
    'Temps de travail recherché'?: string;
    'Blacklist Centres d\'appels'?: string; // Skip for now

    // Personal information
    'Nom et Prénom': string;
    'Numéro de téléphone': string;
    'E-mail': string;
    'Ville'?: string;
    'Date insertion'?: string;
    'Heure insertion'?: string;
}

type ValidationStatus = 'ok' | 'warning' | 'error' | 'duplicate';

interface ValidationIssue {
    type: 'error' | 'warning';
    field: string;
    message: string;
}

interface ValidatedLead {
    rowNumber: number;
    status: ValidationStatus;
    data: ExcelLead;
    issues: ValidationIssue[];
    isDuplicate: boolean;
    duplicateEmail?: string;
    action: 'import' | 'skip' | 'update';
}

interface ImportStats {
    total: number;
    ok: number;
    warnings: number;
    errors: number;
    duplicates: number;
}

// ============================================
// VALUE MAPPING DICTIONARIES
// ============================================

const EXPERIENCE_MAP: Record<string, number> = {
    "0_-_6_mois": 0,
    "7_mois_-_1_an": 1,
    "1_-_3_ans": 2,
    "3_-_5_ans": 4,
    "5_-_7_ans": 6,
    "plus_que_7_ans": 8
};

const LANGUAGE_MAP: Record<string, string> = {
    "français": "FR",
    "anglais": "EN",
    "arabe": "AR",
    "espagnol": "ES",
    "italien": "IT",
    "allemand": "DE",
    "turc": "TR",
    "russe": "RU",
    "chinois": "ZH",
    "portugais": "PT"
};

const ACTIVITY_MAP: Record<string, number> = {
    "Prise de rendez-vous": 1,
    "Télévente": 2,
    "SAV": 3,
    "Support technique": 4,
    "Enquêtes": 5,
    "Téléprospection": 6,
    "Fidélisation": 7,
    "Recouvrement": 8,
    "Back office": 9,
    "Autre": 99
};

const SECTOR_MAP: Record<string, number> = {
    "Télécom - B2B": 1,
    "Télécom - B2C": 2,
    "Enérgie - Eléctricité et Gaz": 3,
    "Banque - Finance": 4,
    "Assurance": 5,
    "E-commerce": 6,
    "Immobilier": 7,
    "Santé": 8,
    "Éducation": 9,
    "Voyage - Tourisme": 10,
    "Automobile": 11,
    "Services publics": 12,
    "Retail": 13,
    "Technologie": 14,
    "Autre": 99
};

const WORK_MODE_MAP: Record<string, number> = {
    "télétravail": 1,
    "présentiel": 2,
    "hybride": 3,
    "peu_importe": 0
};

const SCHEDULE_MAP: Record<string, number> = {
    "temps_plein": 1,
    "temps_partiel": 2,
    "peu_importe": 0
};

// ============================================
// MAIN COMPONENT
// ============================================

export function LeadImportPage({ onBack }: { onBack: () => void }) {
    const { toast } = useToast();

    // State
    const [file, setFile] = useState<File | null>(null);
    const [validatedLeads, setValidatedLeads] = useState<ValidatedLead[]>([]);
    const [stats, setStats] = useState<ImportStats>({ total: 0, ok: 0, warnings: 0, errors: 0, duplicates: 0 });
    const [isLoading, setIsLoading] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [showPreview, setShowPreview] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [editingRow, setEditingRow] = useState<number | null>(null);
    const [showOnlyProblems, setShowOnlyProblems] = useState(false);
    const [itemsPerPage, setItemsPerPage] = useState(20);
    const [editingRowOriginalData, setEditingRowOriginalData] = useState<ExcelLead | null>(null);

    // Ref for main table scroll
    const mainScrollRef = useRef<HTMLDivElement>(null);
    const scrollbarRef = useRef<HTMLDivElement>(null);

    // Sync table scroll with sticky scrollbar
    useEffect(() => {
        const handleScroll = (e: Event) => {
            const target = e.target as HTMLDivElement;
            if (scrollbarRef.current) {
                scrollbarRef.current.scrollLeft = target.scrollLeft;
            }
        };

        const main = mainScrollRef.current;
        if (main) {
            main.addEventListener('scroll', handleScroll, { passive: true });
            return () => main.removeEventListener('scroll', handleScroll);
        }
    }, []);

    // ============================================
    // VALIDATION FUNCTIONS
    // ============================================

    const validateEmail = (email: string): boolean => {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    };

    const validatePhone = (phone: string): boolean => {
        // E.164 format: starts with +
        return /^\+[1-9]\d{1,14}$/.test(phone);
    };

    const validateLead = async (lead: ExcelLead, rowNumber: number): Promise<ValidatedLead> => {
        const issues: ValidationIssue[] = [];
        let status: ValidationStatus = 'ok';
        let isDuplicate = false;
        let duplicateEmail: string | undefined;

        // ========== CRITICAL VALIDATIONS (ERRORS) ==========

        // Email required
        if (!lead['E-mail']) {
            issues.push({ type: 'error', field: 'E-mail', message: 'Email manquant' });
            status = 'error';
        } else if (!validateEmail(lead['E-mail'])) {
            issues.push({ type: 'error', field: 'E-mail', message: 'Format email invalide' });
            status = 'error';
        }

        // Phone required
        if (!lead['Numéro de téléphone']) {
            issues.push({ type: 'error', field: 'Téléphone', message: 'Téléphone manquant' });
            status = 'error';
        } else if (!validatePhone(lead['Numéro de téléphone'])) {
            issues.push({ type: 'error', field: 'Téléphone', message: 'Format téléphone invalide (doit commencer par +)' });
            status = 'error';
        }

        // Name required
        if (!lead['Nom et Prénom'] || lead['Nom et Prénom'].trim() === '') {
            issues.push({ type: 'error', field: 'Nom', message: 'Nom manquant' });
            status = 'error';
        }

        // Facebook ID required
        if (!lead.id) {
            issues.push({ type: 'error', field: 'ID', message: 'ID Facebook manquant' });
            status = 'error';
        }

        // Created time required
        if (!lead.created_time) {
            issues.push({ type: 'error', field: 'Date', message: 'Date de création manquante' });
            status = 'error';
        }

        // ========== DUPLICATE CHECK ==========
        // Only check if no errors so far
        if (status !== 'error' && lead['E-mail']) {
            // TODO: Replace with actual API call to check database
            const existingLead = await checkDuplicateEmail(lead['E-mail']);
            if (existingLead) {
                isDuplicate = true;
                duplicateEmail = lead['E-mail'];
                status = 'duplicate';
                issues.push({
                    type: 'warning',
                    field: 'Email',
                    message: `Email existe déjà dans la base de données`
                });
            }
        }

        // ========== WARNING VALIDATIONS ==========

        // Unknown language
        const language = lead['Langue étrangère maîtrisée']?.trim();
        if (language && LANGUAGE_MAP[language] === undefined) {
            issues.push({
                type: 'warning',
                field: 'Langue',
                message: `Langue inconnue: "${language}"`
            });
            if (status === 'ok') status = 'warning';
        }

        // Unknown activity
        const activity = lead['Activité recherchée']?.trim();
        if (activity && ACTIVITY_MAP[activity] === undefined) {
            issues.push({
                type: 'warning',
                field: 'Activité',
                message: `Activité inconnue: "${activity}"`
            });
            if (status === 'ok') status = 'warning';
        }

        // Unknown sector
        const sector = lead['Opération recherchée']?.trim();
        if (sector && SECTOR_MAP[sector] === undefined) {
            issues.push({
                type: 'warning',
                field: 'Secteur',
                message: `Secteur inconnu: "${sector}"`
            });
            if (status === 'ok') status = 'warning';
        }

        // Unknown work mode
        const workMode = lead['Type de travail recherché']?.trim();
        if (workMode && WORK_MODE_MAP[workMode] === undefined) {
            issues.push({
                type: 'warning',
                field: 'Mode de travail',
                message: `Mode de travail inconnu: "${workMode}"`
            });
            if (status === 'ok') status = 'warning';
        }

        // Unknown schedule
        const schedule = lead['Temps de travail recherché']?.trim();
        if (schedule && SCHEDULE_MAP[schedule] === undefined) {
            issues.push({
                type: 'warning',
                field: 'Temps de travail',
                message: `Temps de travail inconnu: "${schedule}"`
            });
            if (status === 'ok') status = 'warning';
        }

        return {
            rowNumber,
            status,
            data: lead,
            issues,
            isDuplicate,
            duplicateEmail,
            action: 'skip' // User must explicitly select action
        };
    };

    // Mock function - replace with actual API call
    const checkDuplicateEmail = async (email: string): Promise<boolean> => {
        // TODO: Call backend API to check if email exists
        // const response = await fetch(`/api/leads/check-duplicate?email=${email}`);
        // return response.json();
        return false; // Placeholder
    };

    // ============================================
    // FILE UPLOAD & PARSING
    // ============================================

    const handleFileUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = e.target.files?.[0];
        if (!selectedFile) return;

        // Check file extension
        const fileName = selectedFile.name.toLowerCase();
        if (!fileName.endsWith('.xlsx') && !fileName.endsWith('.xls')) {
            toast({
                title: 'Format non supporté',
                description: 'Veuillez télécharger un fichier Excel (.xlsx, .xls)',
                variant: 'destructive'
            });
            return;
        }

        setIsLoading(true);
        setValidatedLeads([]);
        setFile(selectedFile);

        try {
            // Import XLSX library
            const XLSX = await import('xlsx');

            const arrayBuffer = await selectedFile.arrayBuffer();
            const workbook = XLSX.read(arrayBuffer, { type: 'array' });
            const worksheet = workbook.Sheets[workbook.SheetNames[0]];
            const jsonData = XLSX.utils.sheet_to_json(worksheet) as ExcelLead[];

            // Validate each lead
            const validated: ValidatedLead[] = [];
            for (let i = 0; i < jsonData.length; i++) {
                const lead = jsonData[i];
                const validatedLead = await validateLead(lead, i + 1);
                validated.push(validatedLead);
            }

            setValidatedLeads(validated);
            setCurrentPage(1);

            // Calculate stats
            const newStats: ImportStats = {
                total: validated.length,
                ok: validated.filter(l => l.status === 'ok').length,
                warnings: validated.filter(l => l.status === 'warning').length,
                errors: validated.filter(l => l.status === 'error').length,
                duplicates: validated.filter(l => l.status === 'duplicate').length
            };
            setStats(newStats);

            toast({
                title: 'Fichier analysé',
                description: `${validated.length} lignes analysées. ${newStats.ok} OK, ${newStats.warnings} avertissements, ${newStats.errors} erreurs, ${newStats.duplicates} doublons.`
            });

        } catch (error) {
            console.error('Error parsing Excel:', error);
            toast({
                title: 'Erreur',
                description: 'Impossible de lire le fichier Excel',
                variant: 'destructive'
            });
        } finally {
            setIsLoading(false);
        }
    };

    // ============================================
    // ACTION HANDLERS
    // ============================================

    const handleActionChange = (rowNumber: number, action: 'import' | 'skip' | 'update') => {
        setValidatedLeads(prev => prev.map(lead =>
            lead.rowNumber === rowNumber ? { ...lead, action } : lead
        ));
    };

    const handleFieldEdit = async (rowNumber: number, fieldName: string, newValue: string) => {
        setValidatedLeads(prev => {
            return prev.map(lead => {
                if (lead.rowNumber === rowNumber) {
                    // Check if actual change occurred
                    const hasActualChange = fieldName !== '' && lead.data[fieldName as keyof ExcelLead] !== newValue;

                    if (hasActualChange) {
                        // Update the data
                        const updatedData = { ...lead.data, [fieldName]: newValue };
                        // Revalidate only if data actually changed
                        const revalidated = validateLeadSync(updatedData, rowNumber);
                        return revalidated;
                    } else if (editingRowOriginalData && JSON.stringify(lead.data) !== JSON.stringify(editingRowOriginalData)) {
                        // Data was changed from original, revalidate the current data
                        const revalidated = validateLeadSync(lead.data, rowNumber);
                        return revalidated;
                    }
                    // If nothing changed and we're just closing edit mode, return lead unchanged
                    return lead;
                }
                return lead;
            });
        });
        setEditingRow(null);
        setEditingRowOriginalData(null);
    };

    // Synchronous validation for inline edits
    const validateLeadSync = (lead: ExcelLead, rowNumber: number): ValidatedLead => {
        const issues: ValidationIssue[] = [];
        let status: ValidationStatus = 'ok';
        const isDuplicate = false;
        let duplicateEmail: string | undefined;

        // Email validation
        if (!lead['E-mail']) {
            issues.push({ type: 'error', field: 'E-mail', message: 'Email manquant' });
            status = 'error';
        } else if (!validateEmail(lead['E-mail'])) {
            issues.push({ type: 'error', field: 'E-mail', message: 'Format email invalide' });
            status = 'error';
        }

        // Phone validation
        if (!lead['Numéro de téléphone']) {
            issues.push({ type: 'error', field: 'Téléphone', message: 'Téléphone manquant' });
            status = 'error';
        } else if (!validatePhone(lead['Numéro de téléphone'])) {
            issues.push({ type: 'error', field: 'Téléphone', message: 'Format téléphone invalide (doit commencer par +)' });
            status = 'error';
        }

        // Name validation
        if (!lead['Nom et Prénom'] || lead['Nom et Prénom'].trim() === '') {
            issues.push({ type: 'error', field: 'Nom', message: 'Nom manquant' });
            status = 'error';
        }

        // Work mode
        const workMode = lead['Type de travail recherché']?.trim();
        if (workMode && WORK_MODE_MAP[workMode] === undefined) {
            issues.push({ type: 'warning', field: 'Mode de travail', message: `Mode de travail inconnu: "${workMode}"` });
            if (status === 'ok') status = 'warning';
        }

        return {
            rowNumber,
            status,
            data: lead,
            issues,
            isDuplicate,
            duplicateEmail,
            action: 'skip' // User must explicitly select action
        };
    };

    const handleSubmit = async () => {
        setIsSubmitting(true);

        try {
            // Get leads to import
            const leadsToImport = validatedLeads.filter(l => l.action === 'import');
            const leadsToUpdate = validatedLeads.filter(l => l.action === 'update');

            // TODO: Call backend API to import leads
            // const response = await fetch('/api/leads/import', {
            //     method: 'POST',
            //     headers: { 'Content-Type': 'application/json' },
            //     body: JSON.stringify({
            //         imports: leadsToImport.map(l => l.data),
            //         updates: leadsToUpdate.map(l => l.data)
            //     })
            // });

            toast({
                title: 'Import réussi',
                description: `${leadsToImport.length} leads importés, ${leadsToUpdate.length} mis à jour`
            });

            // Clear form
            handleClear();

        } catch (error) {
            console.error('Import error:', error);
            toast({
                title: 'Erreur d\'importation',
                description: 'Une erreur est survenue lors de l\'importation',
                variant: 'destructive'
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleClear = () => {
        setFile(null);
        setValidatedLeads([]);
        setStats({ total: 0, ok: 0, warnings: 0, errors: 0, duplicates: 0 });
        setCurrentPage(1);
    };

    const handleDownloadTemplate = async () => {
        try {
            const XLSX = await import('xlsx');

            // Create template data with headers and example row
            const templateData = [
                {
                    'Numéro': 1,
                    'id': 'facebook_lead_id_123456',
                    'created_time': '2026-02-28 10:30:00',
                    'ad_id': 'ad_123456',
                    'ad_name': 'Campaign Ad Name',
                    'adset_id': 'adset_123456',
                    'adset_name': 'Ad Set Name',
                    'campaign_id': 'campaign_123456',
                    'campaign_name': 'Campaign Name',
                    'form_id': 'form_123456',
                    'form_name': 'Lead Form Name',
                    'is_organic': 'false',
                    'platform': 'fb',
                    'Nom et Prénom': 'xxxx yyyyy',
                    'E-mail': 'xxxx.yyyyy@example.com',
                    'Numéro de téléphone': '+21612345678',
                    'Ville': 'Paris',
                    'Expérience globale en centres d\'appels': '1_-_3_ans',
                    'Langue étrangère maîtrisée': 'anglais',
                    'Activité recherchée': 'Télévente',
                    'Expérience activité recherchée': '1_-_3_ans',
                    'Opération recherchée': 'Télécom - B2C',
                    'Expérience dans l\'opération recherchée': '3_-_5_ans',
                    'Type de travail recherché': 'télétravail',
                    'Temps de travail recherché': 'temps_plein'
                }
            ];

            // Create workbook
            const workbook = XLSX.utils.book_new();
            const worksheet = XLSX.utils.json_to_sheet(templateData);

            // Set column widths
            const columnWidths = [
                { wch: 12 },  // Numéro
                { wch: 25 },  // id
                { wch: 20 },  // created_time
                { wch: 15 },  // ad_id
                { wch: 20 },  // ad_name
                { wch: 15 },  // adset_id
                { wch: 20 },  // adset_name
                { wch: 18 },  // campaign_id
                { wch: 20 },  // campaign_name
                { wch: 15 },  // form_id
                { wch: 20 },  // form_name
                { wch: 12 },  // is_organic
                { wch: 12 },  // platform
                { wch: 20 },  // Nom et Prénom
                { wch: 25 },  // E-mail
                { wch: 18 },  // Numéro de téléphone
                { wch: 15 },  // Ville
                { wch: 25 },  // Expérience globale
                { wch: 20 },  // Langue
                { wch: 20 },  // Activité recherchée
                { wch: 25 },  // Exp. Activité
                { wch: 20 },  // Opération recherchée
                { wch: 25 },  // Exp. Opération
                { wch: 20 },  // Type de travail
                { wch: 20 }   // Temps de travail
            ];
            worksheet['!cols'] = columnWidths;

            XLSX.utils.book_append_sheet(workbook, worksheet, 'Leads');

            // Download file
            XLSX.writeFile(workbook, 'Lead_Import_Template.xlsx');

            toast({
                title: 'Modèle téléchargé',
                description: 'Le modèle Excel a été téléchargé avec succès'
            });
        } catch (error) {
            console.error('Template download error:', error);
            toast({
                title: 'Erreur',
                description: 'Impossible de télécharger le modèle',
                variant: 'destructive'
            });
        }
    };

    const getStatusColor = (status: ValidationStatus): string => {
        switch (status) {
            case 'ok': return 'text-emerald-600 bg-emerald-50 border-emerald-200';
            case 'warning': return 'text-amber-600 bg-amber-50 border-amber-200';
            case 'error': return 'text-red-600 bg-red-50 border-red-200';
            case 'duplicate': return 'text-blue-600 bg-blue-50 border-blue-200';
        }
    };

    const getStatusIcon = (status: ValidationStatus) => {
        switch (status) {
            case 'ok': return <CheckCircle2 className="w-4 h-4" />;
            case 'warning': return <AlertTriangle className="w-4 h-4" />;
            case 'error': return <XCircle className="w-4 h-4" />;
            case 'duplicate': return <AlertCircle className="w-4 h-4" />;
        }
    };

    const getStatusLabel = (status: ValidationStatus): string => {
        switch (status) {
            case 'ok': return 'OK';
            case 'warning': return 'Avertissement';
            case 'error': return 'Erreur';
            case 'duplicate': return 'Doublon';
        }
    };

    const parseName = (fullName: string): { firstName: string; lastName: string } => {
        const parts = fullName.trim().split(' ');
        return {
            firstName: parts[0] || '',
            lastName: parts.slice(1).join(' ') || ''
        };
    };

    // ============================================
    // TABLE FIELD CONFIGURATION
    // ============================================

    const allFields = [
        // Personal information
        { key: 'Nom et Prénom', label: 'Nom' },
        { key: 'E-mail', label: 'Email' },
        { key: 'Numéro de téléphone', label: 'Téléphone' },
        { key: 'Ville', label: 'Ville' },

        // Lead profile data
        { key: 'Expérience globale en centres d\'appels', label: 'Expérience globale' },
        { key: 'Langue étrangère maîtrisée', label: 'Langue' },
        { key: 'Activité recherchée', label: 'Activité' },
        { key: 'Expérience activité recherchée', label: 'Exp. activité' },
        { key: 'Opération recherchée', label: 'Opération' },
        { key: 'Expérience dans l\'opération recherchée', label: 'Exp. opération' },
        { key: 'Type de travail recherché', label: 'Type de travail' },
        { key: 'Temps de travail recherché', label: 'Temps de travail' },

        // Facebook/Instagram metadata
        { key: 'id', label: 'ID Facebook' },
        { key: 'created_time', label: 'Date création' },
        { key: 'ad_id', label: 'Ad ID' },
        { key: 'ad_name', label: 'Ad Name' },
        { key: 'campaign_id', label: 'Campaign ID' },
        { key: 'campaign_name', label: 'Campaign Name' },
        { key: 'form_id', label: 'Form ID' },
        { key: 'form_name', label: 'Form Name' },
        { key: 'platform', label: 'Platform' },
        { key: 'is_organic', label: 'Is Organic' }
    ];

    const renderFieldValue = (lead: ValidatedLead, fieldKey: string) => {
        return (lead.data as ExcelLead)[fieldKey as keyof ExcelLead] || '';
    };

    // ============================================
    // PAGINATION HELPERS
    // ============================================

    const filteredLeads = showOnlyProblems
        ? validatedLeads.filter(l => l.issues.length > 0)
        : validatedLeads;

    const totalPages = Math.ceil(filteredLeads.length / itemsPerPage);
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const currentLeads = filteredLeads.slice(startIndex, endIndex);

    // ============================================
    // RENDER
    // ============================================

    return (
        <div className="space-y-6 max-w-full mx-auto px-6">
            {/* Header */}
            <div className="flex items-center gap-4">
                <Button variant="ghost" size="sm" onClick={onBack}>
                    <ArrowLeft className="w-4 h-4 mr-2" />
                    Retour
                </Button>
                <div>
                    <h1 className="text-2xl font-semibold">Import de leads</h1>
                    <p className="text-sm text-muted-foreground">
                        Importer des leads via Excel
                    </p>
                </div>
            </div>

            {/* Upload Section */}
            <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                    <div>
                        <CardTitle>Télécharger le fichier Excel</CardTitle>
                        <CardDescription>
                            Fichier d'export Facebook Leads (.xlsx ou .xls)
                        </CardDescription>
                    </div>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={handleDownloadTemplate}
                        className="flex items-center gap-2"
                    >
                        <Download className="w-4 h-4" />
                        Télécharger le modèle
                    </Button>
                </CardHeader>
                <CardContent className="space-y-4">
                    <div className="border-2 border-dashed rounded-lg p-8 text-center hover:border-primary/50 transition-colors">
                        <input
                            type="file"
                            id="file-upload"
                            className="hidden"
                            accept=".xlsx,.xls"
                            onChange={handleFileUpload}
                            disabled={isLoading || isSubmitting}
                        />
                        <label htmlFor="file-upload" className="cursor-pointer">
                            <Upload className="w-12 h-12 mx-auto mb-4 text-muted-foreground" />
                            <p className="text-sm font-medium mb-1">
                                Cliquez pour sélectionner un fichier
                            </p>
                            <p className="text-xs text-muted-foreground">
                                Excel (.xlsx, .xls) - Max 10 MB
                            </p>
                        </label>
                    </div>

                    {file && (
                        <div className="flex items-center justify-between p-3 bg-muted/30 rounded-lg">
                            <div className="flex items-center gap-3">
                                <FileSpreadsheet className="w-5 h-5 text-emerald-600" />
                                <div>
                                    <p className="text-sm font-medium">{file.name}</p>
                                    <p className="text-xs text-muted-foreground">
                                        {(file.size / 1024 / 1024).toFixed(2)} MB
                                    </p>
                                </div>
                            </div>
                            <Button
                                variant="ghost"
                                size="sm"
                                onClick={handleClear}
                                disabled={isLoading || isSubmitting}
                            >
                                <Trash2 className="w-4 h-4" />
                            </Button>
                        </div>
                    )}

                    {isLoading && (
                        <div className="flex items-center justify-center py-8">
                            <div className="w-8 h-8 animate-spin rounded-full border-4 border-primary border-t-transparent" />
                            <span className="ml-3 text-sm text-muted-foreground">Analyse en cours...</span>
                        </div>
                    )}
                </CardContent>
            </Card>

            {/* Stats Section */}
            {stats.total > 0 && (
                <Card>
                    <CardHeader>
                        <CardTitle className="text-lg">Résumé de l'analyse</CardTitle>
                    </CardHeader>
                    <CardContent className="space-y-4">
                        <div className="grid grid-cols-2 sm:grid-cols-5 gap-4">
                            <div className="p-4 bg-muted/30 rounded-lg border">
                                <p className="text-sm text-muted-foreground">Total</p>
                                <p className="text-2xl font-semibold">{stats.total}</p>
                            </div>
                            <div className="p-4 bg-emerald-50 rounded-lg border border-emerald-200">
                                <p className="text-sm text-emerald-700">OK</p>
                                <p className="text-2xl font-semibold text-emerald-600">{stats.ok}</p>
                            </div>
                            <div className="p-4 bg-amber-50 rounded-lg border border-amber-200">
                                <p className="text-sm text-amber-700">Avertissements</p>
                                <p className="text-2xl font-semibold text-amber-600">{stats.warnings}</p>
                            </div>
                            <div className="p-4 bg-red-50 rounded-lg border border-red-200">
                                <p className="text-sm text-red-700">Erreurs</p>
                                <p className="text-2xl font-semibold text-red-600">{stats.errors}</p>
                            </div>
                            <div className="p-4 bg-blue-50 rounded-lg border border-blue-200">
                                <p className="text-sm text-blue-700">Doublons</p>
                                <p className="text-2xl font-semibold text-blue-600">{stats.duplicates}</p>
                            </div>
                        </div>

                        <div>
                            <div className="flex items-center justify-between mb-2">
                                <span className="text-sm font-medium">Taux de validité</span>
                                <span className="text-sm text-muted-foreground">
                                    {stats.total > 0 ? Math.round(((stats.ok + stats.warnings) / stats.total) * 100) : 0}%
                                </span>
                            </div>
                            <Progress
                                value={stats.total > 0 ? ((stats.ok + stats.warnings) / stats.total) * 100 : 0}
                                className="h-2"
                            />
                        </div>
                    </CardContent>
                </Card>
            )}

            {/* Preview Table */}
            {validatedLeads.length > 0 && (
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between">
                        <div>
                            <CardTitle className="text-lg">Aperçu des leads ({filteredLeads.length}{showOnlyProblems && ` / ${validatedLeads.length}`})</CardTitle>
                            <CardDescription>
                                Vérifiez et décidez de l'action pour chaque lead
                            </CardDescription>
                        </div>
                        <div className="flex items-center gap-2">
                            <Button
                                variant={showOnlyProblems ? 'default' : 'outline'}
                                size="sm"
                                onClick={() => {
                                    setShowOnlyProblems(!showOnlyProblems);
                                    setCurrentPage(1);
                                }}
                            >
                                ⚠️ Problèmes uniquement
                            </Button>
                            <Button
                                variant="outline"
                                size="sm"
                                onClick={() => setShowPreview(!showPreview)}
                            >
                                {showPreview ? (
                                    <>
                                        <EyeOff className="w-3.5 h-3.5 mr-1.5" /> Masquer
                                    </>
                                ) : (
                                    <>
                                        <Eye className="w-3.5 h-3.5 mr-1.5" /> Voir
                                    </>
                                )}
                            </Button>
                        </div>
                    </CardHeader>

                    {showPreview && (
                        <CardContent className="space-y-4">
                            {/* Status Legend */}
                            <div className="bg-muted/40 border border-dashed rounded-lg p-3 mb-4">
                                <div className="flex items-center gap-4 flex-wrap">
                                    <p className="text-sm font-semibold">Légende du statut:</p>
                                    <div className="flex items-center gap-4 flex-wrap">
                                        <div className="flex items-center gap-2">
                                            <Badge variant="outline" className="text-emerald-600 bg-emerald-50 border-emerald-200 flex items-center justify-center w-8 h-8 p-0">
                                                <CheckCircle2 className="w-4 h-4" />
                                            </Badge>
                                            <span className="text-xs">OK</span>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <Badge variant="outline" className="text-amber-600 bg-amber-50 border-amber-200 flex items-center justify-center w-8 h-8 p-0">
                                                <AlertTriangle className="w-4 h-4" />
                                            </Badge>
                                            <span className="text-xs">Avertissement</span>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <Badge variant="outline" className="text-red-600 bg-red-50 border-red-200 flex items-center justify-center w-8 h-8 p-0">
                                                <XCircle className="w-4 h-4" />
                                            </Badge>
                                            <span className="text-xs">Erreur</span>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <Badge variant="outline" className="text-blue-600 bg-blue-50 border-blue-200 flex items-center justify-center w-8 h-8 p-0">
                                                <AlertCircle className="w-4 h-4" />
                                            </Badge>
                                            <span className="text-xs">Doublon</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            {/* Items Per Page Selector */}
                            <div className="flex items-center gap-3 mb-4">
                                <span className="text-sm font-medium">Afficher par page:</span>
                                <Select
                                    value={itemsPerPage.toString()}
                                    onValueChange={(value) => {
                                        setItemsPerPage(parseInt(value));
                                        setCurrentPage(1);
                                    }}
                                >
                                    <SelectTrigger className="w-24">
                                        <SelectValue />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectItem value="10">10</SelectItem>
                                        <SelectItem value="20">20</SelectItem>
                                        <SelectItem value="50">50</SelectItem>
                                        <SelectItem value="100">100</SelectItem>
                                    </SelectContent>
                                </Select>
                            </div>

                            {/* Table Container with Sticky Horizontal Scrollbar */}
                            <div className="space-y-1">
                                {/* Scrollable Table */}
                                <div
                                    ref={mainScrollRef}
                                    className="overflow-x-auto border rounded-t-lg"
                                >
                                    <Table className="min-w-full">
                                        <TableHeader>
                                            <TableRow className="sticky top-0 z-50 bg-background">
                                                <TableHead className="sticky left-0 z-50 bg-background w-16">#</TableHead>
                                                <TableHead className="sticky left-16 z-50 bg-background w-24">Statut</TableHead>
                                                <TableHead className="sticky left-40 z-50 bg-background w-72">Problèmes</TableHead>
                                                <TableHead className="sticky left-112 z-50 bg-background w-40">Action</TableHead>
                                                {allFields.map((field) => (
                                                    <TableHead key={field.key} className="whitespace-nowrap min-w-fit bg-background">
                                                        {field.label}
                                                    </TableHead>
                                                ))}
                                            </TableRow>
                                        </TableHeader>
                                        <TableBody>
                                            {currentLeads.map((lead) => {
                                                const name = parseName(lead.data['Nom et Prénom']);
                                                return (
                                                    <TableRow key={lead.rowNumber} className={lead.status === 'error' ? 'bg-red-50/50' : ''}>
                                                        <TableCell className="sticky left-0 z-20 bg-background font-mono text-xs text-muted-foreground w-16">
                                                            {lead.rowNumber}
                                                        </TableCell>
                                                        <TableCell className="sticky left-16 z-20 bg-background w-24 flex justify-center">
                                                            <Badge
                                                                variant="outline"
                                                                className={`${getStatusColor(lead.status)} flex items-center justify-center w-8 h-8 p-0`}
                                                                title={getStatusLabel(lead.status)}
                                                            >
                                                                {getStatusIcon(lead.status)}
                                                            </Badge>
                                                        </TableCell>
                                                        <TableCell className="sticky left-40 z-20 bg-background w-72">
                                                            <div className="flex items-center gap-2">
                                                                {lead.issues.length > 0 ? (
                                                                    <>
                                                                        <div
                                                                            title={lead.issues.map(issue => `${issue.field}: ${issue.message}`).join('\n')}
                                                                            className="cursor-help flex items-center flex-shrink-0"
                                                                        >
                                                                            <AlertCircle className="w-5 h-5 text-red-600" />
                                                                        </div>
                                                                        {editingRow === lead.rowNumber ? (
                                                                            <Button
                                                                                size="sm"
                                                                                variant="default"
                                                                                onClick={() => {
                                                                                    handleFieldEdit(lead.rowNumber, '', '');
                                                                                }}
                                                                                className="h-6 text-xs px-2"
                                                                            >
                                                                                ✓ Enregistrer
                                                                            </Button>
                                                                        ) : (
                                                                            <Button
                                                                                size="sm"
                                                                                variant="outline"
                                                                                onClick={() => {
                                                                                    setEditingRow(lead.rowNumber);
                                                                                    setEditingRowOriginalData({ ...lead.data });
                                                                                }}
                                                                                className="h-6 text-xs px-2"
                                                                            >
                                                                                ✎ Éditer
                                                                            </Button>
                                                                        )}
                                                                    </>
                                                                ) : (
                                                                    <span className="text-xs text-emerald-600">Aucun problème</span>
                                                                )}
                                                            </div>
                                                        </TableCell>
                                                        <TableCell className="sticky left-112 z-20 bg-background w-40">
                                                            <Select
                                                                value={lead.action}
                                                                onValueChange={(value: 'import' | 'skip' | 'update') =>
                                                                    handleActionChange(lead.rowNumber, value)
                                                                }
                                                                disabled={lead.status === 'error'}
                                                            >
                                                                <SelectTrigger className="h-8 text-xs">
                                                                    <SelectValue />
                                                                </SelectTrigger>
                                                                <SelectContent>
                                                                    <SelectItem value="import">✅ Importer</SelectItem>
                                                                    <SelectItem value="skip">⏭️ Ignorer</SelectItem>
                                                                    {lead.isDuplicate && (
                                                                        <SelectItem value="update">🔄 Mettre à jour</SelectItem>
                                                                    )}
                                                                </SelectContent>
                                                            </Select>
                                                        </TableCell>
                                                        {allFields.map((field) => {
                                                            const fieldValue = renderFieldValue(lead, field.key);
                                                            const hasError = lead.issues.some(issue => issue.field === field.label && issue.type === 'error');
                                                            const hasWarning = lead.issues.some(issue => issue.field === field.label && issue.type === 'warning');
                                                            return (
                                                                <TableCell
                                                                    key={field.key}
                                                                    className={`text-xs whitespace-nowrap min-w-fit ${hasError ? 'bg-red-50 border-l-2 border-red-500' :
                                                                        hasWarning ? 'bg-amber-50 border-l-2 border-amber-500' : ''
                                                                        }`}
                                                                >
                                                                    {editingRow === lead.rowNumber ? (
                                                                        <input
                                                                            type={field.key === 'E-mail' ? 'email' : field.key === 'Numéro de téléphone' ? 'tel' : 'text'}
                                                                            value={fieldValue}
                                                                            onChange={(e) => {
                                                                                // Update data immediately while editing
                                                                                setValidatedLeads(prev => prev.map(l =>
                                                                                    l.rowNumber === lead.rowNumber
                                                                                        ? { ...l, data: { ...l.data, [field.key]: e.target.value } }
                                                                                        : l
                                                                                ));
                                                                            }}
                                                                            className="w-full px-2 py-1 border rounded text-xs"
                                                                        />
                                                                    ) : (
                                                                        <span className="block overflow-hidden text-ellipsis">{fieldValue || '-'}</span>
                                                                    )}
                                                                </TableCell>
                                                            );
                                                        })}
                                                    </TableRow>
                                                );
                                            })}
                                        </TableBody>
                                    </Table>
                                </div>

                                {/* Sticky Horizontal Scrollbar at Bottom */}
                                <div
                                    ref={scrollbarRef}
                                    className="border border-t-0 rounded-b-lg bg-muted/30 h-3 overflow-x-auto"
                                >
                                    <div style={{ width: '100%', minWidth: '100%' }} />
                                </div>
                            </div>

                            {/* Pagination Controls */}
                            {totalPages > 1 && (
                                <div className="flex items-center justify-between mt-6 pt-4 border-t">
                                    <div className="text-sm text-muted-foreground">
                                        Affichage {startIndex + 1} à {Math.min(endIndex, filteredLeads.length)} sur {filteredLeads.length} leads{showOnlyProblems && ` (${validatedLeads.length} au total)`}
                                    </div>
                                    <div className="flex items-center gap-2">
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            onClick={() => setCurrentPage(prev => Math.max(1, prev - 1))}
                                            disabled={currentPage === 1}
                                        >
                                            Précédent
                                        </Button>
                                        <div className="flex items-center gap-1">
                                            {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                                                <Button
                                                    key={page}
                                                    variant={currentPage === page ? 'default' : 'outline'}
                                                    size="sm"
                                                    onClick={() => setCurrentPage(page)}
                                                    className="w-8 h-8 p-0"
                                                >
                                                    {page}
                                                </Button>
                                            ))}
                                        </div>
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            onClick={() => setCurrentPage(prev => Math.min(totalPages, prev + 1))}
                                            disabled={currentPage === totalPages}
                                        >
                                            Suivant
                                        </Button>
                                    </div>
                                </div>
                            )}
                        </CardContent>
                    )
                    }
                </Card>
            )}

            {/* Action Buttons */}
            {validatedLeads.length > 0 && (
                <div className="flex gap-3">
                    <Button
                        onClick={handleClear}
                        variant="outline"
                        disabled={isSubmitting}
                    >
                        Annuler
                    </Button>
                    <Button
                        onClick={handleSubmit}
                        disabled={isSubmitting || validatedLeads.filter(l => l.action === 'import' || l.action === 'update').length === 0}
                        className="flex-1"
                    >
                        {isSubmitting ? (
                            <>
                                <div className="w-4 h-4 animate-spin rounded-full border-2 border-current border-t-transparent mr-2" />
                                Importation en cours...
                            </>
                        ) : (
                            <>
                                <CheckCircle2 className="w-4 h-4 mr-2" />
                                Importer {validatedLeads.filter(l => l.action === 'import').length} leads
                                {validatedLeads.filter(l => l.action === 'update').length > 0 &&
                                    ` (+ ${validatedLeads.filter(l => l.action === 'update').length} mises à jour)`
                                }
                            </>
                        )}
                    </Button>
                </div>
            )}

            {/* Info Box */}
            <Alert>
                <FileSpreadsheet className="h-4 w-4" />
                <AlertDescription>
                    <div className="font-medium mb-3">Format attendu - Export Facebook/Instagram Leads</div>
                    <div className="text-sm space-y-3">
                        {/* Required Columns */}
                        <div>
                            <span className="font-medium text-red-600">Colonnes obligatoires (Erreur si manquantes):</span>
                            <ul className="ml-4 mt-1.5 space-y-1">
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">id</code> - ID Facebook du lead (unique)</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">created_time</code> - Date de création (format: YYYY-MM-DD HH:MM:SS)</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">campaign_id</code> - ID de la campagne Facebook</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">campaign_name</code> - Nom de la campagne</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">is_organic</code> - true ou false</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">platform</code> - fb (Facebook) ou ig (Instagram)</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Nom et Prénom</code> - Nom complet du lead</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">E-mail</code> - Adresse email valide</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Numéro de téléphone</code> - Format E.164: +XXX... (doit commencer par +)</li>
                            </ul>
                        </div>

                        {/* Facebook Metadata */}
                        <div>
                            <span className="font-medium text-blue-600">Métadonnées Facebook (Importées pour analyse):</span>
                            <ul className="ml-4 mt-1.5 space-y-1">
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">ad_id</code>, <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">ad_name</code> - Publicité source</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">adset_id</code>, <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">adset_name</code> - Ensemble de publicités</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">form_id</code>, <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">form_name</code> - Formulaire utilisé</li>
                            </ul>
                        </div>

                        {/* Professional Data */}
                        <div>
                            <span className="font-medium text-emerald-600">Données professionnelles (Optionnelles):</span>
                            <ul className="ml-4 mt-1.5 space-y-1">
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Expérience globale en centres d'appels</code> - Valeurs: 0_-_6_mois, 7_mois_-_1_an, 1_-_3_ans, 3_-_5_ans, 5_-_7_ans, plus_que_7_ans</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Langue étrangère maîtrisée</code> - Valeurs: français, anglais, arabe, espagnol, italien, allemand, etc.</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Activité recherchée</code> - Ex: Prise de rendez-vous, Télévente, SAV, Support technique, etc.</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Expérience activité recherchée</code> - Même format que l'expérience globale</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Opération recherchée</code> - Ex: Télécom - B2B, Enérgie, Banque - Finance, etc.</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Expérience dans l'opération recherchée</code> - Même format que l'expérience globale</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Type de travail recherché</code> - Valeurs: télétravail, présentiel, hybride, peu_importe</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Temps de travail recherché</code> - Valeurs: temps_plein, temps_partiel, peu_importe</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Ville</code> - Ville du lead</li>
                            </ul>
                        </div>

                        {/* Skipped Columns */}
                        <div>
                            <span className="font-medium text-gray-600">Colonnes ignorées (non importées):</span>
                            <ul className="ml-4 mt-1.5 space-y-1">
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Numéro</code> - Numéro de ligne Excel (auto-généré)</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Blacklist Centres d'appels</code> - À clarifier ultérieurement</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Date insertion</code>, <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">Heure insertion</code> - Redondant avec created_time</li>
                            </ul>
                        </div>

                        {/* Authentication Notice */}
                        <div className="p-3 bg-blue-50 border border-blue-200 rounded-lg">
                            <span className="font-medium text-blue-700">🔐 Authentification automatique:</span>
                            <ul className="ml-4 mt-1 space-y-0.5 text-blue-700">
                                <li>• Un mot de passe temporaire sera créé automatiquement (= adresse email)</li>
                                <li>• Le lead pourra se connecter pour mettre à jour son profil</li>
                                <li>• Le changement de mot de passe sera obligatoire à la première connexion</li>
                                <li>• L'arabe (AR) sera ajouté automatiquement comme langue maternelle</li>
                            </ul>
                        </div>

                        {/* Duplicate Detection */}
                        <div className="p-3 bg-amber-50 border border-amber-200 rounded-lg">
                            <span className="font-medium text-amber-700">⚠️ Détection des doublons:</span>
                            <ul className="ml-4 mt-1 space-y-0.5 text-amber-700">
                                <li>• Les doublons sont détectés par <strong>adresse email uniquement</strong></li>
                                <li>• Les leads dupliqués seront marqués en bleu dans la prévisualisation</li>
                                <li>• Vous pouvez choisir: Ignorer, Importer quand même, ou Mettre à jour l'existant</li>
                            </ul>
                        </div>

                        {/* Error Handling */}
                        <div className="p-3 bg-red-50 border border-red-200 rounded-lg">
                            <span className="font-medium text-red-700">🔴 Gestion des erreurs:</span>
                            <ul className="ml-4 mt-1 space-y-0.5 text-red-700">
                                <li>• <strong>Erreurs</strong> (rouge) = Bloquent l'import - doivent être corrigées</li>
                                <li>• <strong>Avertissements</strong> (jaune) = Valeurs inconnues - peuvent être importées avec valeurs par défaut</li>
                                <li>• Vous devez explicitement confirmer chaque lead à importer via le dropdown "Action"</li>
                            </ul>
                        </div>

                        {/* Import Process */}
                        <div className="pt-3 border-t">
                            <span className="font-medium">📋 Processus d'import:</span>
                            <ol className="ml-4 mt-1.5 space-y-1 list-decimal">
                                <li>Téléchargez le fichier Excel → Analyse automatique</li>
                                <li>Vérifiez le résumé (Total, OK, Avertissements, Erreurs, Doublons)</li>
                                <li>Consultez la prévisualisation et les problèmes détectés et apportez les modifications nécessaires</li>
                                <li>Choisissez l'action pour chaque lead (Importer / Ignorer / Mettre à jour)</li>
                                <li>Cliquez sur "Importer" pour lancer l'importation des leads sélectionnés</li>
                            </ol>
                        </div>

                        {/* Tables Updated */}
                        <div className="pt-3 border-t">
                            <span className="font-medium text-purple-600">💾 Tables mises à jour pour chaque lead:</span>
                            <ul className="ml-4 mt-1.5 space-y-0.5 text-purple-700">
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">leads</code> - Informations personnelles + métadonnées Facebook</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">lead_profiles</code> - Expérience professionnelle + langues + secteurs</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">lead_settings</code> - Préférences de travail + activités recherchées</li>
                                <li>• <code className="text-xs bg-muted px-1.5 py-0.5 rounded font-mono">qualifications</code> - Statut de qualification (non qualifié par défaut)</li>
                            </ul>
                        </div>
                    </div>
                </AlertDescription>
            </Alert>
        </div>
    );
}
