namespace SomeGenericSimpleApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint? fib_Count = null;
            do
            {
                Console.Write("Specify how many fibonacci numbers should be calculated: ");
                string? Input = Console.ReadLine();
                if (Input != null && uint.TryParse(Input, out uint num))
                {
                    fib_Count = num;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again!");
                }
            }
            while (fib_Count == null);
            uint FINAL_RESULT = CalculateFibonacciNumber((uint)(fib_Count - 1));
            string resultStr = $"The number is \"{FINAL_RESULT}\"!";
            Console.WriteLine(resultStr);
        }

        private static uint CalculateFibonacciNumber(uint fibIndex)
        {
            if (fibIndex <= 1)
            {
                return 1;
            }
            uint fibNumber = CalculateFibonacciNumber(fibIndex - 1) + CalculateFibonacciNumber(fibIndex - 2);
            return fibNumber;
        }
    }
}
