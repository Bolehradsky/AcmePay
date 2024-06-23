namespace AcmePay.Test.Fixtures
{
    public static class CreditCardNumbers
    {
        static string[] creditCards = {"4111 1111 1111 1111", "4012 8888 8888 1881","5555 5555 5555 4444","5105 1051 0510 5100",
                                       "3782 8224 6310 005","3714 4963 5398 431","6011 0000 0000 0004","6011 6011 6011 6611",
                                       "3530 1113 3330 0000","3566 0020 2036 0505"};
        public static string GeRandomCreditcard()
        {
            var random = new Random();
            var randomIndex = random.Next(0, creditCards.Length);
            return creditCards[randomIndex];

        }
    }
}
