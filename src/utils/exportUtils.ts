// Export utilities for PDF and Excel generation

export interface ExportData {
  title: string;
  date: string;
  data: Record<string, any>[];
  columns: { key: string; label: string }[];
}

// Generate CSV content from data
export const generateCSV = (exportData: ExportData): string => {
  const { columns, data, title, date } = exportData;
  
  // Header row
  const headers = columns.map(col => `"${col.label}"`).join(',');
  
  // Data rows
  const rows = data.map(row => 
    columns.map(col => {
      const value = row[col.key];
      // Escape quotes and wrap in quotes
      return `"${String(value ?? '').replace(/"/g, '""')}"`;
    }).join(',')
  );
  
  // Add metadata
  const metadata = [
    `"${title}"`,
    `"Date d'export: ${date}"`,
    '',
  ].join('\n');
  
  return `${metadata}\n${headers}\n${rows.join('\n')}`;
};

// Download CSV file
export const downloadCSV = (exportData: ExportData, filename: string): void => {
  const csv = generateCSV(exportData);
  const blob = new Blob(['\ufeff' + csv], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  const url = URL.createObjectURL(blob);
  
  link.setAttribute('href', url);
  link.setAttribute('download', `${filename}.csv`);
  link.style.visibility = 'hidden';
  
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  URL.revokeObjectURL(url);
};

// Generate and download Excel-compatible file (CSV with Excel formatting)
export const downloadExcel = (exportData: ExportData, filename: string): void => {
  // For true Excel, we'd use a library like xlsx
  // For now, we generate a CSV that Excel can open
  downloadCSV(exportData, filename);
};

// Print-friendly report generation
export const printReport = (content: HTMLElement | null, title: string): void => {
  if (!content) return;
  
  const printWindow = window.open('', '_blank');
  if (!printWindow) return;
  
  const styles = `
    <style>
      body {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        padding: 20px;
        color: #333;
      }
      h1 {
        color: #1a73e8;
        border-bottom: 2px solid #1a73e8;
        padding-bottom: 10px;
        margin-bottom: 20px;
      }
      table {
        width: 100%;
        border-collapse: collapse;
        margin: 20px 0;
      }
      th, td {
        border: 1px solid #ddd;
        padding: 12px;
        text-align: left;
      }
      th {
        background-color: #f5f5f5;
        font-weight: 600;
      }
      tr:nth-child(even) {
        background-color: #fafafa;
      }
      .header-info {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;
        color: #666;
        font-size: 14px;
      }
      @media print {
        body { -webkit-print-color-adjust: exact; }
      }
    </style>
  `;
  
  printWindow.document.write(`
    <!DOCTYPE html>
    <html>
      <head>
        <title>${title}</title>
        ${styles}
      </head>
      <body>
        <h1>${title}</h1>
        <div class="header-info">
          <span>CallCenterMatch.ai</span>
          <span>Date: ${new Date().toLocaleDateString('fr-FR')}</span>
        </div>
        ${content.innerHTML}
      </body>
    </html>
  `);
  
  printWindow.document.close();
  printWindow.focus();
  
  setTimeout(() => {
    printWindow.print();
    printWindow.close();
  }, 250);
};

// Performance data export helper
export const exportPerformanceData = (period: string = 'semaine'): ExportData => {
  const now = new Date();
  
  return {
    title: `Rapport de Performance - ${period}`,
    date: now.toLocaleDateString('fr-FR'),
    columns: [
      { key: 'metric', label: 'Métrique' },
      { key: 'value', label: 'Valeur' },
      { key: 'target', label: 'Objectif' },
      { key: 'percentage', label: 'Atteinte (%)' },
    ],
    data: [
      { metric: 'Leads traités', value: 285, target: 320, percentage: '89%' },
      { metric: 'Entretiens réalisés', value: 142, target: 200, percentage: '71%' },
      { metric: 'Candidats retenus', value: 89, target: 120, percentage: '74%' },
      { metric: 'Candidats intégrés', value: 56, target: 80, percentage: '70%' },
      { metric: 'Taux de conversion', value: '31%', target: '35%', percentage: '89%' },
      { metric: 'Score qualité', value: '92%', target: '90%', percentage: '102%' },
    ],
  };
};

// Team data export helper
export const exportTeamData = (): ExportData => {
  const now = new Date();
  
  return {
    title: 'Rapport Équipe',
    date: now.toLocaleDateString('fr-FR'),
    columns: [
      { key: 'rank', label: 'Classement' },
      { key: 'name', label: 'Nom' },
      { key: 'leads', label: 'Leads traités' },
      { key: 'conversion', label: 'Taux conversion' },
      { key: 'score', label: 'Score' },
    ],
    data: [
      { rank: 1, name: 'Ahmed Saidi', leads: 320, conversion: '35%', score: 98 },
      { rank: 2, name: 'Wael Wael', leads: 285, conversion: '31%', score: 92 },
      { rank: 3, name: 'Fatima Benali', leads: 260, conversion: '28%', score: 88 },
      { rank: 4, name: 'Karim Tazi', leads: 245, conversion: '26%', score: 82 },
      { rank: 5, name: 'Sara Idrissi', leads: 220, conversion: '24%', score: 78 },
    ],
  };
};

// Orders data export helper
export const exportOrdersData = (): ExportData => {
  const now = new Date();
  
  return {
    title: 'Rapport Commandes',
    date: now.toLocaleDateString('fr-FR'),
    columns: [
      { key: 'ref', label: 'Référence' },
      { key: 'date', label: 'Date' },
      { key: 'status', label: 'Statut' },
      { key: 'leadsLivres', label: 'Leads livrés' },
      { key: 'leadsTotal', label: 'Total commandé' },
      { key: 'tauxConversion', label: 'Taux conversion' },
    ],
    data: [
      { ref: 'CMD-2024-001', date: '15/01/2024', status: 'En cours', leadsLivres: 60, leadsTotal: 200, tauxConversion: '75%' },
      { ref: 'CMD-2024-002', date: '10/01/2024', status: 'Terminée', leadsLivres: 150, leadsTotal: 150, tauxConversion: '82%' },
      { ref: 'CMD-2024-003', date: '12/01/2024', status: 'En cours', leadsLivres: 45, leadsTotal: 100, tauxConversion: '68%' },
    ],
  };
};
