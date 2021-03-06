using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public async Task EnumerableExtensions_ParallelForEach()
        {
           var list = new List<string>(_cities);

           list.AddRange(_cities.Select( c=> c + "xxx") );

           var dict = new ConcurrentDictionary<string, string>();

           await list.ParallelForEach( async (index, item)=>
           {
                try
                { 
                    dict.TryAdd(item, item);
                }
                catch(Exception)
                {
                }

                await Task.CompletedTask;
           });

           Assert.AreEqual(_cities.Length*2, dict.Count);
           Assert.AreEqual("New York", dict["New York"]);
           Assert.AreEqual("Columbus", dict["Columbus"]);
           Assert.AreEqual("New Orleans", dict["New Orleans"]);
        }

       private static string[] _cities = new string[]
        {
    "New York",
    "Los Angeles",
    "Chicago",
    "Houston[3]",
    "Phoenix",
    "Philadelphia",
    "San Antonio",
    "San Diego",
    "Dallas",
    "San Jose",
    "Austin",
    "Jacksonville",
    "Fort Worth",
    "Columbus",
    "Charlotte",
    "San Francisco",
    "Indianapolis",
    "Seattle",
    "Denver[i]",
    "Washington",
    "Boston",
    "El Paso",
    "Nashville",
    "Detroit",
    "Oklahoma City",
    "Portland",
    "Las Vegas",
    "Memphis",
    "Louisville",
    "Baltimore",
    "Milwaukee",
    "Albuquerque",
    "Tucson",
    "Fresno",
    "Mesa",
    "Sacramento",
    "Atlanta",
    "Colorado Springs",
    "Omaha",
    "Raleigh",
    "Miami",
    "Long Beach",
    "Virginia Beach",
    "Oakland",
    "Minneapolis",
    "Tulsa",
    "Tampa",
    "Arlington",
    "New Orleans",
    "E Wichita",
    "Bakersfield",
    "Cleveland",
    "Aurora",
    "Anaheim",
    "Honolulu",
    "Santa Ana",
    "Riverside",
    "Corpus Christi",
    "Lexington",
    "Henderson",
    "Stockton",
    "Saint Paul",
    "Cincinnati",
    "St. Louis",
    "Pittsburgh",
    "Greensboro",
    "Lincoln",
    "Anchorage",
    "Plano",
    "Orlando",
    "Irvine",
    "Newark",
    "Durham",
    "Chula Vista",
    "Toledo	 Ohio",
    "Fort Wayne",
    "St. Petersburg",
    "Laredo	 Texas",
    "Jersey City",
    "Chandler",
    "Madison",
    "Lubbock",
    "Scottsdale",
    "Reno",
    "Buffalo",
    "Gilbert",
    "Glendale",
    "N Las Vegas",
    "Winston�Salem",
    "Chesapeake",
    "Norfolk",
    "Fremont",
    "Garland",
    "Irving",
    "Hialeah",
    "Boise",
    "Spokane",
    "Baton Rouge",
    "Tacoma",
    "San Bernardino",
    "Modesto",
    "Fontana",
    "Des Moines",
    "Moreno Valley",
    "Santa Clarita",
    "Fayetteville",
    "Birmingham",
    "Oxnard",
    "Port St. Lucie",
    "Grand Rapids",
    "Huntsville",
    "Salt Lake City",
    "Frisco",
    "Yonkers",
    "Amarillo",
    "Huntington Beach",
    "McKinney",
    "Montgomery",
    "Augusta",
    "Akron",
    "Little Rock",
    "Tempe",
    "Overland Park",
    "Grand Prairie",
    "Tallahassee",
    "Cape Coral",
    "Mobile",
    "Knoxville",
    "Shreveport",
    "Worcester",
    "Ontario",
    "Vancouver",
    "Sioux Falls",
    "Chattanooga",
    "Brownsville",
    "Fort Lauderdale",
    "Providence",
    "Newport News",
    "Rancho Cucamonga",
    "Santa Rosa",
    "E Peoria",
    "Oceanside",
    "Elk Grove",
    "Salem",
    "Pembroke Pines",
    "Eugene",
    "Garden Grove",
    "Cary",
    "Fort Collins",
    "Corona",
    "Jackson",
    "Alexandria",
    "Hayward",
    "Clarksville",
    "N Lakewood",
    "Lancaster",
    "Salinas",
    "Palmdale",
    "Hollywood",
    "Macon",
    "Kansas City",
    "Sunnyvale",
    "Pomona",
    "Killeen",
    "Escondido",
    "Naperville",
    "Bellevue",
    "Joliet",
    "Murfreesboro",
    "Midland",
    "Rockford",
    "Paterson",
    "Savannah",
    "Bridgeport",
    "Torrance",
    "McAllen",
    "Syracuse",
    "Surprise",
    "Denton",
    "Roseville",
    "Thornton",
    "Miramar",
    "Pasadena",
    "Mesquite",
    "Olathe",
    "Dayton",
    "Carrollton",
    "Waco",
    "Orange",
    "Fullerton",
    "W Valley",
    "Visalia",
    "Hampton",
    "Gainesville",
    "Warren",
    "Coral Springs",
    "Cedar Rapids",
    "Round Rock",
    "Sterling Heights",
    "Kent",
    "Santa Clara",
    "New Haven",
    "Stamford",
    "Concord",
    "Elizabeth",
    "Athens",
    "Thousand Oaks",
    "Lafayette",
    "Simi Valley",
    "Topeka",
    "Norman",
    "Fargo",
    "Wilmington",
    "Abilene",
    "Odessa",
    "Columbia",
    "Pearland",
    "Victorville",
    "Hartford",
    "Vallejo",
    "Allentown",
    "Berkeley",
    "Richardson",
    "Arvada",
    "Ann Arbor",
    "Rochester",
    "Cambridge",
    "Sugar Land",
    "Lansing",
    "Evansville",
    "College Station",
    "Fairfield",
    "Clearwater",
    "Beaumont",
    "Independence",
    "Provo",
    "W Jordan",
    "Murrieta",
    "Palm Bay",
    "El Monte",
    "Carlsbad",
    "Charleston",
    "Temecula",
    "Clovis",
    "Springfield",
    "Meridian",
    "Wminster",
    "Costa Mesa",
    "High Point",
    "Manchester",
    "Pueblo",
    "Lakeland",
    "Pompano Beach",
    "W Palm Beach",
    "Antioch",
    "Everett",
    "Downey",
    "Lowell",
    "Centennial",
    "Elgin",
    "Richmond",
    "Peoria",
    "Broken Arrow",
    "Miami Gardens",
    "Billings",
    "Jurupa Valley",
    "Sandy Springs",
    "Gresham",
    "Lewisville",
    "Hillsboro",
    "Ventura",
    "Greeley",
    "Inglewood",
    "Waterbury",
    "League City",
    "Santa Maria",
    "Tyler	",
    "Davie",
    "Lakewood",
    "Daly City",
    "Boulder",
    "Allen",
    "W Covina",
    "Sparks",
    "Wichita",
    "Green Bay",
    "San Mateo",
    "Norwalk",
    "Rialto",
    "Las Cruces",
    "Chico",
    "El Cajon",
    "Burbank",
    "S Bend",
    "Renton",
    "Vista",
    "Davenport",
    "Edinburg",
    "Tuscaloosa",
    "Carmel",
    "Spokane Valley",
    "San Angelo",
    "Vacaville",
    "Clinton",
    "Bend",
    "Woodbridge"
    };

    }
}
