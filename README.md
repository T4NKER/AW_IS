# AgileWorksi Proovitöö/ AgileWorks internship homework

## Kuidas jooksutada

Veenduge, et teil oleks installitud .NET 8.0. Selle projekti loomiseks kasutasin Visual Studio Code'is WSL-i.

Kirjuta terminali

`dotnet restore && dotnet run`

selleks, et kasutada RequestService'i jaoks ühikteste

`cd Tests && dotnet test`

Mul oli Microsoft TestPlatform utilities'iga probleem, nii et ülalmainitud käsk töötas minu jaoks, lihtsalt "dotnet test" võib töötada ka rootis.


#### Lisainformatsioon

Selle projekti eesmärk oli luua päringuhaldur, kuhu kasutaja saaks taotlusi sisestada, vaadata ja kustutada. Päringud tuleks sortida tähtaja aja järgi kasvavas järjekorras ja kui tähtajani on tund või vähem või see on juba möödas, kuvatakse päring punasena. Lõin täiendava SSE-teenuse ehk Server-Sent Events Service, mis saadab uue taotluse esitamisel või vana kustutamisel sõnumi kõigile klientidele, kellel on RequestsView avatud. Kuigi see pole nii kasutajasõbralik, tagab see uue teabe viivitamatu edastamise kliendile.


#
#
#

## How to run

Make sure you have .NET 8.0 installed. I used WSL in Visual Studio Code to create this project.

Inside the terminal use 

`dotnet restore && dotnet run`

to use unit tests I created for RequestService

`cd Tests && dotnet test` 

I had a Microsoft TestPlatform utilities problem so this command worked for me, just      `dotnet test` might work in the root directory aswell.


#### Additional information

The goal of this project was to create a Request Manager, where the user could insert, view, and delete requests. The requests should be sorted by deadline time ascending, and when the time until the deadline is an hour or less or has already passed, it should display the request as red. I created an additional SSEService, or Server-Sent Events Service, which sends a message to all clients that have the RequestsView open whenever a new request is submitted or an old one deleted. Although it is not as user-friendly, it ensures that any new information gets passed to the client immediately.

