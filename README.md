# ScriptBan Plugin for KEKCON Tool (BattlEye Client)
**2016 (c) by Cris - Regnum4Games**

[![N|Solid](https://www.regnum4games.de/upload/r4g_logo.png)](https://www.regnum4games.de)


### Erklärung
> Bekannterweise kickt der BattlEye Server Spieler vom Server, wenn Sie zum Beispiel eine Script Restriction oder RemoteExecution Restriction hervorrufen. Doch es gibt auch andere Restriction, beispielweise createVehicle Restriction, für die der Spieler nicht einmal gekickt wird, sondern nur Logs angelegt werden. Dieses Plugin bannt den "Hacker"/Verursacher sofort permanent, um weiteren Schaden zu verhindern.



### Download
[*** zum Download ***](https://github.com/kadse/ScriptBan/releases)


### Features
- **File Logging** in einer Textdatei pro Spieler mit Zeit und Log des ausgeführten Befehls in [spielername_guid.txt] *(optional)*
- Angabe eines eigenes Pfades für Speicherort der Logfiles
- **Datenbank Logging** in einer Datenbanktabelle mit Zeit und Log des ausgeführten Befehls *(optional)*
- Bangrund selber in der Config definierbar

### Konfiguration

Beispiel config-Datei: `plugins/config/scriptban.cfg`

**Alle Parameter müssen in der Config enthalten sein, sonst kommt ein Fehler beim Laden des Plugins in KEKCON !**


***filelogging***
- *true:* File Logging aktiviert
- *false:* File Logging deaktiviert
```sh
filelogging = false
filelogging = true
```

***filepath***

Falls *filelogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst hier den Ordner-Pfad zum gewünschten Log-Ordner eintragen.
```sh
filepath = C:\A3Master\Logs
```

***dblogging***
- *true:* Datenbank Logging aktiviert
- *false:* Datenbank Logging deaktiviert
```sh
dblogging = false
dblogging = true
```

***dbserver***

Falls *dblogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst hier die IP vom Datenbankserver eintragen.
```sh
dbserver = 127.0.0.1
```

***dbname***

Falls *dblogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst hier den Datenbanknamen eintragen.
```sh
dbname = dbname
```

***dbuser***

Falls *dblogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst hier den Datenbank-Nutzer eintragen.
```sh
dbuser = dbuser
```

***dbpassword***

Falls *dblogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst hier das Password für den Datenbank-Nutzer eintragen.
```sh
dbpassword = pw
```

***tablename***

Falls *dblogging* deaktiviert wurde, kann die Zeile unverändert bleiben.
Sonst kann hier ein eigener Name für die Tabelle in der Datenbank angegeben werden.
```sh
tablename = banlogs
```

***banreason***

Hier kann ein eigener Bangrund angegeben werden, der in der `bans.txt` vom BattlEye Server eingetragen wird.
```sh
banreason = AutoBan | perm
```


### Lizenz

Die Nutzung dieses Plugins ist komplett kostenlos. Mit der Nutzung dieses Plugins erklärt sich der Nutzer automatisch einverstanden, dieses Plugin und den Quellcode nicht als eigenes Werk anzupreisen. Sollte dies missachtet werden, können weitere rechtliche Schritte eingeleitet werden. Genauso ist die kommerzielle Weiterverbreitung des Plugins strengstens untersagt. Dieses Plugin wurde für die ArmA 3 Community und dessen Game Server entwickelt und kostenfrei zur Verfügung gestellt, um Hacker vom Spielgeschehen schnell und effizient ausschließen zu können.

### Spenden
Das Plugin stammt aus dem "Hause" Regnum4Games,wo ebenfalls ein Arma 3 Server betrieben wird. Sollte Euch dieses Plugin gefallen und Ihr wollt unseren Server ein wenig unterstützen, kommt einfach mal vorbei. Außerdem freuen wir uns über jede kleine Spende, die zum Erhalt &  Weiterentwicklung unseres Servers beiträgt, so beispielweise auch zur Entwicklung weiterer Plugins oder Weiterentwicklung diesen Plugins.

### Verbesserungsvorschläge
Wünscht Ihr Euch gerne noch die ein oder andere kleine Änderung bzw. Verbesserung an diesem Plugin, könnt Ihr mich einfach über unsere Homepage, im TeamSpeak³ oder per E-Mail kontaktieren.



**Homepage:** [www.regnum4games.de](https://www.regnum4games.de) 

**Forum:** [www.regnum4games.de/forum/](https://www.regnum4games.de/forum/)

**TeamSpeak³:** [ts.regnum4games.de](http://ts.regnum4games.de)

**E-Mail:** [cris@regnum4games.de](mailto:cris@regnum4games.de)

[![paypal](https://www.paypalobjects.com/de_DE/DE/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=regnum4games%40web%2ede&lc=DE&item_name=Regnum4Games-ScriptBan-Plugin-Spende&no_note=0&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHostedGuest)
