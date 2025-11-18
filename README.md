# PopFlix Backend

## Projektbeskrivelse
PopFlix Backend er et Proof-of-Concept streaming-API udviklet i .NET 9 WebAPI med MongoDB som database.  
Projektet gør det muligt at uploade videofiler, lagre dem i MongoDB via GridFS, hente metadata og streame film direkte gennem API’et.  

Formålet er at demonstrere centrale backend-teknikker som:
- REST-design  
- Håndtering af binære data  
- Asynkron databaseadgang  
- Containerisering med Docker  

## Hvad demonstrerer projektet?
- RESTful API-udvikling i .NET Core  
- Lagring og håndtering af binære filer via MongoDB GridFS  
- Streaming af videofiler over HTTP  
- Containerisering ved hjælp af Docker Compose  
- Brug af Postman til test  
- Dokumentation med Swagger/OpenAPI  

## Forudsætninger
For at kunne køre projektet skal følgende være installeret og kørende:
- Visual Studio 2022 (med .NET 9 SDK)  
- Docker Desktop  
- Postman
  
Sørg derudover for at have følgende til rådighed: 
- Videofiler (under 1GB) til test  

## Opsætning og kørsel

### 1. Klon projektet
1. Tryk på **<> Code** i GitHub  
2. Vælg **Open with Visual Studio**  
3. Bekræft popup, hvis den vises  
4. Projektet åbner i Visual Studio → tryk **Clone**

### 2. Start Docker-miljøet
- Inden du går i gang: Vær sikker på at Docker Desktop er kørende
- Åbn terminalen i Visual Studio:
View → Terminal → powershell åbner 

- Navigér til backend-projektet ved at skrive:
cd PopFlixBackend → tryk enter 

- Byg og start containere ved at skrive:
docker-compose up --build → tryk enter. 
- Kontrollér derefter i Docker Desktop, at containerne kører.

## Test i Postman 
POST: Upload film: 
Lav en POST request: http://localhost:8080/movies/import 
- Naviger til “Body” og tryk på den
- Skift dropdown fra "none" til form-data"
- Under Key skriver du “file” og ændrer typen til “File” 
- Under Value: tryk på feltet “New file from local machine” 
- I popup-vinduet, vælg en fil der skal uploades og tryk på “open”
- Lav endnu en entry hvor Key er “title” og typen er “Text” 
- Under Value: skriv det ønskede navn på filen 
- Tryk på “Send”
- Efter upload returnerer API’et metadata for filmen.
- Gentag processen for alle ønskede filer.

GET: Alle film med metadata:
- Lav en GET request: http://localhost:8080/movies → tryk SEND 
- Returnerer en liste med alle film og deres meteadata, inkl. deres id’er.
- Kopier feltet "Id" fra én af de uploadede film (skal bruges i næste trin) 

GET: Enkelt filmmetadata: 
- Lav en GET request: http://localhost:8080/movies/{id} → tryk SEND
- Returnerer metadata på den film hvis id du har indsat

GET: Stream film: 
- Lav en GET request: http://localhost:8080/movies/{{id}}/stream → tryk SEND
- Du bør nu kunne streame den valgte video. 

## API-dokumentation:
- Swagger/OpenAPI dokumentation findes i projektets rodmappe som: Swagger doc
- Dokumentationen kan åbnes i enhver Swagger viewer og vises automatisk, når API’et kører.

## Teknologier anvendt:
- .NET 9 WebAPI
- MongoDB
- GridFS
- Docker Compose
- Postman
