## Die neue web-basierte Plantafel

### Plantafel - Prozessbeschreibung

Die L-mobile Plantafel ist eine Anwendung zur Unterstützung der Personal- und Ressourceneinsatzplanung mit einer grafischen Oberfläche und ergänzenden Tools. Die Anwendung benötigt die L-mobile *Basis SMS* oder das aus einem ERP-System bereit gestellte Datenmodell und kann  mit dem *Modul Mobiler Techniker Client* optional erweitert werden.

### Öffnen der Plantafel

Zum Aufrufen der Plantafel:

- Öffnen Sie die **Profil-Info im Burgermenü**
- Führen Sie die **Plantafel Schaltfläche** aus

![Plantafel](img/WebScheduler/SMS_PT.png)

Alternativ öffnen Sie die Plantafel direkt nach dem Login.

![Plantafel](img/WebScheduler/SMS_PT1.png)

### Profile

Ein Profil beinhaltet die Konfigurationen der Plantafel wie z.B. Darstellungsoptionen, Planungsrahmen und die Ressourcen (Techniker und Werkzeuge).

#### Neues Profil erstellen

Um ein neues Profil zu erstellen gehen Sie wie folgt vor:

- Führen Sie die **Profilschaltfläche**  oben in ihrer **Navigationsleiste** aus
- Führen Sie die Schaltfläche **Neues Profil** aus
- Es öffnet sich ein Formular, vergeben Sie einen Namen für das Profil, optional können Sie es mit einem Haken zum **Standardprofil** machen

Wenn Sie nun erneut die **Profilschaltfläche** in der **Navigationsleiste** ausführen, steht dort der Name des neu erstellen Profils, hinter **"Aktuell:"**.

![Plantafel](img/WebScheduler/SMS_PT2.png)

#### Profil laden

Sie haben die Möglichkeit zwischen verschiedenen Profilen von Ihnen oder denen die von Mitarbeitern erstellt wurden zu wechseln. Gehen Sie folgendermaßen vor:

- Führen Sie die **Profilschaltfläche** oben in ihrer **Navigationsleiste** aus
- Führen Sie die Schaltfläche **Profil Laden** aus
- Wählen Sie eines ihrer Profile aus oder setzen Sie den **Haken** unter der Auswahl, um auch die Profile von **anderen Nutzern** auswählen zu können

Wenn Sie nun erneut die **Profilschaltfläche** in der **Navigationsleiste** ausführen, steht dort der Name des neu erstellen Profils, hinter "Aktuell:"

![Plantafel](img/WebScheduler/SMS_PT3.png)

#### Ressourcen zum Profil hinzufügen

Ressourcen (Techniker und Werkzeuge), können Sie einem Profil zuordnen, dafür müssen Sie das Profil geladen haben, ist dies bereits passiert, gehen sie folgendermaßen vor:

- Führen Sie die **Profilschaltfläche** oben in ihrer **Navigationsleiste** aus
- Führen Sie die Schaltfläche **Ressourcen zu Profil hinzufügen**
- Fügen Sie **Techniker, Werkzeuge oder Fahrzeuge** als Ressource hinzu, dafür wählen Sie, die Resourcen einzeln aus
- Entfernen Sie versehentlich ausgewählte Techniker mit einer erneuten Auswahl
- **Speichern** Sie

![Plantafel](img/WebScheduler/SMS_PT4.png)

![Plantafel](img/WebScheduler/SMS_PT5.png)

***Wichtig*: Sie können auch Techniker hinzufügen ohne für diese eine Lizenz zu aktivieren, z.B. für externe Mitarbeiter.**

#### Profil bearbeiten

Konfigurieren Sie das Profil nach Ihren Vorstellungen:

- Führen Sie die **Profilschaltfläche** oben in ihrer **Navigationsleiste** aus
- Führen Sie die Schaltfläche **Profil bearbeiten** aus
- Es öffnet sich ein Formular zum bearbeiten des Profils

![Plantafel](img/WebScheduler/SMS_PT6.png)

#### Cache konfigurieren

In den Feldern **Untere Grenze** und **Obere Grenze** können Sie einstellen in welchem Zeitraum Daten (Aufträge) geladen werden.
Wählen Sie einen größeren Bereich dauert das Laden der Daten entsprechend länger und bei einer Reduzierung des Zeitraums kann die Ladezeit verringert werden.

