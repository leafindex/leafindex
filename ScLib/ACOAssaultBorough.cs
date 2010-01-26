namespace ScLib
{
    public class ACOAssaultBorough
    {
		string _boroughName;
		int _assaultCount, _wardCount;

		public ACOAssaultBorough(string boroughName, int assaultCount, int wardCount)
        {
			_boroughName = boroughName;
			_assaultCount = assaultCount;
			_wardCount = wardCount;
        }

		public string Borough { get { return _boroughName; } }
		public int Assaults { get { return _assaultCount; } }
		public int Wards { get { return _wardCount; } }
    }
}
