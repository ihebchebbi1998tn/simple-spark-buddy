# Objektdetails - Firma
Zusätzlich zu den im Kapitel Objektdetails aufgeführten Funktionen stehen für Firmen noch weitere Funktionen zur Verfügung die hier im weiteren vorgestellt werden sollen.

## Kopfdarstellung - Zugehörigkeit
Wurde bei der Anlage der Firma ein *Mutterunternehmen* angegeben findet sich in der Kopfdarstellung der *Tochter* ein Verweis auf die Mutter. Mit Hilfe des Verweis kann die Detaildarstellung der Mutter aufgerufen werden.

![Zugehörigkeit zu einer Mutter](img/company/header_parent.png "Zugehörigkeit zu einer Mutter")

## Beziehungen {#relationships}

Beziehungen zwischen Objekten können genutzt werden um zusätzlich zu einer streng hierarchischen Struktur ein Netzwerk von Objekten zu bilden. Dabei kann innerhalb der Beziehung unterschieden werden:

- Beziehungen die in beide Richtungen gleich wirken z.B. Wettbewerber
- Beziehungen die unterschiedlich in beide Richtungen wirken z.B. Kunde / Lieferant
- Beziehungen die nur in eine Richtung wirken

![Beziehungen zwischen Objekten](img/relationships.png "Beziehungen zwischen Objekten")

---

## Anwendungsbereich - Beziehungen
Aktive Beziehungen werden nach Ihrem Beziehungstyp gruppiert und in einer Liste dargestellt. Vorhandene Beziehungen können bearbeitet oder entfernt werden. 

![Listendarstellung vorhandener Beziehungen einer Firma](img/company/relationship_list.png "Listendarstellung vorhandener Beziehungen einer Firma")

Neue Beziehungen werden mit Hilfe der Kontextfunktion *Geschäftsbeziehung hinzufügen* eingerichet.

![Kontextfunktion - Geschäftsbeziehung anlegen](img/company/relationship_action_create.png "Kontextfunktion - Geschäftsbeziehung anlegen")

Sie haben verschiedene Formen der Beziehung zur Auswahl: 

![Kontextfunktion - Geschäftsbeziehung anlegen](img/company/relationship_action_create2.png "Kontextfunktion -Geschäftsbeziehung anlegen")

---

![Kontextfunktion - Artikelbeziehung anlegen](img/company/relationship_action_create3.png "Kontextfunktion - Artikelbeziehung anlegen")

---

![Kontextfunktion - Projektbeziehung anlegen](img/company/relationship_action_create4.png "Kontextfunktion - Projektbeziehung anlegen")

---

![Kontextfunktion - Unternehmen-Person-beziehung anlegen](img/company/relationship_action_create5.png "Kontextfunktion - Unternehmen-Person-beziehung anlegen")

---

![Kontextfunktion - Potenzialbeziehung anlegen](img/company/relationship_action_create6.png "Kontextfunktion - Potenzialbeziehung anlegen")

---

![Kontextfunktion - Geräteaktenbeziehung anlegen](img/company/relationship_action_create7.png "Kontextfunktion - Geräteaktenbeziehung anlegen")

---

Im angebotenen Formular haben Sie die Möglichkeit Informationen zur Gegenseite, Art der Beziehung und weiterführende Informationen zu erfassen.

![Formulardarstellung für Beziehungen](img/company/relationship_form.png "Geschäftsbeziehung Formulardarstellung")

----

Die angebotenen Beziehungsarten werden in einer Zuordnungstabelle pro Objekt gepflegt und können auf Ihre Bedürfnisse angepasst werden. Z.B. **Geschäftsbeziehungstypen**:

![Zuordnungstabelle für Beziehungen](img/company/relationship_types.png "Geschäftsbeziehung Zuordnungstabelle")

----



## Kontext - Kontaktinformationen
Zu einer Firma kann eine Liste von Adressen gespeichert werden. Die Adressliste bildet die Grundlage um z.B. diesen Adressen einzelne Personen zuzuweisen.

![Liste von Adressen der Firma](img/company/address_list.png "Liste von Adressen der Firma")

Neue Adressen können mit Hilfe der Funktion *Hinzufügen* zur geöffneten Firma erfasst werden.


Jeder Adresse kann eine Reihe von Kommunikationsdaten zugeordnet werden um z.B. verschiedene Kontaktmöglichkeiten an den unterschiedlichen Adressen abzubilden. Per Definition kann jeder Firma genau eine __Standardadresse__ zugewiesen werden. Verfügt eine Firma über mehr als eine Adresse kann die Standardadresse mit Hilfe der Funktion *Zur Standardadresse machen* verändert werden.

Vorhandene Adressen können mit dem Verweis auf die *VCard* pro Adresse heruntergeladen werden und so z.B. in Outlook importiert werden.

![Adresse hinzufügen - im Mobilen CRM](img/company/new_address_mobile_crm_material_client.png "Adresse hinzufügen - im Mobilen CRM")

![Adresse zur Standardadresse machen - im Mobilen CRM](img/company/new_standard_address_mobile_crm_material_client.png "Adresse zur Standardadresse machen - im Mobilen CRM")

## Kontext - Personen bei dieser Firma
Im Kontext der Firma wird eine Liste von Personen angeboten. Diese Personen sind sowohl der Firma als auch einer der gespeicherten Adressen der Firma zugeordnet. Die Darstellung kann zwischen aktiven und inaktiven (ausgeschiedenen) Personen umgeschaltet werden.

![Personen bei dieser Firma](img/company/context_staff.png "Personen bei dieser Firma")

Zu jeder Person wird ein Verweis auf die Detaildarstellung sowie weiterführende Informationen und primäre Kommunikationsdaten angeboten. Besonders relevante Personen können mit Hilfe einer Kennzeichnung von anderen unterschieden werden.