![Plantafel](img/WebScheduler/SMS_PT7.png)

#### Zeilenhöhe anpassen

Sie können unter **Profil bearbeiten** auch die **Zeilenhöhe der Ressourcenliste und des Planungsvorrats anpassen**. Je mehr Elemente in einer Liste sind desto schneller verliert man die Übersicht, daher bietet es sich bei steigender Anzahl an Elementen an, die Zeilenhöhe zu reduzieren.

| Zeilenhöhe        | Zeilen    | 
|--------------|-----------|
| 32 Minimum für Label | Eine Zeile      | 
| 48    | Zwei Zeilen  |
|72 | Drei Zeilen | 

#### Planungsvorrat konfigurieren

In den Profileinstellungen können Sie ebenfalls die Ordnung der Elemente im Planungsvorrat sortieren, z.B. Status, Postleitzahl, Kunde etc.

![Plantafel](img/WebScheduler/SMS_PT8.png)

#### Tooltips anpassen

Sie können die Informationen welche im Tooltip erscheinen unter **Profil bearbeiten** anpassen.
Der Tooltip sind jene Informationen die erscheinen wenn Sie mit der Maus über der Schaltfläche eines Auftrags oder eines Technikers drüber schweben ohne mit der Schaltfläche, durch einen Mausklick zu interagieren.

![Plantafel](img/WebScheduler/SMS_PT9.png)

![Plantafel](img/WebScheduler/SMS_PT10.png)

### Alternative Ansichten

Sie haben zwei weitere Ansichten zur Verfügung:

- Ein Kapazitätsgebirge welchen Ihnen die Auslastung der "Ressourcen (Techniker, Werkzeuge und Fahrzeuge)" anzeigt
- Eine Karte welche Ihnen Ihre Serviceobjekte und Aufträge anzeigt

Das Ein- und Ausblenden der Ansichten ist über die Optionen Schaltfläche in der **Navigationsleiste** möglich.

![Plantafel](img/WebScheduler/SMS_PT11.png)

#### Plantafel Darstellung / Ansicht verändern / verschieben

Sie können die einzelnen Anwendungselemente in der Plantafel nach Ihren eigenen Bedürfnissen verschieben, durch die grauen Balken zwischen den Anwendungselementen, können Sie bestimmen wie viel Platz jedes einzelne einnimmt.

![Plantafel](img/WebScheduler/SMS_PT12.png)

#### Karte in einem zusätzlichen Fenster darstellen

Die Karte können Sie nach der Aktivierung in einem zusätzlichen Fenster darstellen, indem Sie das **blaue Symbol**, in der rechten **oberen** **Ecke** der Karte betätigen.

![Plantafel](img/WebScheduler/SMS_PT13.png)

### Aufträge einplanen

Möchten Sie Aufträge einplanen, müssen Sie vorher ein Profil erstellt/geladen,  Techniker hinzugefügt und Aufträge zum einplanen vorbereitet haben.

#### Drag and Drop

Haben Sie bereits alles vorbereitet können Sie über die "Drag & Drop" Funktion Aufträge aus der Pipeline (die Liste rechts in der Plantafel) auf einen Mitarbeiter ziehen.

![Plantafel](img/WebScheduler/SMS_PT14.png)

#### Automatisches Speichern

Über die **Optionen** Schaltfläche in der **Navigationsleiste** können Sie das automatisches Speichern an und aus schalten.

![Plantafel](img/WebScheduler/SMS_PT15.png)

Wenn Sie mehrere Einsätze Planen müssen, ist es zu empfehlen die Funktion auszuschalten.

**Ist automatisches Speichern deaktiviert, müssen Sie die Datensynchronisation manuell ausführen, sonst ist ihre Einteilung in der Plantafel** nach einem erneuten Laden der Plantafel nicht mit dem Server synchronisiert und  damit **nicht gespeichert!**

![Manuelle Synchronisation](img/WebScheduler/SMS_PT16.png)

#### Mehre Personen, Werkzeuge oder Fahrzeuge zu einem Auftrag hinzufügen

Möchten Sie mehrere Personen, Werkzeuge oder Fahrzeuge zu einem Auftrag hinzufügen, dann gehen Sie auf den Auftrag der bereits in die Plantafel gezogen wurde, betätigen Sie Schaltfläche mit einem Rechtsklick, gehen Sie auf **Einsatz bearbeiten** und fügen Sie einen weitere Ressource aus der Auswahlliste hinzu.

