# Notetringar

 - Använder SQLServer via Entity Framerwork. Databasen behöver vara skapad men med hjälp av migrations så skpas innehållet av databasen automatiskt när API:et startar.
 - Man behöver lägga in connectionstring till databasen i appsettings.json


# CoDodo Test

CoCreates Delivery Office(DO) har ett excel dokument kallat "Medarbetaredokument.xlsx" som de använder för att hålla koll på var i ansökningsprocessen(process) till möjliga uppdrag(opportunity) vi befinner oss.
En applikation kallad CoDodo håller på att utvecklas för att ersätta detta. Ett api gjort med Asp.net Core har redan påbörjats och innehåller funktionalitet för att skapa, ta bort och hämta processer. Det finns även en endpoint som kan importera medarbetaredokumentet så att övergången från excel till CoDodo blir smidigare för DO. Importen av medarbetardokumentet skall endast användas vid övergången till CoDodo.

---
#### Dina uppgifter blir följande:

1) Lägga till en databas

   För närvarande sparas all data bara in-memory. Du kan fritt välja databas, men något som inte är allt för avancerat att sätta upp, till exempel SQLite, Postgres, SqlServer eller liknande är att föredra så vi kan testa din kod.

2) Möjlighet att uppdatera en process

   En process uppdateras flera gånger genom sin livstid i och med att vi går vidare i ansökningsprocessen. Något sätt att uppdatera den på i CoDodo behövs alltså. Om uppdateringen är att vi vunnit en process behöver detta rapporteras till CoFinder, som är vårt interna verktyg för att hitta uppdrag.

4) Bugfixar och städa upp

   CododoApi:et innehåller rätt mycket tveksam kod, om du ser något du tycker är fel eller dåligt gjort är det bara att fixa.
   
Skapa en fork av det här repot och skicka länken till oss. Om du inte vill göra koden publik så behöver du bjuda in en av våra utvecklare till repot.
       
**Viktigt❗**
> Du behöver inte göra alla tre uppgifter, det räcker att du gör tillräckligt mycket för att vi skall kunna ha en diskussion om det du gjort. Det är viktigare att du gör saker som visar vad du är bra på än att lösa allt.

**Tips💡**
>Använd Api:ets "/api/ImportExcel" för att få lite test-data att använda, ett exempel på ett medarbetar-dokument finns i git-repot under "Dokument".

---
#### Opportunity
En opportunity är ett möjligt uppdrag för oss, de identifieras av Uri:n för uppdrags-annonsen. I  medarbetardokumentet har Uri:er inte sparats, i excel importen används en GUID i stället för en Uri. En opportunity kan ha ansökningar ifrån flera medarbetare, det vill säga att en opportunity kan förekomma i flera processer.

#### Process
En process är en medarbetares/konsults ansökan till ett uppdrag.
En process identifieras unikt av namnet på konsulten som ingår i den samt opportunityns Uri.
En process brukar börja som "OFFERED" och sedan gå vidare till "INTERVIEW" och sedan till "ASSIGNED", om vi inte förlorar ansökan då den hamnar i "LOST". Uppdrag som vi vunnit, alltså blivit "ASSIGNED" skall skickas vidare till CoFinder.
För att se vad som krävs att vi skickar till CoFinder och för att testa kan du använda CoFinderTester projektet.
