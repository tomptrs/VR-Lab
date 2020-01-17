# VR-Lab
## Woordje uitleg
Voor de stadsdichter van Antwerpen moesten we een vr project maken. Wij hebben gekozen voor het gedicht : Het huis in mij. We hebben een vr project gerealiseerd met 360° video.
Het is geschreven voor de ocolus quest. 

## Opstarten van een project voor de oculus Quest.
Een handig youtube filmpje voor het opstarten van een unity project voor de quest : https://www.youtube.com/watch?v=qiJpjnzW-mw&t=223s

Wat we nodig hebben voor we beginnen is een unity versie met de android build support ook geïnstalleerd. (Dit kan toegevoegd worden op al eerder geïnstalleerde unity versies.)

1. We beginnen met het aanmaken van een nieuw 3D unity project.
![Create](/afbeeldingen/create.png)

2. We gaan beginnen met in de asset store de oculus integration plugin te downloaden.
![oculusPlugin](/afbeeldingen/oculusplugin.png)

2. Bij de build settings moeten we nu het platform wijzigen naar Android. (Als er nog geen android module geladen is moet je deze installeren met de open download page knop.)
![build Settings](/afbeeldingen/buildSetting.png)

3. Als we veranderd zijn van platform moeten we nu naar de player settings gaan en hier een paar instellingen veranderen. 

4. Wat we eerst bij de player settings gaan veranderen is bij de XR settings Virtual Reality toelaten en de oculus SDK toevoegen.
![xr settings](/afbeeldingen/xrsettings.png)

5. Hierna gaan we bij de plater settings naar de tab Other Settings en hier moeten we twee dingen veranderen. Voor te beginnen moet de Vulkan grafics API verwijderd worden en moet de API veranderen naar een hogere API. Dit moet minimum de 4.4 API zijn 
![API](/afbeeldingen/API.png)
![graphics API](/afbleedingen/grapicsAPI.png)

6. Nu is het unity project klaar en moeten we nog een paar dingen doen voordat we het unity project kunnen pushen naar de quest. In de oculus app waarmee je de quest hebt verbonden op more settings drukken developer settings en de developper mode enablen. Als we dit gedaan hebben gaan we de de bril verbinden met de computer. Dit doen we met de usb C kabel dat meegeleverd is. Als we dit gedaan hebben krijgen we in de bril een popup of we USB debugging toestaan van deze computer. Vink always allow aan en accepteer de popup.

Nu zijn we helemaal klaar voor een app te schrijven voor de Oculus Quest.