![Plantafel](img/WebScheduler/SMS_PT17.png)

![Plantafel](img/WebScheduler/SMS_PT18.png)


#### Auftrag Fixieren

Gehen Sie hierfür auf den Auftrag der bereits in die Plantafel gezogen wurde, betätigen Sie Schaltfläche mit einem Rechtsklick, setzen Sie einen Haken bei "Fixiert" oder gehen Sie auf Auftrag bearbeiten und fixieren Sie den Auftrag mit einem Haken bei "Fixiert". Der Auftrag kann nun nicht mehr verschoben werden.

![Plantafel](img/WebScheduler/SMS_PT19.png)

#### Auftrag Bestätigen

Gehen Sie hierfür auf den Auftrag der bereits in die Plantafel gezogen wurde, betätigen Sie Schaltfläche mit einem **Rechtsklick**, führen Sie die **Bestätigen Schaltfläche** aus oder gehen Sie auf Auftrag bearbeiten und machen Sie einen Haken bei Auftrag Freigegeben. Der Auftrag kann nun nicht mehr verschoben werden und wird für den Client des Technikers zur Synchronisation freigegeben.

![Plantafel](img/WebScheduler/SMS_PT20.png)

### Informationen Filtern

In der Plantafel haben Sie die Möglichkeit die Informationen zu filtern.

- Mit der Legende, können Sie nach den jeweiligen Attributen filtern z.B. Abgeschlossen, Eingeplant, Gelesen usw.
- Mit dem Ressourcenfilter können Sie die Techniker und Werkzeuge filtern
- Mit dem Ereignisfilter, können Sie nach Elementen die Bereits in die Plantafel geschoben wurden suchen, eine Einsatznummer oder z.B. nach Abwesenheiten wie Urlaub
- Mit dem Planungsvorratfilter können Sie nach Elementen im Planungsvorrat Filtern z.B. nach eingestellten Gruppierungen wie Freigegeben und Eingeplant, Postleitzahlen, Kunden etc. oder z.B. nach der Auftragsnummer

 ![Plantafel](img/WebScheduler/SMS_PT21.png)
  
  ----
## Plantafel als Client

### Plantafel- Prozessbeschreibung

Die L-mobile Plantafel ist eine Anwendung zur Unterstützung der Personal- und Ressourceneinsatzplanung mit einer grafischen Oberfläche und ergänzenden Tools. Die Anwendung benötigt die L-mobile Basis SMS oder das aus einem ERP-System bereit gestellte Datenmodell und kann  mit dem Modul Mobiler Techniker Client optional erweitert werden.

### Plantafel- Installation

**Plantafel - Installation und Benutzer**

Die Installation der Plantafel kann über zwei verschiedene Wege erfolgen:

1. Im L-mobile im Burgermenü nachdem Sie ihren Benutzerprofil ausgeklappt haben.

![Plantafel](img/Scheduler/SMS_PT.png)

Durch das Anklicken des Menüpunktes "Plantafel" wird der Download auf den Desktop gestartet. Nach der Installation steht auf dem Desktop über das Symbol "Plantafel" ein direkter Zugriff zur Verfügung. Alternativ kann nach der Erstinstallation auch über den Menüpunkt "Plantafel" im SMS die Plantafel-Oberfläche gestartet werden.

2. Bei einer direkten Kopplung mit einem ERP-System wird die Installation durch den entsprechend berechtigten Administrator auf dem Desktop vorgenommen.

Um auf die Plantafel zugreifen zu können, benötigen Benutzer die Rolle Administrator oder Einsatzplaner. Diese werden den Benutzern vom Administrator in der L-mobile SMS Benutzerverwaltung zugeteilt.

![Plantafel](img/Scheduler/SMS_PT2.png)

Viele Elemente der Plantafel können zusätzlich frei konfiguriert werden. Darüber hinaus werden bei vielen Bereichen weitere Informationen auf Anforderung angezeigt.

### Plantafel- Übersicht

#### Plantafel - Übersicht der Elemente

Die wichtigsten Standardelemente der L-mobile Plantafel sind wie folgt:

