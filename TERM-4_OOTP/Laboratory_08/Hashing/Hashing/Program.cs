internal class Program
{
    private static void Main(string[] args)
    {
        string input = "Password123";
        string available_symb = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
        string hash = "";
        int hash_length = 64;
        long current_val = 0;

        for (int i = 0; i < input.Length; i++)
        {
            current_val += (i + 1) * input[i] * 73;
        }

        for (int i = 0; i < hash_length; i++)
        {
            current_val = (current_val * 27 + i);
            hash += available_symb[(int)(Math.Abs(current_val) % available_symb.Length)];
        }
    }
}