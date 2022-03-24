using System;
using System.Diagnostics;
using System.Threading.Tasks;
using testmultilthread;

namespace WhenAllTest
{
    class Program
    {
        static bool ThrowExceptions = false;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }


        static async Task MainAsync(string[] args)
        {
            await PlanPartySequentially(); // ~6000ms
            await PlanPartyConcurrently(); // ~2000ms


        }



        private static async Task PlanPartyConcurrently()
        {
            Console.WriteLine("Now I'm planning one with helpers working concurrently!");
            var partyStatus = new PartyStatus();

            var timer = Stopwatch.StartNew();

            var sendInvites = SendInvites();
            var orderFood = OrderFood();
            var cleanHouse = CleanHouse();

            await Task.WhenAll(sendInvites, orderFood, cleanHouse);
            partyStatus.InvitesSent = await sendInvites;
            partyStatus.FoodCost = await orderFood;
            partyStatus.IsHouseClean = await cleanHouse;

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        private static async Task PlanPartySequentially()
        {
            Console.WriteLine("I'm planning a party!");
            var partyStatus = new PartyStatus();

            var timer = Stopwatch.StartNew();

            partyStatus.InvitesSent = await SendInvites();
            partyStatus.FoodCost = await OrderFood();
            partyStatus.IsHouseClean = await CleanHouse();

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        public static async Task<int> SendInvites()
        {
            await Task.Delay(2000);
            if (Program.ThrowExceptions) throw new Exception("Boom. Nobody is coming.");

            return 100;
        }
        public static async Task<decimal> OrderFood()
        {
            await Task.Delay(2000);

            if (Program.ThrowExceptions) throw new Exception("Boom. No food for you.");

            return 123.23m;
        }
        public static async Task<bool> CleanHouse()
        {
            await Task.Delay(2000);
            if (Program.ThrowExceptions) throw new Exception("Boom. You have small children. Forget having a clean house.");

            return true;
        }
    }
}