- Menüzeile
- Personen und Ressourcen
- Planungsansicht
- Planungsvorrat und Nebenzeiten
- Kapazitätsdiagramm
- Kartendarstellung
- Einsatzliste



![Standardelemente Plantafel](img/Scheduler/gesamte_PT.png "Standardelemente Plantafel")

Alle diese Elemente, außer der Planungsansicht, können ein- und ausgeblendet werden. Sie können auch beliebig innerhalb der Anwendungsansicht platziert werden. Beim Verschieben der Elemente mit der Maus, linke Taste gedrückt halten, es wird eine Platzierungshilfe angezeigt, in der der Benutzer den gewünschten Bereich (oben, unten, rechts, links, Mitte) wählen kann:

![Plantafel](img/Scheduler/PT_Ansicht.png)

Nach dem Platzieren, können die Ränder der Elementenflächen mit dem Mauszeige (Pfeile) weiter in Breite und Höhe angepasst werden. 

#### Planungsansicht und Planung auf der Plantafel

**Planungsansicht**

Im Bereich "Planung" werden die Ressourcen den Auftragseinsätzen im Kalender zugeordnet.

![Plantafel](img/Scheduler/PT_Uebersicht.png "Plantafel")

**Ressourcen**

Die Plantafel ermöglicht es, den Überblick über alle zu planenden Ressourcen im gewählten Zeitraum zu behalten - sowohl Techniker, als auch z.B. Subunternehmer, Maschinen und viel mehr. Diese Ressourcen können sowohl nach Standorten als auch nach Teams (Funktionen) gruppiert werden. Die Anzeige kann zwischen Vor- und Nachname umgeschaltet und sortiert werden. Dies stellt der berechtigter Benutzer unter Profil/ Einstellungen Planung und Auftragsvorrat (Profileinstellung) / Personen um.

![Plantafel](img/Scheduler/PT_Profil.png "Plantafel: Profileinstellung")

Unter Profil/ Ressourcen zu Profil hinzufügen können fehlende Ressourcen hinzugefügt und unnötige aus dem Profil gelöscht werden:

![Plantafel](img/Scheduler/PT_Ressourcen.png "Plantafel: Ressourcen")

Um in der Liste der angezeigten Ressourcen zu filtern, werden Suchworte im Feld "Suchen" eingetragen und mit der Eingabetaste bestätigt.

![Plantafel](img/Scheduler/PT_Suchen.png "Plantafel: Suchen")

Zum Löschen der Auswahl, Klicken Sie Schaltfläche *Löschen* an.

![Plantafel](img/Scheduler/PT_Loeschen.PNG "Schaltfläche: Löschen")  

#### Planungsvorrat {#scheduler.pipeline}

Hier werden die zu  planenden Aufträge dargestellt. Dabei kann die Liste nach unterschiedlichen Kriterien wie z.B. Auftragsart, Maschinentyp, Maschinenstatus o.ä. gruppiert werden. Neue Elemente können optional erweitert werden. Die Gruppierungen werden mit Plus- und Minuszeichen auf- bzw. zugeklappt. Ergänzt wird der Planungsvorrat durch die Darstellung von nicht auftragsbezogenen Nebenzeiten und Abwsesenheiten.

**Planungsvorrat-Suche**

Um in der Liste der angezeigten Aufträge zu filtern, werden Suchworte im Feld "Suchen" eingetragen und mit der Eingabetaste bestätigt.

![Plantafel](img/Scheduler/PT_PV_Suche.png "Plantafel: Suchen")

Zum Löschen der Auswahl, Klicken Sie Schaltfläche *Löschen* an.

![Plantafel](img/Scheduler/PT_Loeschen.png "Schaltfläche: Löschen")  

**Abwesenheiten / Nebenzeiten**

Die Zeitarten, die nicht auftragsbezogen sind, werden unter "Abwesenheit" ganz oben im Planungsvorrat angezeigt. Die Liste dieser Zeitarten kann vom berechtigten Benutzer (Administrator) in der L-mobile SMS unter Verwaltung der entsprechenden Zuordnungstabelle angepasst werden.



![Nebenzeiten](img/Scheduler/Nebenzeiten.png "Nebenzeiten")

**Planungsvorrat-Filter**

Die Standardfilter vom Arbeitsvorrat sind wie folgt:

- Priorität
- Standort
- Auftragstyp
- Installationsart
- Installationsgruppe
- Installationsstatus

