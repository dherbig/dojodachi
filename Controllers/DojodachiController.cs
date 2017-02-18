using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojodachi.Controllers
{
    public class DojodachiController : Controller
    {
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // This helps me know if the Dachi was made this round.
            Boolean check = false;
            // If there is no Dachi data...
            if(HttpContext.Session.GetInt32("Full") == null){
                //reset
                Reset();
                check = true;
            } 
            // This function converts our Session data into a Dojodachi object.
            Dojodachi Ada = Summon();
            Bag(Ada);
            if(check == true){
                TempData["Action"] = "A new Dojodachi, lovingly named " + Ada.Name + ", opens it's eyes for the first time, ready and eager for you to show it the world. :)";
                TempData["Img"] = "init.png";
            }
            // That allows us to refer to all of the attributes of our Dachi directly.
            if(Ada.Full <= 0){
                //Check for Loss
                TempData["Action"] = "You bastard! You didn't feed " + Ada.Name + " enough, and now it has starved to death.  You can try again, but if that's how you treat your pets, maybe you should step aside and let someone more responsible take over...";
                TempData["Img"] = "dead.png";
                return RedirectToAction("Index");
            } else if(Ada.Happy <= 0) {
                TempData["Action"] = "Overcome with enuii, " + Ada.Name + " decided that the meaningless life of a digital pet was not a life worth 'living', and has gone ahead and offed itself.  Yikes.  Try again, and...try to be more interesting, will ya? We only have [INFINITY] of these things left...";
                TempData["Img"] = "ennui.png";
                return RedirectToAction("Index");
                // Check for win
            } else if(Ada.Energy > 99 && Ada.Happy > 99 && Ada.Full > 99){
                TempData["Action"] = "Congratulations. " + Ada.Name + " couldn't be better off. With all of it's needs met, " + Ada.Name + " is now fully grown and is off to... IDK... College or something.  A farm upstate?  Whatever. You won!  Press reset to start over!";
                TempData["Img"] = "begin.png";
            }
            //Fill ViewBag
            if(TempData["Img"] == null){
                TempData["Img"] = "sleepy.png";
            }
            if(TempData["Action"] == null){
                TempData["Action"] = "Come on!  Do something!";
            }
            Save(Ada);
            Bag(Ada);
            return View();
        }

        // Starts or resets the Dachi, and returns to the index.
        public IActionResult Reset(){
            HttpContext.Session.Clear();
            string[] NameSet = {"Mork", "Doopy", "Squidface", "Dartangian", "Yo Momma", "Cancer", "Mika", "Domblebum", "Kora", "Blowhold", "Spike", "Wagner", "Lovelace", "Puke-at-chu", "Hodor", "Capybara", "Nok Nok", "Lothar the Wise, Ruler of the Fifteen Tribes, Slaughterer of Heritics, and Destroyer of Worlds"};
            Random rand = new Random();
            String Hombre = NameSet[rand.Next(NameSet.Length-1)];
            Dojodachi Pam = new Dojodachi(Hombre);
            Save(Pam);
            Bag(Pam);
            return RedirectToAction("Index");
        }

        // Creates a Dojodachi object using the current session values.
        public Dojodachi Summon(){ 
            string Name = HttpContext.Session.GetString("Name");
            int Happy = (int)HttpContext.Session.GetInt32("Happy");
            int Energy = (int)HttpContext.Session.GetInt32("Energy");
            int Full = (int)HttpContext.Session.GetInt32("Full");
            int Meals = (int)HttpContext.Session.GetInt32("Meals");
            Dojodachi Bell = new Dojodachi(Name, Happy, Full, Energy, Meals);
            return Bell;
        }
        // Saves the Dachi and all the Dachi's values into Session.
        public void Save(Dojodachi Mixy){
            HttpContext.Session.SetString("Name", Mixy.Name);
            HttpContext.Session.SetInt32("Happy", Mixy.Happy);
            HttpContext.Session.SetInt32("Full", Mixy.Full);
            HttpContext.Session.SetInt32("Energy", Mixy.Energy);
            HttpContext.Session.SetInt32("Meals", Mixy.Meals);
        }

        // Takes the current Dojodachi and puts it into the ViewBag.
        public void Bag(Dojodachi Xander){
            ViewBag.Name = Xander.Name;
            ViewBag.Happy = Xander.Happy;
            ViewBag.Full = Xander.Full;
            ViewBag.Energy = Xander.Energy;
            ViewBag.Meals = Xander.Meals;
            ViewBag.Action = TempData["Action"];
            ViewBag.Img = TempData["Img"];
        }

        // Feeds your Dojodachi
        [HttpGet]
        [Route("feed")]
        public IActionResult Feed(){
            // Summon Dachi Data
            Dojodachi Marco = Summon();
            Random rand = new Random();
            // Make sure you have food
            if(Marco.Meals > 0)
            {
                // Eat the food
                Marco.Meals -= 1;
                // Does your Dachi like the meal?
                int GoGo = rand.Next(1, 100);
                if (GoGo < 25){
                    // Nope!
                    TempData["Action"] = Marco.Name + " doesn't want anything to do with that.  Try again later.";
                    TempData["Img"] = "nope.png";
                    Save(Marco);
                    Bag(Marco);
                    return RedirectToAction("Index");
                }
                // Otherwise it eats.
                int NomNom = rand.Next(5, 10);
                Marco.Full += NomNom;
                TempData["Action"] = "You fed " + Marco.Name + " a meal. Fullness increased by " + NomNom.ToString() + " and you used up one of your meals.";
                TempData["Img"] = "eat.png";
            } else {
                // If you didn't have enough food
                TempData["Action"] = "You can't feed " + Marco.Name + " because you are out of meals! Work to get more!";
                TempData["Img"] = "hungry.png";
            }
            Save(Marco);
            // Bag(Marco);
            // Back to square one!
            return RedirectToAction("Index");
        }

        // Play with your Dachi
        [HttpGet]
        [Route("play")]
        public IActionResult Play(){
            // I SUMMON YOU, MANCY THE DOJODACHI!!! HEED MY CALL!!
            Dojodachi Mancy = Summon();
            Random rand = new Random();
            // Does Mancy have enough energy?
            if(Mancy.Energy > 4){
                // Was it fun playtime?
                int GoGo = rand.Next(1, 100);
                if (GoGo < 25){
                    // Nope.
                    TempData["Action"] = Mancy.Name + " doesn't want anything to do with that.  Try again later.";
                    TempData["Img"] = "nty.png";
                } else {
                // Otherwise...
                    Mancy.Energy -= 5;
                    int YayYay = rand.Next(5, 10);
                    Mancy.Happy += YayYay;
                    TempData["Action"] = "You played with " + Mancy.Name + ". It's happiness increased by " + YayYay + " and you used up 5 units of " + Mancy.Name +"'s energy.";
                    TempData["Img"] = "play.png";
                }
            // If Mancy had no energy...
            } else {
                TempData["Action"] = Mancy.Name + " is too tuckered out to play. It needs to have at least 5 energy. Let " + Mancy.Name + " sleep to get more!";
                TempData["Img"] = "sleepy.ng";
            }
            Save(Mancy);
            // Bag(Mancy);
            return RedirectToAction("Index");
        }

        // Work dat Dachi
        [HttpGet]
        [RouteAttribute("work")]
        public IActionResult Work(){
            // I need those numbers on my desk, stat!
            Dojodachi Malphy = Summon();
            Random rand = new Random();
            // Do you run out of coffee this morning?
            Malphy.Energy -= 5;
            int NomNom = rand.Next(1,3);
            Malphy.Meals += NomNom;
            TempData["Action"] = "Let me see dat work work work work work. " + Malphy.Name + " is pretty tuckered out after spending 5 energy, but at least it earned " + NomNom.ToString() + " meals. " + Malphy.Name + "'s a bread winner!";
            TempData["Img"] = "work.png";
            Save(Malphy);
            // Bag(Malphy);
            return RedirectToAction("Index");
        }

        // Sleep, little Dachi. You've got plenty more adventures for you in the morning.
        [HttpGet]
        [RouteAttribute("sleep")]
        public IActionResult Sleep(){
            // GET OVER HERE SO I CAN PUT YOU TO BED.
            Dojodachi Garth = Summon();
            Garth.Energy += 15;
            Garth.Happy -= 5;
            Garth.Full -= 5;
            Save(Garth);
            TempData["Action"] = "Goodnight, sweet " + Garth.Name + ". May flights of angels lead you to your rest. Gained 15 energy.";
            TempData["Img"] = "sleep.png";
            // Bag(Garth);
            return RedirectToAction("Index");
        }



    }
}
