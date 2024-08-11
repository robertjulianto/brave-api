using brave_api.Classes;
using brave_api.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;

namespace brave_api.Controllers
{
    [Route("api/brave")]
    public class BraveController : ControllerBase
    {
        private static readonly Dictionary<int, int> WitchKillDict = [];
        private static readonly Dictionary<int, int> Fibonaci = [];

        [HttpPost]
        public IActionResult CountKill([FromBody] BraveRequest request)
        {
            BraveResponse response = new()
            {
                Result = ModelState.IsValid && IsInputValid(request) ? FindAverage(request.PersonA, request.PersonB) : -1
            };

            return Ok(response);
        }

        private static bool IsInputValid(BraveRequest request)
        {
            return IsPersonValid(request.PersonA) && IsPersonValid(request.PersonB);
        }

        private static bool IsPersonValid(Person p)
        {
            return 
                p.YearOfDeath >= 0 
                && p.AgeOfDeath >= 0
                && p.AgeOfDeath < p.YearOfDeath;
        }

        private static decimal FindAverage(Person pA, Person pB)
        {
                int bornYearA = CalculateBornYear(pA);
                int bornYearB = CalculateBornYear(pB);
                GenerateWitchKillTable(Math.Max(bornYearA, bornYearB));
                return (decimal)(WitchKillDict[bornYearA] + WitchKillDict[bornYearB]) / 2;
        }

        private static int CalculateBornYear(Person p)
        {
            return p.YearOfDeath - p.AgeOfDeath;
        }

        private static void GenerateWitchKillTable(int nYear) 
        {
            if (WitchKillDict.ContainsKey(nYear))
                return;
            for (int i = 1; i <= nYear; i++) 
            {
                int curr_fibonaci = Fibonaci.GetValueOrDefault(i - 2, 0) +  Fibonaci.GetValueOrDefault(i - 1, 1);
                Fibonaci.TryAdd(i, curr_fibonaci);
                WitchKillDict.TryAdd(i, WitchKillDict.GetValueOrDefault(i - 1, 0) + curr_fibonaci);
            }
        }
    }
}