Diese Filter stellt der berechtigte Benutzer unter Profil/ Einstellungen Planung und Auftragsvorrat (Profileinstellung) / Planungsvorrat-Filter um. Dafür werden die Filtergruppe mit dem Pluszeichen aufgekappt und selektiert.

![Plantafel](img/Scheduler/PT_PV_Filter.png)

**Planungsvorrat-Gruppen**

Die Standardgruppen vom Arbeitsvorrat stellt der berechtigter Benutzer unter Profil/ Einstellungen Planung und Auftragsvorrat (Profileinstellung) / Planungsvorrat-Gruppen um. Dafür werden die Gruppen auf der linken Seite ausgewählt und mit den Pfeiltasten nach links verschoben, wo mit den Tasten, die nach oben und unten zeigen, die Reihenfolge der Gruppierungen festgelegt wird.

![Plantafel](img/Scheduler/PT_PV_Gruppen.png)

**Aufträge verteilen / einplanen**

Die Aufträge (und Abwesenheiten) werden aus dem Planungsvorrat mit der rechten Maustaste auf den entsprechenden Techniker und Arbeitstag gezogen. Dabei ist es möglich nur einen oder mehrere Serviceeinsätze bei einem oder unterschiedlichen Techniker zu erstellen, d.h. der Auftrag darf mehrmals verteilt werden.

**Einsätze bearbeiten**

Es gibt zwei Möglichkeiten für die Feinplanung:

1. Direkt auf der Planungsansicht den Einsatz mit der Maus verschieben und dessen Größe anpassen, oder

2. im Menü/ Eigenschaften der jeweiligen Einsätze. Mit einem Doppelklick oder mit der rechten Maustaste/Menü/ Eigenschaften öffnet sich folgendes Eingabefenster:

![Plantafel](img/Scheduler/PT_Einsatz.png)

Hier kann der Benutzer das Datum, Uhrzeiten bzw. Dauer des Einsatzes verändern.

Unter dem Punkt Status wird der Einsatz an den Techniker "freigegeben" bzw. die Freigabe widerrufen, als auch als feststehender Termin "fixiert". 

Unter den Informationen werden Details zum Einsatz für die Einsatzplaner (interne Informationen) oder den Techniker (Informationen für den Techniker) eingetragen.  Falls eine automatische Übertragung der Technikerinformationen auf alle Arbeitseinsätze dieses Auftrags  erwünscht ist, muss das Häkchen im Feld ![Plantafel](img/Scheduler/PT_Texte.png) 
selektiert werden.

Unter der Änderungshistorie werden Änderungen in der Planung des gewählten Einsatzes angezeigt. Hier kann nachverfolgt werden wann welcher Einsatzplaner Änderungen vorgenommen hat und welcher Techniker jeweils auf welches Datum geplant wurde.:

![Plantafel](img/Scheduler/PT_Historie.png)

**Planbalken/ Etiketten einstellen**

Eine konfigurierbare Beschriftung der Planbalken erfolgt mit Hilfe einer individuellen Konfiguration unter Menü/ Extras/ Einstellungen/ Farben & Tooltips/ Serviceeinsatz Einstellungen/ Beschriftung:

![Plantafel](img/Scheduler/PT_Balken.png)

**Fähigkeiten**

Wurden bei der Auftragserstellung bestimmte Fähigkeiten (Skills) als Voraussetzung zur Erledigung festgelegt,  erscheint beim Anklicken des Auftrags bei den geeigneten Technikern in der Ressourcenübersicht rechts vom Namen ein Werkzeug-Symbol.

![Plantafel](img/Scheduler/Werkzeugsymbol.png)

Wurde in der Vergangenheit ein Techniker bereits bei dem selben Gerät zur Auftragsbearbeitung eingesetzt oder ist bereits ein Techniker diesem Auftrag zugeordnet, erscheint beim Anklicken des Auftrags bei den entsprechenden Technikern das "Technikersymbol"   ![Plantafel](img/TEchnikersymbol.png). 
Beim Anklicken eines Technikers mit dem Mauszeiger werden weitere Informationen über den Techniker und seine Fähigkeiten eingeblendet.

![Plantafel](img/Scheduler/PT_Faehig.png "Fähigkeiten")

Die Liste und die Zuordnung der Fähigkeiten zu den Mitarbeitern erfolgt in der L-mobile SMS / Benutzerverwaltung vom berechtigten Benutzer (Administrator).

