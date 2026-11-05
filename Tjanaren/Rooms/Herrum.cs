using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Herrum : Rum
    {
        public Herrum() : base("Herrum",
            "Du kliver in i ett påkostat herrum. Väggarna är klädda " +
"med mörka träpaneler och luften bär en tydlig doft av " +
"gammalt läder och piprök. " +
"Ett bastant skrivbord av massiv ek dominerar rummet. " +
"På skrivbordet ligger flera prydligt ordnade dokument " +
"och en tung brevpress. " +
"Vid ena väggen står en djup läderfåtölj som ser ut att " +
"vara lika gammal som husets herre. " +
"Bredvid fåtöljen finns ett litet sidobord med ett " +
"elegant rökställ. I rökstället vilar en pipa och " +
"bredvid står ett rejält askfat. " +
"Här inne råder en helt annan standard än i " +
"tjänstefolkets utrymmen. Herr von Krångel har " +
"uppenbarligen aldrig ansett att sparsamhet behöver " +
"gälla honom själv."
            )
        {
            VisaNamn = "i herrummet";
            OppnaUtgangar("Vaster");

            SakerIRummet.Add(new Inredning("Skrivbord", "Du undersöker det bastanta ekskrivbordet. Träytan är " +
"blankpolerad och kanterna är omsorgsfullt snidade. " +
"På skrivbordet ligger en tung brevpress av mässing " +
"och flera dokument ordnade i prydliga högar. " +
"En gammaldags skrivbordslampa kastar ett varmt sken " +
"över handlingarna. Det verkar som om herr von Krångel " +
"fortfarande ägnar en hel del tid åt pappersarbete, " +
"trots att han sedan länge lämnat arbetslivet bakom sig.", false, false));

            SakerIRummet.Add(new Inredning("Dokument", "Du börjar försiktigt bläddra bland dokumenten på " +
"skrivbordet. De flesta verkar vara gamla juridiska " +
"handlingar, prydligt sorterade efter datum. " +
"På flera av dem återkommer namnet Karl Wilhelm von Krångel, " +
"före detta domare. " +
"Ett särskilt dokument fångar din uppmärksamhet. " +
"Det verkar vara ett utkast till en rättslig skrivelse " +
"angående stöld, olovligt nyttjande och åverkan på " +
"privat egendom. " +
"Dokumentet är ännu inte undertecknat, men formuleringarna " +
"är synnerligen skarpa. " +
"Du får en obehaglig känsla av att herr von Krångel " +
"fortfarande betraktar varje övertramp som ett brottmål. " +
"Du lägger försiktigt tillbaka handlingarna precis " +
"som du fann dem.", false, false));

            SakerIRummet.Add(new Inredning("Fåtölj", "Du undersöker den stora läderfåtöljen. Det mörka lädret " +
"är mjukt och armstöden har blivit blanknötta av många " +
"års användning. " +
"När du böjer dig ner upptäcker du hörnet av ett gulnat " +
"papper som sticker fram under ena fåtöljbenet. " +
"Du skjuter försiktigt fåtöljen åt sidan och plockar " +
"upp dokumentet. Överst står det: SKRIFTLIG ERINRAN. " +
"Till: Karl Wilhelm von Krångel. " +
"Du läser vidare: \"Med anledning av upprepade klagomål " +
"beträffande ert uppträdande i tjänsten har det beslutats " +
"att tilldela er en skriftlig erinran. Ert sätt att bemöta " +
"såväl kollegor som allmänhet har vid flera tillfällen " +
"bedömts som synnerligen olämpligt.\" " +
"Längst ner finns en handskriven anteckning: " +
"\"Fullständigt befängt! Jag har endast upprätthållit " +
"ordningen!\" " +
"Du kan inte låta bli att dra på munnen. Herr von Krångel " +
"tycks inte alltid ha varit lika förtjust i varningar " +
"när han själv varit mottagare. " +
"Du lägger tillbaka dokumentet under fåtöljen och " +
"skjuter försiktigt tillbaka den på sin plats.", false, false));
        }
    }
}
