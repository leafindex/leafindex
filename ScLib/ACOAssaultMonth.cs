namespace ScLib
{
    public class ACOAssaultMonth
    {
		string _monthName;
		int _assaultCount;

		public ACOAssaultMonth(string monthName, int assaultCount)
        {
			_monthName = monthName;
			_assaultCount = assaultCount;
        }

		public string Month { get { return _monthName; } }
		public int Assaults { get { return _assaultCount; } }
    }
}