#### Kapazitätsdiagramm

Mit dem Kapazitätsgebirge können Sie die Auslastung einzelner Mitarbeiter, Gruppen oder ganzer Standorte im Blick behalten. Die Planung verändert dabei die Auslastungssituation in Echtzeit. Alle Nebenzeiten oder Abwesenheiten werden entsprechend Ihrer Kapazitätseinstellung mindernd oder neutral gewertet. 

![Plantafel](img/Scheduler/PT_Kapa.png)

Zum Einblenden des Kapazitätsdiagram, wird unter Menü/ Profil/ Kapazitätsgebirge angeklickt:

![Plantafel](img/Scheduler/PT_Kapa2.png)

Zum Ausblenden des Kapazitätsdiagrams, wird die Schaltfläche ![Plantafel](img/Scheduler/PT_Close.png)angeklickt.

#### Einsatzliste

Die Einsatzliste zeigt Arbeitseinsätze als eine Liste, die im Standard eine Filterfunktion für eine Suche und Analyse beinhaltet.

![Plantafel](img/Scheduler/Einsatzliste.png)

Zum Einblenden der Einsatzliste, wird unter Menü / Ansicht / Einsatzliste angeklickt:

![Plantafel](img/Scheduler/Einsatzliste2.png)

Taste "Zeige Filter" ![Plantafel](img/Scheduler/PT_ZeigeFilter.png)  blendet die vorangestellte Auswahl an Filterkriterien die über die Tasten "Suche" und "Zurücksetzen" gesteuert werden.

![Plantafel](img/Scheduler/Einsatzliste3.png)

Mit dem Mausklick auf dem Spaltennamen ändert der Benutzer die Reihenfolge der Zeilen ab- bzw. aufsteigend. Die Farbe in der Spalte "Status" entspricht dem Status des Einsatzes (Siehe oben Plantafel/ Farben für den Zustand der Planung)

![Plantafel](img/Scheduler/Einsatzliste4.png)

{% include 'scheduler-map.md' %}

### Plantafel- Einstellungen

Unter dem Menü Datei kann der Benutzer folgende Funktionen finden:

- Speichern (auch über Menüschaltfläche ![Plantafel](img/Scheduler/PT_Speichern.png))

- Seiteneinstellungen: für den Druck der Anzeige (die Anzeige der Anwendung ist auch mit der Maus an den Ränder der Gesamtfläche verschiebbar)

- Drucken: sichtbarer Bereich wird, wie unter Seiteneinstellungen definiert, gedruckt

- Beenden (auch über Windows-Schaltfläche ![Plantafel](img/Scheduler/PT_Beenden.png) möglich)

![Plantafel](img/Scheduler/Menue_Datei.PNG)

Weitere Schaltflächen:

-  Menüpunkt "Aktualisieren" (auch über Menüschaltfläche  ![Plantafel](img/Scheduler/PT_Aktualisieren.png))
-  Schaltfläche "Detailstufe umschalten"   ![Plantafel](img/Scheduler/PT_Detail.png) zum Ein- und ausklappen der Ressourcen

**Planungshorizont einstellen**

- Menü Schaltfläche "Zeitraum" ![Plantafel](img/Scheduler/PT_Zeitraum.png) hat eine Standardvorschlagsliste wie "diese Woche" oder "nächster Monat" zur Auswahl zum Anklicken, oder
- Menü Schaltfläche "Gehe zum Datum" öffnet den Kalender, um einen genauen Tag auszuwählen

![Plantafel](img/Scheduler/PT_Datum.png) 

- Menü Schaltflächen mit Zoomzeichen ![Plantafel](img/Scheduler/PT_Zoom.png) vergrößern oder verkleinern die Kalenderansicht im Planungsbereich entsprechend der Zeichen
- Unter dem Menü/ Profil/ Profileinstellungen/ Planungseinstellungen können die obere und untere Grenzen vom Planungshorizont zwischen zwei Wochen und einem Jahr (im Standard) ausgewählt werden:

![Plantafel](img/Scheduler/PT_Planung.png) 

#### Benutzerunterstützung auf der Plantafel - Tooltips und Farben

**Tooltips** sind Informationen, die beim Zeigen mit der Maus in einem separaten Fenster im Vordergrund eingeblendet werden.

