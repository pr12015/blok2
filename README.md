# Grupa 27
* Stefan Stipanovic, PR1/2015
* Aleksandar Copic, PR72/2015
* Milan Petrovic, PR53/2015
* Miroslav Tanasic, PR112/2015

## Projektni zadatak 27

# Sigurnost i bezbednost u elektroenergetskim sistemima

# Primenjeno softversko inzinjerstvo

### Opis resavanog problema
Tema projektnog zadatka je implementacija sistema za obradu merenaj pametnih brojila i cuvanje podataka o ocitanoj potrosnji. Za svako brojilo servis u bazi podataka cuva sledece informacije:
* jedinstveni identifikator brojila (8-cifren broj)
* ime i prezime vlasnika
* utrosena elektricna energija.

U sistemu postoji 4 tipa korisnika:
* Potrosac - ima pravo da procita stanje brojila.
* Operator - pravo modifikacije ID-a i stanja brojila.
* Administrator - ima pravo dodavanja i brosanja brojila iz baze.
* Super-korisnik - ima pravo da brise celu bazu podataka.

Server se sastoji i iz komponente Load Balancer (LB) koja komunicira sa Workerima (W) kako bi izracunala potrosnju elektricne energije.

Komunikacija izmedju LB i W se uspostavlja uz pomoc sertifikata po pravilu lanca poverenja, dok izmedju korisnika i servera uz pomoc Windows autentifikacije.

Svi pokusaji uspesne i neuspesne autorizacije su logovani u custom kreiranom Windows Event Log-u.

### Teorijske osnove
Besbednosni mehanizmi koji su korisceni pri izradi zadatka:
Windows autentifikacija i sertifikati. Servisi se medjusobno jedan drugom javljaju bez slanja poverljivih podataka, ali kod NTLM autentifikacionog protokola izostaje verifikacija strane koja je primila zahtev. Komunikacija izmedju LB i W komponenti je zasnovana na sertifikatima, izdatim od sertifikacionog tela TestCA. Ono izdaje sertifikate za LB i W. U sertifikat se ugradjuje javni kljuc korisnika, dok se tajni kljuc ne razmenjuje. Autorizacija prava pristupa se vrsi primenom
RBAC autorizacione seme na osnovu rola koje se citaju iz rbac_config.xml fajla.

### Dizajn implementiranog sistema
![Dizajn](https://github.com/pr12015/images/blob/master/design.png)

### Testiranje sistema
Sistem je testiran tako sto je u startu pokrenuto N workera, koji ce se peridicno gasiti, kako bi se proverilo azuriranje cost factora. Takodje posle izvesnog vremena ce sa paliti i novi workeri koji ce se prijavljivati na LB. Nakon workera bice pokrenuto M korisnika koji ce konstanto slati zahteve za obracunom potrosnje elektricne energije.

Rezultati testiranja autorizacije:
![Autorizacija](https://github.com/pr12015/images/blob/master/Capture.PNG)
![Autorizacija](https://github.com/pr12015/images/blob/master/Capture1.PNG)
![Autorizacija](https://github.com/pr12015/images/blob/master/Capture2.PNG)
![Autorizacija](https://github.com/pr12015/images/blob/master/Capture3.PNG)
