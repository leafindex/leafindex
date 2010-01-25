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

				public string BoroughName { get { return _boroughName; } }
				public int AssaultCount { get { return _assaultCount; } }
				public int WardCount { get { return _wardCount; } }
    }
}