Auf der L-mobile Plantafel gibt es zwei Arten der Tooltips:

(1) Tooltips mit Auftragsinformationen im Planungsbereich

![Plantafel](img/Scheduler/PT_Tooltip.png "Auftragsinformationen/Tooltips")

(2) Tooltips mit Auftragsinformationen im Planungsvorrat

![Plantafel](img/Scheduler/PT_Tooltip2.png "Auftragsinformationen/ Tooltips")

**Farben für den Zustand der Planung**

![Plantafel](img/Scheduler/Zustand_Farben.PNG)

Unterschiedliche Zustände in der Planung werden durch unterschiedliche Farben der Planbalken deutlich:

- Rot – Eingeplant, ungespeichert - Planung simulieren
- Gelb – Eingeplant, gespeichert, für andere Disponenten/ Planer sichtbar
- Blau – Fixtermin, kann nicht mehr verändert werden
- Grün – Freigabe, Techniker bekommt diesen bei Nutzung des Mobilen Techniker Clients automatisch aufs Endgerät übermittelt und kann am Auftrag arbeiten.

**Plantafel – Farben und Tooltips einstellen**

Die Farben und Tooltips können unter Menü/ Extras/ Einstellungen angepasst werden:

![Plantafel](img/Scheduler/PT_Einstellungen.png "Farben und Tooltips")

#### Plantafel – Profile

**Plantafel - Definition der Profile**
Profile auf der Plantafel sind benutzerdefinierte Darstellungen der Planung, Ressourcen und Aufträge. Es können Standardprofile für die gesamte Organisation, Benutzergruppen oder einzelne Benutzer erstellt werden.
Über Profil/ Lade... werden verschiedene gespeicherte Ansichten der Plantafel aufgerufen:

![Plantafel](img/Scheduler/PT_Profil1.png)

Über Profil/ Speichern Unter... werden neue Ansichte der Plantafel erstellt:

![Plantafel](img/Scheduler/PT_Profil2.png)

Dabei können berechtichte Benutzer auch neue Standardprofile beim Setzen des ensprechenden Häkchens erstellen:

![Plantafel](img/Scheduler/PT_Profil3.png)

Über Profil / Löschen wird das aktuell geladen Profil gelöscht. Anschliessend wird das Standard-Profil des Benutzers geladen. Das Standard-Profil kann nicht gelöscht werden.

![Plantafel](img/Scheduler/PT_Profil4.png)

Über Profil/ Ressourcen zu Profil hinzufügen werden neue Ressourcen (z.B. Techniker) auf der Planungsansicht eingeblendet:

![Plantafel](img/Scheduler/PT_Profil5.png)

Dabei markieren Sie die Zeile der Ressource in der Tabelle und klicken anschliessend ![Plantafel](img/Scheduler/PT_Profil6.png) an.

### Teamplanung

Mit der Teamplanung ist es möglich mehrere Techniker zusammen auf einen Einsatz zu planen. Dazu werden mithilfe von Benutzergruppen Teams gebildet. Eine Hauptressource, welche pro Einsatz geändert werden kann, erhält den Einsatz stellvertretend für das Team zur weiteren Durchführung.
Für die Teamplanung muss das entsprechende Plugin für Plantafel und SMS aktiviert werden.

#### Teams und Hauptressourcen

Teams in der Plantafel entsprechen Benutzergruppen. Diese lassen sich im Backend in Verwaltung - Benutzergruppen erstellen und den Benutzern zuordnen.

![Plantafel](img/Scheduler/team_team.png)

Pro Benutzergruppe sollte eine Hauptressource definiert werden, welche den Teameinsatz stellvertretend durchführt.

![Plantafel](img/Scheduler/team_mr.png)

#### Planung
Einsätze können wie bisher per Drag & Drop geplant werden mit dem Unterschied, dass man direkt auf ein Team planen kann.
![Plantafel](img/Scheduler/team_planning.png)
Der daraus resultierende Einsatz wird über alle Mitglieder des Teams dargstellt und kann nur auf Team-Ebene verschoben werden.
![Plantafel](img/Scheduler/team_planned.png)
Ein Teameinsatz kann auch auf ein anderes Team umgeplant werden oder zu einem Einzeleinsatz auf einen Techniker gelegt werden.

