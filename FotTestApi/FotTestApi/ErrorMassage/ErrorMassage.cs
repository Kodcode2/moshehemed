namespace FotTestApi
{
    public static class ErrorMassage
    {
        public const string errorNumberHighOrLow = "It is not possile enter a number below 0 and above one 1000";
        public const string errorNotFound = "Agent by Id is not found!";
        public const string notContainsKey = "The data entered does not exist";
    }

    public class NotCool() : Exception("Not cool...") { }
	public class ErrorNumberHighOrLow() : Exception("It is not possile enter a number below 0 and above one 1000")
		 { }
}
