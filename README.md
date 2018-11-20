# ScriptBan Plugin for KEKCON Tool (BattlEye Client)
**2016 (c) by Cris - App-Raisal**

### Erklärung
> Bekannterweise kickt der BattlEye Server Spieler vom Server, wenn Sie zum Beispiel eine Script Restriction oder RemoteExecution Restriction hervorrufen. Dieses Plugin bannt den "Hacker"/Verursacher sofort permanent, um weiteren Schaden zu verhindern.



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
oder
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
oder
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

***banrestrictions***

Hier können alle BattlEye Restrictions eingetragen werden, für die das Plugin den Spieler bannen soll.
Diese werden hintereinander, durch Kommas getrennt, angegeben.
```sh
banrestrictions = attachto,createvehicle,deletevehicle,mpeventhandler,publicvariable,remoteexec,script,setdamage,setvariable
```


### Lizenz

Die Nutzung dieses Plugins ist komplett kostenlos. Mit der Nutzung dieses Plugins erklärt sich der Nutzer automatisch einverstanden, dieses Plugin und den Quellcode nicht als eigenes Werk anzupreisen. Sollte dies missachtet werden, können weitere rechtliche Schritte eingeleitet werden. Genauso ist die kommerzielle Weiterverbreitung des Plugins strengstens untersagt. Dieses Plugin wurde für die ArmA 3 Community und deren Game Server entwickelt und kostenfrei zur Verfügung gestellt, um Hacker vom Spielgeschehen schnell und effizient ausschließen zu können.


### Spenden
Das Plugin stammt aus dem "Hause" Regnum4Games,wo ebenfalls ein Arma 3 Server betrieben wird. Sollte Euch dieses Plugin gefallen und Ihr wollt unseren Server ein wenig unterstützen, kommt einfach mal vorbei. Außerdem freuen wir uns über jede kleine Spende, die zum Erhalt &  Weiterentwicklung unseres Servers beiträgt, so beispielweise auch zur Entwicklung weiterer Plugins oder Weiterentwicklung diesen Plugins.


### Verbesserungsvorschläge
Wünscht Ihr Euch gerne noch die ein oder andere kleine Änderung bzw. Verbesserung an diesem Plugin, könnt Ihr mich einfach über unsere Homepage, im TeamSpeak³ oder per E-Mail kontaktieren.



**TeamSpeak³:** [ts.app-raisal.de](https://ts.app-raisal.de/)

**E-Mail:** [cris@app-raisal.de](mailto:cris@app-raisal.de)

**Spenden:** [paypal.me/kadse1301](https://paypal.me/kadse1301)