Zusätzlich zur vordefinierten Hauptressource, welche standardmäßig bei der Planung verwendet wird kann die Hauptressource auch pro Teameinsatz geändert werden. Dazu kann man in den Einsatzeigenschaften entsprechend ein anderes Teammitglied auswählen. Weiterhin kann man in diesem Dialog weitere Techniker (auch team fremde) zum Einsatz hinzufügen oder entfernen. Team-fremde Techniker werden besonders hervorgehoben.
![Plantafel](img/Scheduler/team_setmr.png)


## Administration
Die Plantafel kann über das SMS administriert werden. Dazu findet man unter dem Menüpunkt _Verwaltung_ den Eintrag _Plantafel_ . Diese Funktionen sind der Projektentwicklung der L-mobile vorbehalten.

![Plantafel Administration](img/Scheduler/PT_Admin.png)

Im rechten Bereich können neue Dateien hochgeladen und Installationspakete erstellt werden. Im linken Bereich sind vorhandene Einstellungen und Installationspakete sichtbar.

### Programmdateien
Für neue Installationspakete werden Programmdateien benötigt. Diese können entweder über ein Formular hochgeladen oder auf dem Server im Unterordner `App_Data\Scheduler` der entsprechenden Webseite abgelegt werden. Als Dateiformat wird ein ZIP-Archiv erwartet.
Vorhandene Programmdateien können in der Verwaltung entfernt oder zu Installationspaketen umgewandelt werden.

### Einstellungen
Für die Plantafel kann ein Icon sowie ein Konfigurationspaket festgelegt werden. Beides geschieht per Formular-Upload. Als Icon wird ein Bild im ICO-Format erwartet. Die Konfiguration kann entweder einzeln als XML-Datei (z.B. `app.debug.config`) oder als ZIP-Archiv hochgeladen werden. Aus dem ZIP-Archiv werden die entsprechenden Konfigurationsdateien herausgefiltert und in ihrer Ordnerstruktur behandelt. Für ZIP-Archive muss den Konfigurationstyp (_Debug_ oder _Release_) ausgewählt werden. Anhand des Typs wird entschieden welche Konfigurationsdateien für die Transformation benutzt werden.

Die Ordnerstruktur kann z.B. so aussehen:

- `customer.lmobile`
  - `app.debug.config`
  - `app.release.config`
  - `app.debug.ico`
  - `Plugins`
    - `Einsatzplanung.Service`
      - `app.debug.config`
      - `app.release.config`

Weiterhin wird die Einstellung `Setup/Name` für den Namen des Pakets verwendet.

### Installationspaket
Aus den Programmdateien kann ein Installationspaket erstellt werden. Dies ist dann erlaubt, wenn die Version der Programmdateien gleich oder größer der aktuell freigegebenen Plantafel ist.
Für das Installationspaket wird die aktuelle Konfiguration und das Icon benutzt. Verschiedene Installationspakete können also unterschiedliche Konfigurationen und Icons haben.

Installationspakete müssen immer eine eindeutige Version haben. Wurde z.B. für Version _5.0.0.470_ bereits ein Installationspaket erstellt und man möchte nur die Konfiguration ändern, so läd man eine angepasst Konfiguration hoch und erstellt anschließend ein neues Installationspaket. Dabei wird eine Warnung angezeigt:
> Es gibt bereits ein Installationspaket basierend auf Version 5.0.0.470. Es wird automatisch eine höhere Versionsnummer erzeugt? Forfahren?

Bestätigt man diese, wird automatisch eine neue eindeutige Version festgelegt. Die Basis-Version wird in der Ansicht hevorgehoben.

![Version mit Basisversion](img/Scheduler/PT_Admin_Version.png)

Zu einem Installationspaket kann man das Manifest herunterladen, was zur Installation dient. Weiterhin findet man die benutzte Konfiguration und das Icon.

Mit dem Manifest kann ein Installationspaket getestet werden, ohne dass es für alle Plantafel-Benutzer freigegeben werden muss. 

Die Freigabe eines Installationspakets kann nur für Pakete mit höherer Version als das aktuell freigegeben Paket (sofern vorhanden) erfolgen. Freigegebene Installationspakete sind für Plantafel-Benutzer erreichbar - z.B. nach dem Login, oder im Menüpunkt _Service_ - und werden beim nächsten Start der Plantafel entsprechend aktualisiert.

