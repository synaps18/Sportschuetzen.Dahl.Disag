# Disag Server

## Funktionen

Der DISAG Server stellt eine REST API für eine verbundene DISAG Maschine bereit. Es ist somit z.b. möglich einen Raspberry Pi an die DISAG anzuschließen und die Maschine somit über das Netzwerk anzusteuern.

### DISAG RM III

#### Voraussetzungen
Die RM3 muss über ein serielles RS232 Kabel an einen Computer angeschlossen werden. Dazu kann ein USB Adapter verwendet werden. Damit die Maschine auf Befehle vom Computer reagiert, muss diese in den "Win Mode" geschaltet werden.

#### Unterstützte Funktionen:

- Seriennummer lesen
- Maschinentyp lesen
- Win Mode schalten
- Normal Mode schalten
- Serien lesen (mit Aufdruck)
- Abbrechen 

