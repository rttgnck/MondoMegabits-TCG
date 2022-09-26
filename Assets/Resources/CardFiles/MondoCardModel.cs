// MondoCardModel
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Mondo Card information model used in reading the JSON data file about the cards 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

namespace Mondo.MondoCards
{
    [System.Serializable]
    public class MondoCardModel
    {
        public int id;
        public string name;
        public string faction;
        public int rarity;
        public string frameSize;
        public string type;
        public string subtype;
        public string bodyText;
    }
}


/////////////////////////////////////////////EXAMPLE OF CARD DATA/////////////////////////////////////////////
//{
//    id: 1,
//    name: "Wireless Vaccine+-",
//    faction: "FAKE TECH",
//    rarity: 4,
//    frameSize: "C",
//    type: "Item",
//    subtype: "Weapon",
//    bodyText: `"Not feeling well? Well, maybe it's 'cause you're always on that phone."
//
//                Materials Needed to Craft: 4x San Francisco roadside needle, 1x Malaysian Airlines black box wireless receiver, used 9mm hollow point round (commonly found inside dead gangstalkers), vaccine of choice
//                Retail Cost: $155,000($5.23 if you have insurance(you don't))
//                Network Test: 7.7 Gbps Download / 35 Mbps upload
//
//                Remotely inject any known vaccine into another person's genome--- yeah, get it in there, let it mess around inside.. Commonly utilized near elementary schools and fast food joints. Once applied to a target they become sluggish and weak, also they can't get hard anymore, permanently.
//
//                Rapivax: Deals 25 damage for each vaccine;
//                Unlimited use;
//                15 % chance of turning enemy 100% gay (flip a coin three times like an idiot with nothing better to do) Player may not calculate odds using anything other than a coin.
//                Rapivax is double-certified "safe 100%" by both the FDA and Dept. of Agriculture.
//
//                Bugelsil: Deals no damage;
//                One use only;
//                Bugelsil affects every enemy card which already suffers from any other vaccine status effect;
//                Bugelsil has no effect on unvaccinated cards;
//                Strange side effect: The high acidity of the Bugelsil vaccine causes all metal enemy item cards to corrode and break (opponent must return them to bottom of his deck); 50 / 50 % chance enemy card(s) become gay OR asexual (flip a coin).
//                Bugelsil is triple - certified safe by the D.O.T., the Office on Violence Against Women, and the FDA.
//
//                Provaxtrin: Blocks HIV and HIV Ex Plus a for ten (10) turns;
//                Can choose multiple targets for a dose but it is one-time use only;
//                Lowers target INT by 2;
//                Player may target either his own creatures or opponent's, or both. A good strategic choice if you have many homosexual cards and your opponent has many high Intelligence cards. any safety certification (FDA and Dept. of Vaccinations pending as of publication).`,
//}
/////////////////////////////////////////////EXAMPLE OF CARD DATA/////////////////////////////////////////////