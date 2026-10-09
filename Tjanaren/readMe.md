# Tjänaren – ett humoristiskt textäventyr på svenska

## Bakgrunden till Tjänaren

Under min praktik arbetade jag med projektet *VR i vården*, där min huvudsakliga uppgift var att färdigställa en webbportal för hantering av upplevelser och användare.

När arbetet kom in på utvecklingen av själva upplevelserna upptäckte jag ett problem. De etablerade verktygen för spelutveckling är generellt inte tillgängliga med skärmläsare.

Jag undersökte möjligheterna och hittade inget som fungerade för mig.

Det väckte en fråga som jag spontant ställde till AI:

**"Kan vi utveckla ett spel som är tillgängligt för blinda?" * *

Den frågan blev startskottet för *Tjänaren*, ett svenskspråkigt textäventyr för Windows där tillgänglighet med tangentbord och skärmläsare är en grundläggande del av utvecklingen.

## Från idé till spel

Som blind har jag under många år spelat textbaserade äventyrsspel. Till skillnad från traditionella grafiska spel kan textäventyr göras tillgängliga med hjälp av tangentbord och skärmläsare.

En annan anledning till att jag valde just textäventyr är att utbudet på svenska är begränsat. Det finns gamla klassiker som *Stugan*, men jag ville skapa något nytt, utvecklat för Windows.

Min ambition var inte att skapa ett textäventyr där spelaren bara går mellan olika rum och utför självklara uppgifter. Jag ville skapa ett riktigt äventyr, med en berättelse som engagerar, humoristiska inslag och pussel som kräver eftertanke.

Resultatet blev idén till *Tjänaren*, där spelaren arbetar åt den högfärdiga familjen von Krångel. Här gäller det att utföra sina arbetsuppgifter, lösa problem och försöka hålla sig väl med ett herrskap som inte direkt är känt för sin ödmjukhet.

Humorn är en viktig del av spelidén. Syrliga kommentarer, små gliringar och oväntade reaktioner ska bidra till att göra spelupplevelsen underhållande.

Samtidigt ska pusslen bjuda på motstånd. Min erfarenhet av äventyrsspel har lärt mig att en stor del av nöjet ligger i att själv lista ut hur saker och ting hänger ihop.

## Från Program.cs till en återanvändbar spelmotor

När utvecklingen av *Tjänaren* började var det en enkel konsolapplikation i C#. All kod låg i en enda fil, `Program.cs`, och målet var till en början att utforska hur ett textbaserat spel skulle kunna fungera.

Men i takt med att nya rum, föremål och spelmekaniker tillkom växte också behovet av en bättre struktur.

Jag började därför dela upp koden i separata klasser med tydliga ansvarsområden. Spelmotorn fick hantera spelarens kommandon och spelets övergripande funktioner, medan de olika rummen fick egna klasser som hanterar sina beskrivningar, föremål och händelser.

Även spelarens inventarier, utrustning och de tillstånd som påverkar berättelsens fortsättning fick egna delar i strukturen.

Så småningom flyttades spelet från konsolen till Windows Forms, där jag kunde skapa ett gränssnitt anpassat för tangentbordsnavigering och skärmläsare.

Det var först efter hand som jag insåg vad jag faktiskt hade byggt. Utan att det varit min ursprungliga avsikt hade projektet utvecklats till en spelmotor som till stora delar var fristående från själva berättelsen.

Det innebar att grunden skulle kunna återanvändas för helt andra textäventyr genom att skapa nya rum, föremål och berättelser, utan att behöva börja om från början.

För mig har projektet också varit ett sätt att fördjupa mina kunskaper i objektorienterad programmering. Det som började som en relativt liten övning har utvecklats till ett större projekt där jag fått omsätta teori i praktiken.

## Tillgänglighet och användarupplevelse

Tillgänglighet har varit en grundläggande del av *Tjänaren* sedan projektet startade. Som blind utvecklare använder jag själv skärmläsare och har därför kunnat utgå från mina egna erfarenheter när jag utformat spelets gränssnitt.

Spelet är utvecklat för Windows med Windows Forms och är tänkt att kunna spelas med tangentbord och skärmläsare, utan att spelaren behöver använda musen.

Spelaren navigerar mellan olika rum med piltangenterna, medan information om omgivningen, föremål och händelser presenteras i text. Inventarier och andra funktioner hanteras också via tangentbordet.

En viktig del av utvecklingsarbetet har varit att undersöka hur gränssnittets fokus fungerar tillsammans med skärmläsare.

Jag har också arbetat med att göra spelets olika funktioner begripliga utan visuella ledtrådar. Målet är att spelaren ska kunna utforska omgivningen, lösa pussel och följa berättelsen genom text och tangentbordsstyrning.

Tillgängligheten utvecklas och testas fortlöpande i takt med att spelet får fler funktioner. Ambitionen är att skapa en spelupplevelse där tillgänglighet är en självklar del av konstruktionen, inte något som läggs till i efterhand.

## Spelmekanik och funktioner

I *Tjänaren* får spelaren utforska familjen von Krångels hem och dess omgivningar. Som tjänare förväntas man utföra olika arbetsuppgifter och lösa de problem som uppstår, helst utan att dra på sig herrskapets missnöje.

Spelet bygger på klassiska textäventyrsmekanismer där spelaren undersöker sin omgivning, samlar föremål och använder dem för att lösa olika pussel.

Föremål kan förvaras i spelarens inventarium, och vissa uppgifter kräver att rätt verktyg eller utrustning används vid rätt tillfälle.

Spelarens handlingar påverkar också berättelsens fortsättning. Vissa händelser och möjligheter blir tillgängliga först när andra uppgifter har genomförts.

En särskild spelmekanik är systemet med övertramp. Familjen von Krångel har begränsat tålamod med sin personal, och vissa mindre välbetänkta handlingar kan få konsekvenser.

Ambitionen är att kombinera dessa mekanismer med en humoristisk berättelse och pussel som kräver att spelaren tänker efter, experimenterar och använder sin uppfinningsrikedom.

## Projektstatus och fortsatt utveckling

*Tjänaren* är fortfarande under aktiv utveckling och publiceras på GitHub för att visa mitt arbete med C#, objektorienterad programmering, tillgänglighet och utveckling av en återanvändbar spelmotor.

Projektet är ännu inte färdigt för en officiell spelrelease. Berättelsen, pusslen och vissa funktioner utvecklas fortfarande, samtidigt som spelet testas och förbättras.

Någon färdig körbar version för nedladdning finns därför inte i nuläget.

Målet är att färdigställa spelet och på sikt kunna erbjuda en komplett spelupplevelse för Windows.